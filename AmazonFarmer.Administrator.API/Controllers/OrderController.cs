using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using ChangeCustomerPayment;
using CreateOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Org.BouncyCastle.Ocsp;
using PaymentCustomer;
using System.Collections.Generic;
using System.Numerics;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        private readonly NotificationService _notificationService;
        private WsdlConfig _wsdlConfig;

        public OrderController(IRepositoryWrapper repoWrapper, NotificationService notificationService, IOptions<WsdlConfig> wsdlConfig) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper 
            _notificationService = notificationService;
            _wsdlConfig = wsdlConfig.Value;
        }
        [AllowAnonymous]
        [HttpPost("getOrders")]
        public async Task<APIResponse> GetOrders(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            List<SP_OrderDetailsResult> report = await _repoWrapper.OrderRepo.Get_OrderDetailsResults(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search,0);
            if (report != null && report.Count() > 0)
            {
                InResp.totalRecord = report.Count > 0 ? report.First().totalRows : 0;
                InResp.filteredRecord = report.Count();
                InResp.list = report.Select(x=> new GetOrdersReponse
                {
                    orderID = x.orderID.ToString().PadLeft(10,'0'),
                    sapOrderID = x.sapOrderID ?? string.Empty,
                    orderType = ConfigExntension.GetEnumDescription((EOrderType)x.orderType),
                    orderStatus = ConfigExntension.GetEnumDescription((EOrderStatus)x.orderStatus),
                    orderAmount = x.orderAmount,
                    createdOn = x.createdOn,
                    priceOnPayment = x.priceOnPayment ?? decimal.Zero,
                    paymentStatus = ConfigExntension.GetEnumDescription((EOrderPaymentStatus)x.paymentStatus),
                    deliveryStatus = ConfigExntension.GetEnumDescription((EDeliveryStatus)x.deliveryStatus),
                    planID = x.planID.ToString().PadLeft(10,'0'),
                    seasonName = x.seasonName ?? string.Empty,
                    warehouseName = x.warehouseName ?? string.Empty,
                    warehouseIncharge = x.warehouseIncharge ?? string.Empty,
                    farmerName = x.farmerName ?? string.Empty,
                    farmName = x.farmName ?? string.Empty,
                    address1 = x.address1 ?? string.Empty
                }).ToList();
            }
            else
            {
                InResp.list = new List<GetOrdersReponse>();
            }
            resp.response = InResp;
            return resp;
        }
        [AllowAnonymous]
        [HttpGet("downloadOrders")]
        public async Task<dynamic> DownloadOrders()
        {
            List<SP_OrderDetailsResult> report = await _repoWrapper.OrderRepo.Get_OrderDetailsResults(0, 0, "", "", "",1);
            var lst = report.Select(x => new GetOrdersReponse
            {
                orderID = x.orderID.ToString().PadLeft(10, '0'),
                sapOrderID = x.sapOrderID ?? string.Empty,
                orderType = ConfigExntension.GetEnumDescription((EOrderType)x.orderType),
                orderStatus = ConfigExntension.GetEnumDescription((EOrderStatus)x.orderStatus),
                orderAmount = x.orderAmount,
                createdOn = x.createdOn,
                priceOnPayment = x.priceOnPayment ?? decimal.Zero,
                paymentStatus = ConfigExntension.GetEnumDescription((EOrderPaymentStatus)x.paymentStatus),
                deliveryStatus = ConfigExntension.GetEnumDescription((EDeliveryStatus)x.deliveryStatus),
                planID = x.planID.ToString().PadLeft(10, '0'),
                seasonName = x.seasonName ?? string.Empty,
                warehouseName = x.warehouseName ?? string.Empty,
                warehouseIncharge = x.warehouseIncharge ?? string.Empty,
                farmerName = x.farmerName ?? string.Empty,
                farmName = x.farmName ?? string.Empty,
                address1 = x.address1 ?? string.Empty
            }).ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new OfficeOpenXml.ExcelPackage();
            //ExcelExtension excelExt = new ExcelExtension();
            package = ExcelExtension.generateTable(lst.Cast<dynamic>().ToList(), package, ConfigExntension.GetEnumDescription(EDocumentName.OrderReport));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=Report.xlsx");
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Concat(ConfigExntension.GetEnumDescription(EDocumentName.OrderReport), "-", DateTime.Now.ToString(), ".xlsx"));

        }
        [HttpPost("PostDoPaymentFixes")]
        [AllowAnonymous]
        [Obsolete]
        public async Task<APIResponse> DoPaymentFixes(OrderDoPaymentRequest req)
        {
            tblTransaction? transaction = await _repoWrapper.OnlinePaymentRepo.getTransactionByTranAuthID(req.Tran_Auth_ID);
            if (transaction != null
            && transaction.Order != null
            && transaction.Order.OrderStatus != EOrderStatus.Deleted)
            {
                if (transaction.TransactionStatus == ETransactionStatus.Acknowledged)
                {

                    await TransactionLedgeUpdate(transaction);
                    await TransactionFulfilment(transaction);
                }
                else if (transaction.TransactionStatus == ETransactionStatus.SapLedgerUpdated)
                {
                    await TransactionFulfilment(transaction);
                }
            }


            return new APIResponse() { isError = false, response = "", message = "Payment Done!" };
        } 
        private async Task TransactionFulfilment(tblTransaction transaction)
        {
            string salesOrderNumber = "";
            TblOrders order = transaction.Order;
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
                    if (orderPrice >= alreadyPaidAdvancePayment)
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


    }
}
