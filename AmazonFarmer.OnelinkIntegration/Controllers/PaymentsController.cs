
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using BalanceCustomer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using SimulatePrice;
using System.Net;
using System.Security.Claims;
using System.Transactions;
using System.Text;
using AmazonFarmer.OnelinkIntegration.Helpers;
using System;
using AmazonFarmer.WSDL.Helpers;
using Microsoft.Extensions.Options;
using FirebaseAdmin.Messaging;

namespace AmazonFarmer.OnelinkIntegration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly NotificationService _notificationService;
        private readonly SignInManager<TblUser> _signInManager;
        private readonly UserManager<TblUser> _userManager;
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;
        private WsdlConfig _wsdlConfig;
        public PaymentsController(IRepositoryWrapper repoWrapper,
            NotificationService notificationService,
            SignInManager<TblUser> signInManager,
            UserManager<TblUser> userManager,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, IOptions<WsdlConfig> wsdlConfig
            )
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _env = webHostEnvironment;
            _wsdlConfig = wsdlConfig.Value;
        }

        [HttpPost("BillInquiry")]
        public async Task<BillInquiryResponse> BillInquiry(BillInquiryRequest request)
        {
            string? username = HttpContext.Request.Headers["username"];
            string? password = HttpContext.Request.Headers["password"];
            //validate username and password
            password = string.IsNullOrEmpty(password) ? "" : password;
            username = string.IsNullOrEmpty(username) ? "" : username;

            //username = "Am@zonEngro"; //khubaibb
            //password = "@Am@z0n$engr0+=";

            bool validate = await ValidateUser(username, password);
            password = string.IsNullOrEmpty(password) ? "No Password" : "Password";

            BillInquiryResponse resp = new BillInquiryResponse();
            int BillRequestID = 0;
            try
            {
                bool validOrder = true;
                string Prefix = _configuration["Transaction:Prefix"].ToString();
                request.prefix = Prefix;
                //if (request.consumer_number.StartsWith(Prefix))
                //{
                //    request.prefix = Prefix;
                //    validOrder = true;
                //}

                if (!validate)
                {
                    throw new AmazonFarmerException("Invalid User");
                }
                // default response
                resp = new BillInquiryResponse
                {
                    reserved = "",
                    amount_after_dueDate = "+0000000000000",
                    amount_paid = "+0000000000000",
                    amount_within_dueDate = "+0000000000000",
                    billing_month = DateTime.UtcNow.ToString("yyMM"),
                    bill_status = "U",
                    consumer_Detail = "",
                    date_paid = "",
                    due_date = "",
                    response_Code = "01",
                    tran_auth_Id = "",
                    BillInquiryRequestID = BillRequestID
                };

                Int64 OrderID = 0;
                int OrderRandomNumber = 0;
                request.order_id = request.consumer_number;
                //request.order_id = request.consumer_number.Substring(request.prefix.Length);
                //BillRequestID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryRequest(request, username, password);

                if (!validOrder)
                {
                    throw new AmazonFarmerException("Invalid Order");
                }
                if (request.order_id.Contains('-'))
                {
                    OrderID = Convert.ToInt64(request.order_id.Split("-")[0]);
                    OrderRandomNumber = Convert.ToInt32(request.order_id.Split("-")[1]);
                    request.order_id = request.order_id.Split("-")[0];
                }
                else
                {
                    throw new AmazonFarmerException("Invalid Order");
                }

                TblOrders? Order = await _repoWrapper.OrderRepo.getOrderByOrderID(OrderID);

                #region Consumer Number Doesn't Exist and Invalid Consumer
                // If Consumer Number Doesn't Exist
                if (Order == null || Order.OrderRandomTransactionID != OrderRandomNumber)
                {
                    request.order_id = null;
                    BillRequestID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryRequest(request, username, password);

                    resp = new BillInquiryResponse
                    {
                        reserved = "",
                        //amount_after_dueDate = "+0000000000000",
                        amount_after_dueDate = new string(' ', 14),
                        //amount_paid = "+0000000000000",
                        amount_paid = new string(' ', 12),
                        //amount_within_dueDate = "+0000000000000",
                        amount_within_dueDate = new string(' ', 14),
                        //billing_month = DateTime.UtcNow.ToString("yyMM"),
                        billing_month = new string(' ', 4),
                        //bill_status = "U",
                        bill_status = " ",
                        //consumer_Detail = "",
                        consumer_Detail = new string(' ', 30),
                        //date_paid = "",
                        date_paid = new string(' ', 8),
                        //due_date = "",
                        due_date = new string(' ', 8),
                        response_Code = "01",
                        //tran_auth_Id = "",
                        tran_auth_Id = new string(' ', 6),
                        BillInquiryRequestID = BillRequestID
                    };
                    // save response in database
                    int BillInquiryResponseID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryResponse(resp, request.consumer_number);
                    await _repoWrapper.SaveAsync();
                    return resp;
                }
                #endregion

                else
                {
                    //Saving the billing inquiring in to DB and calling save async
                    BillRequestID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryRequest(request, username, password);

                    string CustomerName = "Default Name";
                    if (Order.User != null)
                    {
                        CustomerName = Order.User.FirstName + " " + Order.User.LastName;
                    }

                    #region Bill Already Paid
                    // if Payment already done
                    tblTransaction transaction = await _repoWrapper.OnlinePaymentRepo.getTransactionByOrderID(OrderID);
                    if (transaction != null
                        || Order.PaymentStatus != EOrderPaymentStatus.NonPaid)
                    {
                        //string amountPaid = "+" + (transaction.Amount * 100).ToString().PadLeft(13, '0');
                        string amountPaid = Math.Round(Order.PaymentDatePrice.Value * 100).ToString().PadLeft(12, '0');
                        string amountAfterWithinPaid = "+" + Math.Round(Order.PaymentDatePrice.Value * 100).ToString().PadLeft(13, '0');

                        resp = new BillInquiryResponse
                        {
                            reserved = "",
                            //amount_after_dueDate = "+0000000000000",
                            amount_after_dueDate = amountAfterWithinPaid,
                            amount_paid = amountPaid,
                            //amount_within_dueDate = "+0000000000000",
                            amount_within_dueDate = amountAfterWithinPaid,
                            billing_month = DateTime.UtcNow.ToString("yyMM"),
                            bill_status = "P",
                            consumer_Detail = CustomerName,
                            date_paid = transaction.PaidDate.ToString("yyyyMMdd"),
                            due_date = DateTime.UtcNow.AddDays(1).ToString("yyyyMMdd"),
                            //response_Code = "05",
                            response_Code = "06",
                            tran_auth_Id = transaction.Tran_Auth_ID,
                            BillInquiryRequestID = BillRequestID
                        };

                        // save response in database
                        int BillInquiryResponseID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryResponse(resp, request.consumer_number);
                        await _repoWrapper.SaveAsync();

                        return resp;
                    }
                    #endregion

                    // Simulate order pricing by order ID get payable amount
                    decimal amount = await GetOrderPriceByOrderID(Order);

                    #region Consumer Number is Expired / Blocked
                    // If Consumer Number is Expired
                    // If Order is locked or not active
                    if (Order.DuePaymentDate <= DateTime.UtcNow
                        || Order.OrderStatus != EOrderStatus.Active
                        || Order.isLocked)
                    {
                        string AmountText = "+" + (Math.Round(amount * 100).ToString()).PadLeft(13, '0');

                        resp = new BillInquiryResponse
                        {
                            reserved = "",
                            //amount_after_dueDate = "+0000000000000",
                            amount_after_dueDate = AmountText,
                            //amount_paid = "+0000000000000",
                            amount_paid = new string(' ', 12),
                            //amount_within_dueDate = "+0000000000000",
                            amount_within_dueDate = AmountText,
                            billing_month = DateTime.UtcNow.ToString("yyMM"),
                            bill_status = "B",
                            consumer_Detail = CustomerName,
                            //date_paid = "",
                            date_paid = new string(' ', 8),
                            //due_date = "",
                            due_date = DateTime.UtcNow.AddDays(1).ToString("yyyyMMdd"),
                            response_Code = "02",
                            //tran_auth_Id = "",
                            tran_auth_Id = new string(' ', 6),
                            BillInquiryRequestID = BillRequestID
                        };
                        // save response in database
                        int BillInquiryResponseID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryResponse(resp, request.consumer_number);
                        await _repoWrapper.SaveAsync();

                        return resp;
                    }
                    #endregion

                    //// Simulate order pricing by order ID
                    //double amount = await GetOrderPriceByOrderID(OrderID);

                    #region unpaid case where all checks are done
                    if (amount > 0)
                    {
                        //string AmountText = "+" + ((amount * 100).ToString()).PadLeft(13, '0');
                        string AmountTextPaid = Math.Round(amount * 100).ToString().PadLeft(12, '0');
                        string AmountText = "+" + (Math.Round(amount * 100).ToString()).PadLeft(13, '0');

                        resp = new BillInquiryResponse
                        {
                            reserved = request.reserved,
                            amount_after_dueDate = AmountText,
                            //amount_paid = AmountTextPaid,
                            amount_paid = new string(' ', 12),
                            amount_within_dueDate = AmountText,
                            billing_month = DateTime.UtcNow.ToString("yyMM"),
                            bill_status = "U",
                            consumer_Detail = CustomerName, // Farmer Name in Order
                            //date_paid = "",
                            date_paid = new string(' ', 8),
                            due_date = DateTime.UtcNow.AddDays(1).ToString("yyyyMMdd"),
                            response_Code = "00",
                            //tran_auth_Id = "",
                            tran_auth_Id = new string(' ', 6),
                            BillInquiryRequestID = BillRequestID
                        };

                        // save response in database
                        int BillInquiryResponseID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryResponse(resp, request.consumer_number);
                        await _repoWrapper.SaveAsync();

                        return resp;

                        #endregion
                    }
                    else
                    {
                        resp = new BillInquiryResponse
                        {
                            reserved = "Invalid amount",
                            amount_after_dueDate = "+0000000000000",
                            amount_paid = "+0000000000000",
                            amount_within_dueDate = "+0000000000000",
                            billing_month = DateTime.UtcNow.ToString("yyMM"),
                            bill_status = "U",
                            consumer_Detail = "",
                            date_paid = "",
                            due_date = "",
                            response_Code = "04",
                            tran_auth_Id = "",
                            BillInquiryRequestID = BillRequestID
                        };
                        // save response in database
                        int BillInquiryResponseID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryResponse(resp, request.consumer_number);
                        await _repoWrapper.SaveAsync();
                        return resp;
                    }

                }
            }
            catch (Exception ex)
            {

                // Server Error -- Something went wrong
                resp = new BillInquiryResponse
                {
                    reserved = ex.Message == null ? "Something went wrong" : ex.Message,
                    amount_after_dueDate = "",
                    amount_paid = "",
                    amount_within_dueDate = "",
                    billing_month = "",
                    bill_status = "",
                    consumer_Detail = "",
                    date_paid = "",
                    due_date = "",
                    response_Code = "03",
                    tran_auth_Id = "",
                    BillInquiryRequestID = BillRequestID
                };
                // save response in database
                if (BillRequestID > 0)
                {
                    int BillInquiryResponseID = await _repoWrapper.OnlinePaymentRepo.addBillInquiryResponse(resp, request.consumer_number);
                    await _repoWrapper.SaveAsync();
                }

                await HandleExceptionAsync(ex, BillRequestID, "BillInquiry", _env);

                return resp;
            }
            return resp;
        }


        [HttpPost("BillPayment")]
        public async Task<BillPaymentResponse> BillPayment(BillPaymentRequest request)
        {
            string? username = HttpContext.Request.Headers["username"];
            string? password = HttpContext.Request.Headers["password"];
            //validate username and password
            password = string.IsNullOrEmpty(password) ? "" : password;
            username = string.IsNullOrEmpty(username) ? "" : username;


            //validate username and password
            bool validate = await ValidateUser(username, password);
            password = string.IsNullOrEmpty(password) ? "No Password" : "Password";

            BillPaymentResponse resp = new BillPaymentResponse();
            int BillPaymentRequestID = 0;
            try
            {
                bool validOrder = true;
                string Prefix = _configuration["Transaction:Prefix"].ToString();
                request.prefix = Prefix;
                //if (request.consumer_number.StartsWith(Prefix))
                //{
                //    request.prefix = Prefix;
                //    validOrder = true;
                //}


                long OrderID = 0;
                int OrderRandomNumber = 0;
                request.orderid = request.consumer_number;
                //request.orderid = request.consumer_number.Substring(request.prefix.Length);
                //BillPaymentRequestID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentRequest(request,username,password);

                if (!validOrder)
                {
                    throw new AmazonFarmerException("Invalid Order");
                }

                // default response
                resp = new BillPaymentResponse
                {
                    reserved = "",
                    Identification_parameter = "", // Customer name in Order
                    response_Code = "01",
                    BillPaymentRequestID = BillPaymentRequestID
                };

                if (request.orderid.Contains("-"))
                {
                    OrderID = Convert.ToInt64(request.orderid.Split("-")[0]);
                    OrderRandomNumber = Convert.ToInt32(request.orderid.Split("-")[1]);
                    request.orderid = request.orderid.Split("-")[0];
                }
                else
                {
                    throw new AmazonFarmerException("Invalid Order");
                }

                TblOrders Order = await _repoWrapper.OrderRepo.getOrderByOrderID(OrderID);

                #region Consumer Number Doesn't Exist and Invalid Consumer
                // If Consumer Number Doesn't Exist
                if (Order == null || Order.OrderRandomTransactionID != OrderRandomNumber)
                {
                    request.orderid = null;
                    BillPaymentRequestID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentRequest(request, username, password);

                    resp = new BillPaymentResponse
                    {
                        //reserved = "Invalid Consumer ID",
                        reserved = "",
                        //Identification_parameter = "", // Customer name in Order
                        Identification_parameter = new string(' ', 20),
                        response_Code = "01",
                        BillPaymentRequestID = BillPaymentRequestID
                    };

                    // save response in database
                    int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                    await _repoWrapper.SaveAsync();
                    return resp;
                }
                #endregion
                else
                {
                    BillPaymentRequestID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentRequest(request, username, password);

                    string CustomerName = "Default Name";
                    if (Order.User != null)
                    {
                        CustomerName = Order.User.FirstName + " " + Order.User.LastName;
                    }

                    #region Duplicate Payment
                    // if Payment already done / duplicate
                    bool DuplciatePaymentRequest = await _repoWrapper.OnlinePaymentRepo.getDuplicateBillPaymentRequest(request.consumer_number, request.tran_auth_id, request.tran_date, request.tran_time, BillPaymentRequestID);
                    if (DuplciatePaymentRequest)
                    {
                        resp = new BillPaymentResponse
                        {
                            reserved = "Duplicate Transaction",
                            Identification_parameter = new string(' ', 20), // Customer name in Order
                            response_Code = "03",
                            BillPaymentRequestID = BillPaymentRequestID
                        };

                        // save response in database
                        int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                        await _repoWrapper.SaveAsync();
                        return resp;
                    }
                    #endregion



                    #region Bill Already Paid
                    // if Payment already done / duplicate
                    tblTransaction transaction = Order.Transactions.FirstOrDefault();
                    if (transaction != null
                        || Order.PaymentStatus != EOrderPaymentStatus.NonPaid)
                    {
                        //resp = new BillPaymentResponse
                        //{
                        //    reserved = "Duplicate Transaction",
                        //    Identification_parameter = CustomerName, // Customer name in Order
                        //    response_Code = "03",
                        //    BillPaymentRequestID = BillPaymentRequestID
                        //};
                        resp = new BillPaymentResponse
                        {
                            reserved = "Bill Already Paid",
                            Identification_parameter = new string(' ', 20), // Customer name in Order
                            response_Code = "06",
                            BillPaymentRequestID = BillPaymentRequestID
                        };

                        // save response in database
                        int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                        await _repoWrapper.SaveAsync();
                        return resp;
                    }
                    #endregion

                    //Getting the latest bill inquery response order amount for particular consumber number
                    decimal orderAmount = await _repoWrapper.OnlinePaymentRepo.getOrderPriceByComsumerNumber(request.consumer_number);

                    #region Amount mismatch from SAP

                    // Get 
                    decimal amount = orderAmount;

                    decimal RequestAmount = 0;
                    decimal.TryParse(request.Transaction_amount, out RequestAmount);
                    RequestAmount /= 100;

                    //double RequestAmount = string.IsNullOrEmpty(request.Transaction_amount) ? 0 : double.Parse(request.Transaction_amount);

                    if (string.IsNullOrEmpty(request.Transaction_amount)
                        || RequestAmount > amount
                        || RequestAmount < amount
                        || RequestAmount == 0)
                    {
                        resp = new BillPaymentResponse
                        {
                            //reserved = "Transaction Expired",
                            reserved = "",
                            //Identification_parameter = CustomerName, // Customer name in Order
                            Identification_parameter = new string(' ', 20),
                            response_Code = "02",
                            BillPaymentRequestID = BillPaymentRequestID
                        };

                        // save response in database
                        int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                        await _repoWrapper.SaveAsync();
                        return resp;
                    }

                    #endregion

                    #region Consumer Number expired
                    // If Consumer Number is Expired
                    if (Order.DuePaymentDate < DateTime.UtcNow
                        || Order.OrderStatus != EOrderStatus.Active
                        || Order.isLocked
                        )
                    {
                        resp = new BillPaymentResponse
                        {
                            //reserved = "Transaction Expired",
                            reserved = "",
                            //Identification_parameter = CustomerName, // Customer name in Order
                            Identification_parameter = new string(' ', 20),
                            response_Code = "02",
                            BillPaymentRequestID = BillPaymentRequestID
                        };

                        // save response in database
                        int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                        await _repoWrapper.SaveAsync();
                        return resp;
                    }
                    #endregion

                    // Check amount in request with order id in inquiry table
                    //double amount = 10;

                    if (amount > 0)
                    {
                        #region Success Payment
                        //string AmountText = "+" + ((amount * 100).ToString()).PadLeft(13, '0');
                        string AmountText = Math.Round(amount * 100).ToString().PadLeft(12, '0');

                        //successful transaction
                        resp = new BillPaymentResponse
                        {
                            //reserved = "Success",
                            reserved = "",
                            //Identification_parameter = CustomerName, // Customer name in Order
                            Identification_parameter = new string(' ', 20),
                            response_Code = "00",
                            BillPaymentRequestID = BillPaymentRequestID
                        };
                        // update order amount and status
                        Order.PaymentDate = DateTime.UtcNow;
                        Order.PaymentDatePrice = Convert.ToDecimal(request.Transaction_amount);
                        Order.PaymentStatus = EOrderPaymentStatus.PaymentProcessing;

                        await _repoWrapper.OrderRepo.UpdateOrder(Order);
                        await _repoWrapper.SaveAsync();

                        DateTime? TranDate = DateTime.ParseExact(request.tran_date, "yyyyMMdd", null);
                        DateTime? TranTime = DateTime.ParseExact(request.tran_time, "HHmmss", null);
                        // add data in transaction table
                        tblTransaction tran = new tblTransaction
                        {
                            Amount = amount,
                            CreatedDateTime = DateTime.UtcNow,
                            OrderID = Convert.ToInt32(request.orderid),
                            ConsumerCode = request.consumer_number,
                            Prefix = request.prefix,
                            OrderType = Order.OrderType,
                            PaidDate = TranDate != null ? TranDate.Value : DateTime.UtcNow,
                            PaidTime = TranTime != null ? TranTime.Value : DateTime.UtcNow,
                            TransactionStatus = ETransactionStatus.Pending,
                            Tran_Auth_ID = request.tran_auth_id,
                            BillPaymentRequestID = BillPaymentRequestID,
                            SAPInvoiceNumber = string.Empty,
                            SAPOrderID = string.Empty
                        };

                        await _repoWrapper.OnlinePaymentRepo.AddTransactionData(tran);
                        await _repoWrapper.SaveAsync();

                        // save response in database
                        int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                        await _repoWrapper.SaveAsync();


                        List<NotificationRequest> notifications = new();

                        NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.PaymentProcessing, "EN");
                        TblUser farmer = Order.User;
                        if (notificationDTO != null)
                        {
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
                        }


                        NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                        replacementDTO.ConsumerNumber = request.consumer_number;
                        replacementDTO.PlanID = Order.PlanID.ToString().PadLeft(10,'0');
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.PaymentProcessing;

                        if (notifications != null && notifications.Count() > 0)
                            await _notificationService.SendNotifications(notifications, replacementDTO);
                        return resp;
                        #endregion
                    }
                    else
                    {

                        resp = new BillPaymentResponse
                        {
                            //reserved = "Invalid Data",
                            reserved = "",
                            //Identification_parameter = "", // Customer name in Order
                            Identification_parameter = new string(' ', 20),
                            response_Code = "04",
                            BillPaymentRequestID = BillPaymentRequestID
                        };

                        if (BillPaymentRequestID > 0)
                        {

                            // save response in database
                            int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                            await _repoWrapper.SaveAsync();
                        }
                        return resp;
                    }

                }
            }
            catch (Exception ex)
            {
                resp = new BillPaymentResponse
                {
                    reserved = ex.Message,
                    Identification_parameter = "", // Customer name in Order
                    response_Code = "01",
                    BillPaymentRequestID = BillPaymentRequestID
                };
                if (BillPaymentRequestID > 0)
                {
                    // save response in database
                    int BillPaymentResponseID = await _repoWrapper.OnlinePaymentRepo.addBillPaymentResponse(resp);
                    await _repoWrapper.SaveAsync();
                }
                await HandleExceptionAsync(ex, BillPaymentRequestID, "BillInquiry", _env);
                return resp;
            }
        }


        private async Task<bool> ValidateUser(string username, string password)
        {
            TblUser? user = await _repoWrapper.UserRepo.getUserByUserName(username);
            if (user != null)
            {
                bool isOnelinkUser = await _userManager.IsInRoleAsync(user, "OneLink");
                if (isOnelinkUser)
                {
                    var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);

                    if (!result.Succeeded)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        private async Task<decimal> GetOrderPriceByOrderID(TblOrders order)
        {

            OrderPaymentDetailRequest req = new()
            {
                OrderID = order.OrderID,
                PlanID = order.PlanID
            };
            OrderPaymentDetailResponse resp = await GetOrderPrice(req);
            return resp.PayableAmount;
        }

        private async Task<OrderPaymentDetailResponse> GetOrderPrice(OrderPaymentDetailRequest req)
        {

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            List<TblOrders> planOrders = await _repoWrapper.OrderRepo.getAllOrderByPlanID(req.PlanID, userID);
            TblOrders planOrder = planOrders.Where(po => po.OrderID == req.OrderID).FirstOrDefault();


            decimal orderPrice = 0;

            tblFarmerProfile profile = planOrder.User.FarmerProfile.First();
            tblPlan plan = planOrder.Plan;

            string SAPFarmerCode = profile.SAPFarmerCode;

            if ((planOrder.Products == null || planOrder.Products.Count == 0)
                && (planOrder.OrderType == EOrderType.Product))
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

            decimal alreadyPaidAdvancePayment = planOrders.Where(po =>
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

                    if (isLastOrder)
                    {
                        if (orderPrice >= alreadyPaidAdvancePayment)
                        {
                            orderPrice = orderPrice - alreadyPaidAdvancePayment;
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

                orderPrice = orderPrice - alreadyPaidAdvancePayment;
            }
            else if (planOrder.OrderType == EOrderType.OrderReconcile)
            {
                orderPrice = planOrder.OrderAmount;
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
            }

            if (planOrder.OrderType == EOrderType.Advance
                   || planOrder.OrderType == EOrderType.AdvancePaymentReconcile
                   )
            {
                customerBalance = 0;
            }

            OrderPaymentDetailResponse response = new OrderPaymentDetailResponse
            {
                DoPlabceOrder = orderPrice - customerBalance <= 0 ? true : false,
                PayableAmount = orderPrice - customerBalance > 0 ? orderPrice - customerBalance : 0,
                OrderAmount = orderPrice,
                TransactionID = planOrder.OrderID.ToString() + "-" + planOrder.OrderRandomTransactionID.ToString(),
                OrderID = planOrder.OrderID
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
                    condGp1 = plan.OrderServices.Count > 0 ? plan.OrderServices[0].Service.Code : "",
                    condGp2 = plan.OrderServices.Count > 1 ? plan.OrderServices[1].Service.Code : "",
                    condGp3 = plan.OrderServices.Count > 2 ? plan.OrderServices[2].Service.Code : "",
                    condGp4 = plan.OrderServices.Count > 3 ? plan.OrderServices[3].Service.Code : "",
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

        private async Task HandleExceptionAsync(Exception exception, int requestId, string requestTypeName, IWebHostEnvironment _env)
        {
            var exceptionDetails = GetExceptionDetails(exception);
            var json = JsonConvert.SerializeObject(exceptionDetails);
            string filePath = Path.Combine(_env.ContentRootPath, $"logs/Exceptions/{requestTypeName}_{requestId}_{DateTime.UtcNow:yyyy-MM-ddThhmmss-7199222}.json");
            await WriteFileAsync(filePath, json);
        }

        private ExceptionDetails GetExceptionDetails(Exception ex)
        {
            if (ex == null) return null;

            return new ExceptionDetails
            {
                Type = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Data = ex.Data,
                InnerException = GetExceptionDetails(ex.InnerException)
            };
        }

        private async Task WriteFileAsync(string filePath, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await stream.WriteAsync(byteData, 0, byteData.Length);
            }
        }

    }
}
