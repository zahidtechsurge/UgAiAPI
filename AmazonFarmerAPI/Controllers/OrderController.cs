using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.WSDL;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using SimulatePrice;
using AmazonFarmer.Core.Application.Exceptions;
using BalanceCustomer;
using Microsoft.IdentityModel.Protocols.WsTrust;
using Org.BouncyCastle.Ocsp;
using PaymentCustomer;
using MailKit.Search;
using CreateOrder;
using Org.BouncyCastle.Asn1.X509;
using ChangeCustomerPayment;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL.Helpers;
using Microsoft.Extensions.Options;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class OrderController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        private readonly NotificationService _notificationService;
        private WsdlConfig _wsdlConfig;
        private IConfiguration _configuration;

        public OrderController(IRepositoryWrapper repoWrapper, NotificationService notificationService, IOptions<WsdlConfig> wsdlConfig,
            IConfiguration configuration) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper 
            _notificationService = notificationService;
            _wsdlConfig = wsdlConfig.Value;
            _configuration = configuration;
        }

        [HttpPost("GetOrderPaymentInfo")]
        [AllowAnonymous]
        public async Task<APIResponse> GetOrderPaymentInformation(OrderPaymentDetailRequest req)
        {
            APIResponse aPIResponse = new APIResponse();

            OrderPaymentDetailResponse response = await GetOrderPrice(req);

            aPIResponse.isError = false;
            aPIResponse.message = "Records Fetched";
            aPIResponse.response = response;
            return aPIResponse;
        }

        [HttpPost("PostTransactionAcknowledgment")]
        [AllowAnonymous]
        [Obsolete]
        public async Task<APIResponse> PostTransactionAcknowledgmentUpdate(PaymentAcknowledgmentRequest req)
        {
            tblTransaction? transaction = await _repoWrapper.OnlinePaymentRepo.getTransactionByTranAuthID(req.Tran_Auth_ID);

            if (transaction != null
                && transaction.Order != null
                && transaction.Order.OrderStatus != EOrderStatus.Deleted
                && transaction.Order.PaymentStatus == EOrderPaymentStatus.PaymentProcessing)
            {
                transaction.TransactionStatus = ETransactionStatus.Acknowledged;
                transaction = _repoWrapper.OnlinePaymentRepo.UpdateTransaction(transaction);
                _repoWrapper.OnlinePaymentRepo.AddTransactionLog(new tblTransactionLog { CreatedDateTime = DateTime.UtcNow, TransactionID = transaction.Id, TransactionStatus = transaction.TransactionStatus });

                await _repoWrapper.SaveAsync();

                await TransactionLedgeUpdate(transaction);
                await TransactionFulfilment(transaction);
            }

            return new APIResponse() { isError = false, response = "", message = "Payment Done!" };
        }

        [HttpPost("PostPlaceOrder")]
        public async Task<APIResponse> PlaceOrderDirect(PlaceOrderRequest req)
        {

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            TblOrders? order = await _repoWrapper.OrderRepo.getOrderByOrderID(req.OrderID, userID);

            if (order != null
                && order.OrderStatus != EOrderStatus.Deleted)
            {

                tblFarmerProfile profile = order.User.FarmerProfile.FirstOrDefault();
                tblPlan plan = order.Plan;
                TblOrderProducts orderProduct = order.Products.FirstOrDefault();
                List<PlanCropProductPrice> planCropProductPrices = new List<PlanCropProductPrice>();

                decimal orderPrice = 0;

                if (order.OrderType == EOrderType.Product)
                {
                    PlanCropProductPrice planCropProductPrice = await GetOrderPriceWSDL(order, plan, profile.SAPFarmerCode, planCropProductPrices);

                }
                else
                {
                    orderPrice = order.OrderAmount;
                }

                string salesOrderNumber = "";
                try
                {
                    List<TblOrders> planOrders = await _repoWrapper.OrderRepo.getAllOrderByPlanID(order.PlanID, order.CreatedByID);
                    List<TblOrders> advanceOrders = planOrders
                        .Where(po =>
                            (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)
                            && !po.IsConsumed
                            )
                        .ToList();
                    decimal alreadyPaidAdvancePayment = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.Paid
                        && (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)
                        ).Select(po => po.OrderAmount).Sum();

                    bool isLastOrder = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.NonPaid).Count() == 1 ? true : false;
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
                    if (order.OrderType == EOrderType.Product)
                    {
                        salesOrderNumber = await CreateOrderWSDL(profile.SAPFarmerCode, plan, orderProduct);
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
                order.PaymentStatus = EOrderPaymentStatus.Paid;
                order.PaymentDate = DateTime.UtcNow;
                order.PaymentDatePrice = orderPrice;
                order.SAPOrderID = salesOrderNumber;
                await _repoWrapper.OrderRepo.UpdateOrder(order);
                await _repoWrapper.SaveAsync();


                return new APIResponse() { isError = false, response = "", message = "Payment Done!" };

            }
            return new APIResponse() { isError = true, response = "", message = "No Order found!" };
        }

        //[HttpPost("DoChangeCustomerPaymentRequest")]
        //[AllowAnonymous]
        //[Obsolete]
        //public async Task ChangeCustomerPayment(ChangeCustomerPaymentRequest req)
        //{
        //    TblOrders? order = await _repoWrapper.OrderRepo.getOrderByOrderID(req.OrderID);

        //    await ChangeCustomerPaymentWSDL(order);
        //}
        private async Task TransactionFulfilment(tblTransaction transaction)
        {
            string salesOrderNumber = "";
            TblOrders order = transaction.Order;
            tblFarmerProfile profile = transaction.Order.User.FarmerProfile.FirstOrDefault();
            TblOrderProducts orderProduct = transaction.Order.Products.FirstOrDefault();
            tblPlan plan = transaction.Order.Plan;

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

                    bool isLastOrder = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.NonPaid).Count() == 0 ? true : false;
                    isLastOrder = isLastOrder && planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.PaymentProcessing).Count() == 1 ? true : false;
                    if (isLastOrder)
                    {
                        foreach (TblOrders advanceOrder in advanceOrders)
                        {
                            await ChangeCustomerPaymentWSDL(advanceOrder);
                            advanceOrder.IsConsumed = true;
                            await _repoWrapper.OrderRepo.UpdateOrder(advanceOrder);
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
        }
        private async Task TransactionLedgeUpdate(tblTransaction transaction)
        {

            tblFarmerProfile profile = transaction.Order.User.FarmerProfile.FirstOrDefault();
            TblOrderProducts orderProduct = transaction.Order.Products.FirstOrDefault();

            tblPlan plan = transaction.Order.Plan;
            string companyCode = "2000";

            //if (orderProduct != null && (transaction.OrderType == EOrderType.Product || transaction.OrderType == EOrderType.OrderReconcile))
            //{
            //    companyCode = orderProduct.Product.SalesOrg;
            //}
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
                throw;
            }

            TblOrders order = transaction.Order;

            if (order.OrderType == EOrderType.Advance || order.OrderType == EOrderType.AdvancePaymentReconcile)
                order.PaymentStatus = EOrderPaymentStatus.Paid;


            order.PaymentDate = DateTime.UtcNow;
            order.PaymentDatePrice = transaction.Amount;

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
                throw new AmazonFarmerException(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
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
                throw new AmazonFarmerException(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
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
                throw new AmazonFarmerException(wsdlResponse.ET_RETURN.FirstOrDefault().MSG);
            }

        }
        private async Task<OrderPaymentDetailResponse> GetOrderPrice(OrderPaymentDetailRequest req)
        {

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            List<TblOrders> planOrders = await _repoWrapper.OrderRepo.getAllOrderByPlanID(req.PlanID, userID);
            TblOrders planOrder = planOrders.Where(po => po.OrderID == req.OrderID).FirstOrDefault();

            decimal orderPrice = 0.0m;
            decimal payableAmount = 0.0m;

            tblFarmerProfile profile = planOrder.User.FarmerProfile.First();
            tblPlan plan = planOrder.Plan;

            string SAPFarmerCode = profile.SAPFarmerCode;

            if ((planOrder.Products == null || planOrder.Products.Count == 0)
                && planOrder.OrderType == EOrderType.Product)
                throw new AmazonFarmerException(_exceptions.pricingNotMaintained);

            //Check if the payment can be made
            if (DateTime.UtcNow > planOrder.DuePaymentDate)
                throw new AmazonFarmerException(_exceptions.orderExpired);

            if (planOrder.OrderStatus == EOrderStatus.Blocked)
                throw new AmazonFarmerException(_exceptions.orderExpired);

            if (planOrder.PaymentStatus != EOrderPaymentStatus.NonPaid)
                throw new AmazonFarmerException(_exceptions.alreadyPaid);

            int advancePercentValue = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentPercent));
            int advancePaymentBufferTime = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.AdvancePaymentBufferTime));
            int orderPaymentBufferTime = Convert.ToInt32(await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType(EConfigType.OrderPaymentBufferTime));

            bool anyAdvancePaymentProcessing = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.PaymentProcessing
               && (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)
               ).Any();

            bool isLastOrder = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.NonPaid).Count() == 1 ? true : false;
            bool isAnyPendingPayment = planOrders.Where(po => po.PaymentStatus == EOrderPaymentStatus.PaymentProcessing).Any();

            decimal alreadyPaidAdvancePayment = planOrders
                    .Where(po =>
                        po.PaymentStatus == EOrderPaymentStatus.Paid
                        && !po.IsConsumed
                        && (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)
                    ).Select(po => po.OrderAmount).Sum();

            if (isAnyPendingPayment && isLastOrder)
                throw new AmazonFarmerException(_exceptions.orderPaymentPendingLastOrder);

            if (anyAdvancePaymentProcessing)
                throw new AmazonFarmerException(_exceptions.advancePaymentNotDone);

            if (planOrder.OrderType == EOrderType.Product)
            {
                if (planOrders
                    .Where(po => (po.PaymentStatus == EOrderPaymentStatus.NonPaid || po.PaymentStatus == EOrderPaymentStatus.PaymentProcessing)
                    && (po.OrderType == EOrderType.Advance || po.OrderType == EOrderType.AdvancePaymentReconcile)).Count() == 0)
                {
                    List<PlanCropProductPrice> planCropProductPrices = new List<PlanCropProductPrice>();
                    PlanCropProductPrice planCropProductPrice = await GetOrderPriceWSDL(planOrder, plan, SAPFarmerCode, planCropProductPrices);
                    orderPrice = planCropProductPrice.TotalAmount;


                    payableAmount = orderPrice;
                    if (isLastOrder)
                    {
                        if (orderPrice < alreadyPaidAdvancePayment)
                        {
                            payableAmount = orderPrice;
                        }
                        else
                        {
                            payableAmount = orderPrice - alreadyPaidAdvancePayment;
                        }
                    }
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.advancePaymentNotDone);
                }
            }
            else if (planOrder.OrderType == EOrderType.Advance)
            {
                List<PlanCropProductPrice> planCropProductPrices = new List<PlanCropProductPrice>();

                foreach (TblOrders po in planOrders)
                {
                    if (po.OrderType == EOrderType.Product)
                    {
                        PlanCropProductPrice planCropProductPrice = await GetOrderPriceWSDL(po, plan, SAPFarmerCode, planCropProductPrices);

                        orderPrice += planCropProductPrice.TotalAmount;
                    }
                }
                orderPrice = (orderPrice * advancePercentValue) / 100;
                //For advance order do the calculation again for math.ceil.

                //making amount decimal to ceiling 
                orderPrice = Math.Ceiling(orderPrice);
                orderPrice = orderPrice * 1.00m;

                payableAmount = orderPrice;

            }
            else if (planOrder.OrderType == EOrderType.AdvancePaymentReconcile)
            {

                List<PlanCropProductPrice> planCropProductPrices = new List<PlanCropProductPrice>();
                foreach (TblOrders po in planOrders)
                {
                    if (po.OrderType == EOrderType.Product)
                    {
                        PlanCropProductPrice planCropProductPrice = await GetOrderPriceWSDL(po, plan, SAPFarmerCode, planCropProductPrices);
                        orderPrice += planCropProductPrice.TotalAmount;
                    }
                }
                orderPrice = (orderPrice * advancePercentValue) / 100;

                payableAmount = orderPrice - alreadyPaidAdvancePayment;

            }
            else if (planOrder.OrderType == EOrderType.OrderReconcile)
            {
                orderPrice = planOrder.OrderAmount;
                payableAmount = orderPrice;
            }

            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);
            ZSD_AMAZON_CUSTOMER_BAL requestWsdl = new ZSD_AMAZON_CUSTOMER_BAL
            {
                CREDIT_SEGMENT = "2013",
                CUST_NUM = profile.SAPFarmerCode

            };
            var wsdlResponse = await wSDLFunctions.CustomerBalance(requestWsdl);
            decimal customerBalance = 0;

            if (wsdlResponse != null
              && (wsdlResponse.MessageType.ToUpper() == "S" || wsdlResponse.MessageType.ToUpper() == "E")
             )
            {
                customerBalance = wsdlResponse.CustomerBalance;
                customerBalance = Math.Floor(customerBalance);
                customerBalance = customerBalance * 1.00m;
            }

            if (planOrder.OrderType == EOrderType.Advance
                   || planOrder.OrderType == EOrderType.AdvancePaymentReconcile
                   )
            {
                customerBalance = 0;
            }
            string PaymentGatewayPrefix = _configuration["PaymentGatewayPrefix"].ToString();
            OrderPaymentDetailResponse response = new OrderPaymentDetailResponse
            {
                DoPlabceOrder = payableAmount - customerBalance <= 0 ? true : false,
                PayableAmount = payableAmount - customerBalance > 0 ? payableAmount - customerBalance : 0.0m,
                OrderAmount = orderPrice,
                //TransactionID = planOrder.OrderID.ToString() + "-" + planOrder.OrderRandomTransactionID.ToString(),
                TransactionID = PaymentGatewayPrefix + planOrder.OrderID.ToString() + planOrder.OrderRandomTransactionID.ToString(), //Removed hyphen fom order id
                OrderID = planOrder.OrderID,
                AvailableBalance = customerBalance == 0 ? 0.0m : customerBalance,
            };
            return response;
        }
        private async Task<PlanCropProductPrice> GetOrderPriceWSDL(TblOrders planOrder, tblPlan plan,
                string SAPFarmerCode, List<PlanCropProductPrice> planCropProductPrices)
        {

            List<PlanCropProductPrice_Services> newServices = plan.OrderServices
                .Select(ps => new PlanCropProductPrice_Services
                {
                    ServiceCode = ps.Service.Code
                }).ToList();

            TblOrderProducts orderProduct = planOrder.Products.FirstOrDefault();

            PlanCropProductPrice? planProductPrice = planCropProductPrices
                .Where(pp => pp.ProductCode == orderProduct.Product.ProductCode
                    && pp.Services.Count == newServices.Count &&
                    pp.Services.TrueForAll(s => newServices.Exists(ns => ns.ServiceCode == s.ServiceCode)))
                .FirstOrDefault();

            decimal oldProductPrice = 0;

            if (planProductPrice == null)
            {
                RequestType request = new()
                {
                    condGp1 = plan.OrderServices.Count > 0 ? plan.OrderServices[0].Service.Code != null ? plan.OrderServices[0].Service.Code : "" : "",
                    condGp2 = plan.OrderServices.Count > 1 ? plan.OrderServices[1].Service.Code != null ? plan.OrderServices[1].Service.Code : "" : "",
                    condGp3 = plan.OrderServices.Count > 2 ? plan.OrderServices[2].Service.Code != null ? plan.OrderServices[2].Service.Code : "" : "",
                    condGp4 = plan.OrderServices.Count > 3 ? plan.OrderServices[3].Service.Code != null ? plan.OrderServices[3].Service.Code : "" : "",
                    custNum = SAPFarmerCode,
                    custRef = "Created Plan for Farm",
                    division = orderProduct.Product.Division,
                    matNum = orderProduct.Product.ProductCode,
                    saleDistict = planOrder.Warehouse.SalePoint,
                    salesOrg = orderProduct.Product.SalesOrg,
                    saleUnit = orderProduct.Product.UOM.UOM
                };
                WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

                var wsdlResponse = await wSDLFunctions.PriceSimluate(request);

                if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0
                   && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S"
                   && !string.IsNullOrEmpty(wsdlResponse.itemNum.TrimStart('0')))
                {

                    int PlanCropID = await _repoWrapper.PlanRepo.getPlanCropIDByPlanProductID(orderProduct.PlanProductID);

                    oldProductPrice = (Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal)) * orderProduct.QTY;

                    //making amount decimal to ceiling  
                    oldProductPrice = Math.Ceiling(oldProductPrice);
                    oldProductPrice = oldProductPrice * 1.00m;

                    planProductPrice = new()
                    {
                        Quantity = orderProduct.QTY,
                        UnitPrice = Convert.ToDecimal(wsdlResponse.netVal),
                        UnitTax = Convert.ToDecimal(wsdlResponse.taxVal),
                        UnitTotalAmount = Convert.ToDecimal(wsdlResponse.netVal) + Convert.ToDecimal(wsdlResponse.taxVal),
                        TotalAmount = oldProductPrice,
                        PlanCropId = PlanCropID,
                        ProductId = orderProduct.ProductID,
                        ProductCode = orderProduct.Product.ProductCode,
                        AdvancePercentValue = planOrder.AdvancePercent,
                        Services = newServices
                    };
                    planCropProductPrices.Add(planProductPrice);
                }

                if (oldProductPrice == 0)
                    throw new AmazonFarmerException(_exceptions.pricingNotMaintained);
            }
            else
            {
                decimal unitTotalPrice = Convert.ToDecimal(planProductPrice.UnitTax) + Convert.ToDecimal(planProductPrice.UnitPrice);
                planProductPrice = new()
                {
                    Quantity = orderProduct.QTY,
                    UnitPrice = Convert.ToDecimal(planProductPrice.UnitPrice),
                    UnitTax = Convert.ToDecimal(planProductPrice.UnitTax),
                    UnitTotalAmount = unitTotalPrice,
                    TotalAmount = unitTotalPrice * orderProduct.QTY,
                    PlanCropId = orderProduct.PlanProduct.PlanCropID,
                    ProductId = orderProduct.ProductID,
                    ProductCode = orderProduct.Product.ProductCode,
                    AdvancePercentValue = planOrder.AdvancePercent,
                    Services = newServices
                };
                planCropProductPrices.Add(planProductPrice);

                oldProductPrice = planProductPrice.TotalAmount;
            }

            return planProductPrice;

        }
    }



}
