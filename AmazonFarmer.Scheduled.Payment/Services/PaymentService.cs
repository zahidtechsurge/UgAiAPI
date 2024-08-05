using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities; 
using AmazonFarmer.NotificationServices.Helpers;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using ChangeCustomerPayment;
using CreateOrder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentCustomer; 

namespace AmazonFarmer.Scheduled.Payment.Services
{
    public class PaymentService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private WsdlConfig _wsdlConfig;
        private GoogleAPIConfiguration _googleApiConfig;
        private readonly NotificationService _notificationService;

        public PaymentService(IRepositoryWrapper repoWrapper, IOptions<WsdlConfig> wsdlConfig, IOptions<GoogleAPIConfiguration> googleApiConfig, NotificationService notificationService)
        {
            _repoWrapper = repoWrapper;
            _wsdlConfig = wsdlConfig.Value;
            _notificationService = notificationService;
            _googleApiConfig = googleApiConfig.Value;
        }

        public async Task PostTransactionAcknowledgmentUpdate(PaymentAcknowledgmentRequest req, IServiceScope scope, IConfiguration _configuration)
        {
            tblTransaction? transaction = await _repoWrapper.OnlinePaymentRepo.getTransactionByTranAuthID(req.Tran_Auth_ID);

            Console.WriteLine(req.ConsumerNumber);
            Console.WriteLine(transaction.ConsumerCode);

            if (transaction != null
                && transaction.Order != null
                && transaction.Order.OrderStatus != EOrderStatus.Deleted
                && transaction.Order.PaymentStatus == EOrderPaymentStatus.PaymentProcessing
                && transaction.Amount == req.Amount
                && transaction.ConsumerCode == req.ConsumerNumber)
            {
                transaction.TransactionStatus = ETransactionStatus.Acknowledged;
                transaction = _repoWrapper.OnlinePaymentRepo.UpdateTransaction(transaction);
                _repoWrapper.OnlinePaymentRepo.AddTransactionLog(new tblTransactionLog { CreatedDateTime = DateTime.UtcNow, TransactionID = transaction.Id, TransactionStatus = transaction.TransactionStatus });

                await _repoWrapper.SaveAsync();

                await TransactionLedgeUpdate(transaction);
                await TransactionFulfilment(transaction, scope, _configuration);
            }
            else
            {
                if (transaction == null || transaction.Order == null)
                {
                    throw new AmazonFarmerException("Transaction not valid");
                }
                else if (transaction.Order.OrderStatus == EOrderStatus.Deleted)
                {
                    throw new AmazonFarmerException("Order is deleted");
                }
                else if (transaction.Order.PaymentStatus != EOrderPaymentStatus.PaymentProcessing)
                {
                    throw new AmazonFarmerException("Order payment is not in processing state.");
                }
                else if (transaction.Amount != req.Amount)
                {
                    Console.WriteLine(transaction.Amount);
                    Console.WriteLine(req.Amount);
                    throw new AmazonFarmerException("Amount mismatch");
                }
                else if (transaction.ConsumerCode != req.ConsumerNumber)
                {
                    throw new AmazonFarmerException("Consumer number not valid");
                }
            }
        }

        public async Task TransactionLedgeUpdate(tblTransaction transaction)
        {
            tblFarmerProfile profile = transaction.Order.User.FarmerProfile.FirstOrDefault();
            TblOrderProducts orderProduct = transaction.Order.Products.FirstOrDefault();
            tblPlan plan = transaction.Order.Plan;
            string companyCode = "2000";


            ZSD_AMAZON_CUSTOMER_PAYMENTResponse wsdlResponse = null;
            try
            {
                wsdlResponse = await DoPaymentWSDL(transaction.Amount, profile.SAPFarmerCode, transaction.OrderType, companyCode);
            }
            catch (Exception ex)
            {
                transaction.Reason = ex.Message;
                transaction = _repoWrapper.OnlinePaymentRepo.UpdateTransaction(transaction);
                _repoWrapper.OnlinePaymentRepo.AddTransactionLog(new tblTransactionLog { CreatedDateTime = DateTime.UtcNow, TransactionID = transaction.Id, TransactionStatus = transaction.TransactionStatus });
                await _repoWrapper.SaveAsync();
                await ExceptionHandlingService.HandleExceptionAsync(ex, "DoPaymentWSDL");
                throw;
            }

            TblOrders order = transaction.Order;

            if (order.OrderType == EOrderType.Advance
                || order.OrderType == EOrderType.AdvancePaymentReconcile
                || order.OrderType == EOrderType.OrderReconcile
                )
            {
                order.PaymentStatus = EOrderPaymentStatus.Paid;
            }


            order.PaymentDate = DateTime.UtcNow;
            order.PaymentDatePrice = transaction.Amount / 100;

            order.SAPTransactionID = wsdlResponse.DOC_NUM;
            order.FiscalYear = wsdlResponse.FISCAL_YEAR;
            order.CompanyCode = wsdlResponse.ECOMPANY_CODE;

            transaction.SAPInvoiceNumber = wsdlResponse.DOC_NUM;

            if (order.OrderType == EOrderType.Product)
            {
                transaction.TransactionStatus = ETransactionStatus.SapLedgerUpdated;
            }
            else
            {
                transaction.TransactionStatus = ETransactionStatus.Fulfilled;
            }
            await _repoWrapper.OrderRepo.UpdateOrder(order);
            transaction = _repoWrapper.OnlinePaymentRepo.UpdateTransaction(transaction);
            _repoWrapper.OnlinePaymentRepo.AddTransactionLog(new tblTransactionLog { CreatedDateTime = DateTime.UtcNow, TransactionID = transaction.Id, TransactionStatus = transaction.TransactionStatus });

            await _repoWrapper.SaveAsync();


            if (order.OrderType == EOrderType.Advance
                || order.OrderType == EOrderType.AdvancePaymentReconcile
                || order.OrderType == EOrderType.OrderReconcile
                )
            {
                List<NotificationRequest> notifications = new();
                NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                NotificationDTO notificationDTO = null;
                if (order.OrderType == EOrderType.Advance
                || order.OrderType == EOrderType.AdvancePaymentReconcile)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.AdvancePaymentProcessCompleted;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.AdvancePaymentProcessCompleted, "EN");
                }
                else if (order.OrderType == EOrderType.OrderReconcile)
                {
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.OrderReconcilePaymentProcessCompleted;
                    notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OrderReconcilePaymentProcessCompleted, "EN");
                }

                if (notificationDTO != null)
                {
                    TblUser farmer = order.User;
                    //string message = notificationDTO.body;

                    var farmerEmail = new NotificationRequest
                    {
                        Type = ENotificationType.Email,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Email, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.body
                    };
                    notifications.Add(farmerEmail);

                    var farmerFCM = new NotificationRequest
                    {
                        Type = ENotificationType.FCM,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.DeviceToken, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.fcmBody
                    };
                    notifications.Add(farmerFCM);
                    var farmerSMS = new NotificationRequest
                    {
                        Type = ENotificationType.SMS,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.PhoneNumber, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.smsBody
                    };
                    var farmerDevice = new NotificationRequest
                    {
                        Type = ENotificationType.Device,
                        Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Id, Name = farmer?.FirstName } },
                        Subject = notificationDTO.title,
                        Message = notificationDTO.deviceBody
                    };
                    notifications.Add(farmerDevice);

                    replacementDTO.PKRAmount = "Rs" + transaction.Amount.ToString("N2");
                    replacementDTO.ConsumerNumber = transaction.ConsumerCode;

                    await _notificationService.SendNotifications(notifications, replacementDTO);
                }

            }

        }

        public async Task TransactionFulfilment(tblTransaction transaction, IServiceScope scope, IConfiguration _configuration)
        {
            string salesOrderNumber = "";
            TblOrders order = transaction.Order;
            TblUser user = transaction.Order.User;
            tblFarmerProfile profile = transaction.Order.User.FarmerProfile.FirstOrDefault();
            TblOrderProducts orderProduct = transaction.Order.Products.FirstOrDefault();
            tblPlan plan = transaction.Order.Plan;
            decimal orderPrice = transaction.Amount;

            if (order.OrderType == EOrderType.Product)
            {
                try
                {
                    List<TblOrders> planOrders = await _repoWrapper.OrderRepo.getAllOrderByPlanID(order.PlanID, order.CreatedByID);
                    List<TblOrders> advanceOrders = planOrders
                        .Where(po =>
                         (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)
                                && !po.IsConsumed
                        ).ToList();

                    decimal alreadyPaidAdvancePayment = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.Paid
                        && (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)
                        ).Select(po => po.OrderAmount).Sum();

                    bool isLastOrder = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.NonPaid).Count() == 0 ? true : false;
                    isLastOrder = isLastOrder && planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.PaymentProcessing).Count() == 1 ? true : false;

                    if (isLastOrder)
                    {
                        if (orderPrice >= alreadyPaidAdvancePayment)
                        {
                            foreach (TblOrders advanceOrder in advanceOrders)
                            {
                                await ChangeCustomerPaymentWSDL(advanceOrder);
                                advanceOrder.IsConsumed = true;
                                await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
                            }
                        }
                    }
                    salesOrderNumber = await CreateOrderWSDL(profile.SAPFarmerCode, plan, orderProduct);
                }
                catch (Exception ex)
                {
                    transaction.Reason = ex.Message;
                    transaction = _repoWrapper.OnlinePaymentRepo.UpdateTransaction(transaction);
                    await _repoWrapper.SaveAsync();
                    throw;
                }

                order.SAPOrderID = salesOrderNumber;
                order.PaymentStatus = EOrderPaymentStatus.Paid;
                order.OrderAmount = transaction.Amount;
                transaction.SAPOrderID = salesOrderNumber;

                transaction.TransactionStatus = ETransactionStatus.Fulfilled;
                await _repoWrapper.OrderRepo.UpdateOrder(order);
                transaction = _repoWrapper.OnlinePaymentRepo.UpdateTransaction(transaction);
                _repoWrapper.OnlinePaymentRepo.AddTransactionLog(new tblTransactionLog { CreatedDateTime = DateTime.UtcNow, TransactionID = transaction.Id, TransactionStatus = transaction.TransactionStatus });
                await _repoWrapper.SaveAsync();
            }

            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            List<NotificationRequest> notifications = new();
            NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OrderPaymentProcessCompleted, "EN");
            //string notificationMessage = notificationDTO.body;

            if (notificationDTO != null)
            {
                var farmerEmailNotification = new NotificationRequest
                {
                    Type = ENotificationType.Email,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.Email, Name = user.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.body
                };
                notifications.Add(farmerEmailNotification);

                var farmerSMSNotification = new NotificationRequest
                {
                    Type = ENotificationType.SMS,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = profile.CellNumber, Name = user.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.smsBody
                };
                notifications.Add(farmerSMSNotification);

                var farmerFCMNotification = new NotificationRequest
                {
                    Type = ENotificationType.FCM,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.DeviceToken, Name = user.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.fcmBody
                };
                notifications.Add(farmerFCMNotification);

                var farmerDeviceNotification = new NotificationRequest
                {
                    Type = ENotificationType.Device,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user.Id, Name = user.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.deviceBody
                };
                notifications.Add(farmerDeviceNotification);
            }


            NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
            replacementDTO.NotificationBodyTypeID = ENotificationBody.OrderPaymentProcessCompleted;
            replacementDTO.ConsumerNumber = transaction.ConsumerCode;
            replacementDTO.OrderID = order.OrderID.ToString().PadLeft(10, '0');
            replacementDTO.WarehouseId = order.WarehouseID.ToString();
            replacementDTO.WarehouseName = order.Warehouse.Name;
            replacementDTO.GoogleMapLinkWithCoordinated = string.Format(_googleApiConfig.ApiKey, order.Warehouse.latitude, order.Warehouse.longitude);
            replacementDTO.PickupDate = order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy");

            if (notifications != null & notifications.Count() > 0)
                await notificationService.SendNotifications(notifications, replacementDTO);
        }


        private async Task<ZSD_AMAZON_CUSTOMER_PAYMENTResponse> DoPaymentWSDL(decimal payableAmount, string SAPFarmerCode, EOrderType orderType, string companyCode)
        {
            decimal oldProductPrice = 0;
            string ePaymentText = "";
            string reasonCode = "";
            if (orderType == EOrderType.Advance || orderType == EOrderType.AdvancePaymentReconcile)
            {
                ePaymentText = EPaymentText.Initial5Percent;
                reasonCode = "FA";
            }
            else if (orderType == EOrderType.Product || orderType == EOrderType.OrderReconcile)
            {
                ePaymentText = EPaymentText.OrderPayment;
                reasonCode = "";
            }

            ZSD_AMAZON_CUSTOMER_PAYMENT request = new()
            {
                AMOUNT = payableAmount,
                AMOUNTSpecified = true,
                BASELINE_DATE = DateTime.Now.ToString(WSDLFunctions.WSDLDateFormat),
                COMPANY_CODE = "2000",
                CUST_ACC = SAPFarmerCode,
                DOC_DATE = DateTime.Now.ToString(WSDLFunctions.WSDLDateFormat),
                DOC_HEADER = "XYZ111",
                POSTING_DATE = DateTime.Now.ToString(WSDLFunctions.WSDLDateFormat),
                REASON_CODE = reasonCode,
                REF = ePaymentText,
                TEXT = ePaymentText
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            var wsdlResponse = await wSDLFunctions.CustomerPayment(request);

            if (wsdlResponse != null && wsdlResponse.ET_RETURN.Count() > 0
              && (wsdlResponse.ET_RETURN.FirstOrDefault().MSGTYP.ToUpper() == "S")
             )
            {
                return wsdlResponse;
            }
            else
            {
                throw new Exception(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
            }
        }
        private async Task<string> CreateOrderWSDL(string SAPFarmerCode, tblPlan plan, TblOrderProducts orderProduct)
        {
            decimal oldProductPrice = 0;
            ZSTR_AMA_SO_ITEM item = new ZSTR_AMA_SO_ITEM
            {
                ITEMNUM = "10",
                MATNUM = orderProduct.Product.ProductCode,
                REQQTY = orderProduct.QTY,
                UOM = orderProduct.Product.UOM.UOM
            };
            ZSTR_AMA_SO_ITEM[] items = new ZSTR_AMA_SO_ITEM[1];
            items[0] = item;
            ZSD_AMAZON_SALEORD_CRT request = new()
            {
                CONDGRP_1 = plan.OrderServices.Count > 0 ? plan.OrderServices[0].Service.Code : "",
                CONDGRP_2 = plan.OrderServices.Count > 1 ? plan.OrderServices[1].Service.Code : "",
                CONDGRP_3 = plan.OrderServices.Count > 2 ? plan.OrderServices[2].Service.Code : "",
                CONDGRP_4 = plan.OrderServices.Count > 3 ? plan.OrderServices[3].Service.Code : "",
                CUST_NUM = SAPFarmerCode,
                CUST_REF = "Created Plan for Farm",
                DIVISION = orderProduct.Product.Division,
                ITEM = items,
                REQ_DELIVERY_DATE = orderProduct.Order.ExpectedDeliveryDate.Value.ToString(WSDLFunctions.WSDLDateFormat),
                SALEPOINT = orderProduct.Order.Warehouse.SalePoint,
                SALE_ORG = orderProduct.Product.SalesOrg
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            var wsdlResponse = await wSDLFunctions.CreateOrder(request);

            if (wsdlResponse != null && wsdlResponse.ET_RETURN.Count() > 0
              && (wsdlResponse.ET_RETURN.FirstOrDefault().MSGTYP.ToUpper() == "S")
             )
            {
                return wsdlResponse.SALE_ORD;
            }
            else
            {
                throw new Exception(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
            }

        }

        private async Task<bool> ChangeCustomerPaymentWSDL(TblOrders order)
        {
            ZSD_AMAZON_CUSTOMER_PAY_CHG request = new()
            {
                COMPANY_CODE = order.CompanyCode,
                DOC_NUM = order.SAPTransactionID,
                FISCAL_YEAR = order.FiscalYear,
                REASON_CODE = "",
                TEXT = "Z041"
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            var wsdlResponse = await wSDLFunctions.ChangeCustomerPaymentRequest(request);

            if (wsdlResponse != null && wsdlResponse.ET_RETURN.Count() > 0
              && (wsdlResponse.ET_RETURN.FirstOrDefault().MSGTYP.ToUpper() == "S")
             )
            {
                return true;
            }
            else
            {
                throw new Exception(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
            }

        }
    }
}
