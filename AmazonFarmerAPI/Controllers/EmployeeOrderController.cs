using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using Google.Cloud.Vision.V1;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Numerics;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/Employee/Order")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class EmployeeOrderController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        private readonly NotificationService _notificationService;

        public EmployeeOrderController(IRepositoryWrapper repoWrapper, NotificationService notificationService) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper 
            _notificationService = notificationService;
        }

        [HttpPost("getBlockedOrders")]
        public async Task<APIResponse> getBlockedOrders(getBlockedOrders_Req req)
        {
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (!User.IsInRole("Employee") || designationID == null || designationID == 0 || designationID != (int)EDesignation.Territory_Sales_Officer)
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);
            else
            {
                APIResponse resp = new APIResponse();

                IQueryable<TblOrders> orders = await _repoWrapper.OrderRepo.getOrders();
                List<int> territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                orders = orders.Where(x => territoryIds.Contains(x.Plan.Farm.DistrictID) && x.PaymentStatus == EOrderPaymentStatus.NonPaid && x.DuePaymentDate < DateTime.UtcNow);

                pagination_Resp pagResp = new pagination_Resp()
                {
                    totalRecord = orders.Count()
                };

                orders = orders.OrderByDescending(x => x.OrderID).Skip(req.skip).Take(req.take);

                pagResp.filteredRecord = orders.Count();
                var torders = orders.ToList();

                List<getBlockedOrders_Resp> lst = new List<getBlockedOrders_Resp>();
                foreach (var order in torders)
                {
                    getBlockedOrders_Resp obj = new getBlockedOrders_Resp()
                    {
                        orderID = order.OrderID.ToString().PadLeft(10, '0'),
                        deliveryDate = order.ExpectedDeliveryDate,
                        expiredDate = order.DuePaymentDate,
                        product = "order product not found",
                        qty = 0
                    };
                    TblOrderProducts op = order.Products.FirstOrDefault();
                    if (op != null)
                    {
                        TblProduct p = op.Product;
                        if (p != null)
                        {
                            if (p.Name != null)
                            {
                                obj.product = p.Name;
                            }
                            else
                            {
                                obj.product = "product name not found";
                            }
                        }
                        else
                        {
                            obj.product = "product not found";
                        }
                        obj.qty = op.QTY;
                    }
                    lst.Add(obj);
                }
                pagResp.list = lst;

                #region Comment
                //pagResp.list = orders.Select(x => new getBlockedOrders_Resp
                //{
                //    orderID = x.OrderID.ToString().PadLeft(10, '0'),
                //    deliveryDate = x.ExpectedDeliveryDate,
                //    expiredDate = x.DuePaymentDate,
                //    product = x.Products != null ? 
                //        x.Products.FirstOrDefault().Product != null ? 
                //            x.Products.FirstOrDefault().Product.Name ?? "Product name not found" 
                //        : "Product Empty" 
                //    : "Products EMPTY",//x.Products.FirstOrDefault().Product.Name,
                //    qty = x.Products != null ? x.Products.FirstOrDefault().QTY : 0
                //}).ToList();
                #endregion

                resp.response = pagResp;

                return resp;
            }

        }

        [HttpGet("getOrderDetail/{OrderID}")]
        public async Task<APIResponse> GetOrderDetail(Int64 OrderID)
        {
            #region Auth
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (!User.IsInRole("Employee") || designationID == null || designationID == 0 || designationID != (int)EDesignation.Territory_Sales_Officer)
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);
            #endregion

            APIResponse APIResponse = new APIResponse();


            var OrderDetail = await _repoWrapper.OrderRepo.getOrderByID(OrderID);
            if (OrderDetail == null)
                throw new AmazonFarmerException(_exceptions.orderAlreadyBlocked);
            else
            {
                APIResponse = new APIResponse()
                {
                    isError = false,
                    message = string.Empty,
                    response = new
                    {
                        planID = OrderDetail.PlanID,
                        farmerName = string.Concat(OrderDetail.User.FirstName, " ", OrderDetail.User.LastName ?? string.Empty),
                        farmerPhoneNumber = OrderDetail.User.PhoneNumber,
                        farmerEmail = OrderDetail.User.Email,
                        orderExpiredDate = OrderDetail.DuePaymentDate,
                        orderDeliveryDate = OrderDetail.ExpectedDeliveryDate,
                        orderID = OrderDetail.OrderID,
                        orderType = OrderDetail.OrderType,
                        orderProductName = OrderDetail.Products?.FirstOrDefault()?.Product.Name ?? "order product not found",
                        orderProductQty = OrderDetail.Products?.FirstOrDefault()?.QTY ?? decimal.Zero,
                        orderProductUnitPrice = OrderDetail.Products?.FirstOrDefault()?.UnitPrice ?? decimal.Zero,
                        orderProductUnitTax = OrderDetail.Products?.FirstOrDefault()?.UnitTax ?? decimal.Zero,
                        orderProductUnitTotalAmount = OrderDetail.Products?.FirstOrDefault()?.UnitTotalAmount ?? decimal.Zero,
                        orderProductAmount = OrderDetail.Products?.FirstOrDefault()?.Amount ?? decimal.Zero,
                        warehouseName = OrderDetail.Warehouse.Name,
                        warehouseLocation = OrderDetail.Warehouse.Address
                    }
                };
            }

                return APIResponse;
        }
        [HttpPut("updateBlockedOrder")]
        public async Task<JSONResponse> updateBlockedOrder(updateBlockedOrder_Req req)
        {
            JSONResponse resp = new JSONResponse();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TblUser? loggedInUser = await _repoWrapper.UserRepo.getUserByUserID(userID);
            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims
            if (loggedInUser == null)
                throw new AmazonFarmerException(_exceptions.authorityLetterIDNotFound);
            else if (!User.IsInRole("Employee") || designationID == null || designationID == 0 || designationID != (int)EDesignation.Territory_Sales_Officer)
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);
            else
            {
                TblOrders order = await _repoWrapper.OrderRepo.getOrderByID(Convert.ToInt64(req.orderID.TrimStart('0')));
                List<int> territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                if (!territoryIds.Contains(order.Plan.Farm.DistrictID))
                    throw new AmazonFarmerException(_exceptions.orderNotFound);
                else
                {
                    if (req.statusID == 1)
                    {
                        order.DuePaymentDate = DateTime.ParseExact(req.nextExpiryDate, "yyyy-MM-dd", null);
                    }
                    else
                    {
                        order.OrderStatus = EOrderStatus.Blocked;
                    }
                    await _repoWrapper.OrderRepo.UpdateOrder(order);
                    await _repoWrapper.SaveAsync();

                    #region Sending Notification to farmer
                    NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                    List<NotificationRequest> notifications = new List<NotificationRequest>();
                    if (order.OrderStatus == EOrderStatus.Blocked)
                    {
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.OrderBlocked;
                        NotificationDTO notification = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OrderBlocked, order.User.FarmerProfile.FirstOrDefault().SelectedLangCode);
                        notifications = getNotifiedUsers(notification, order.User, order.PlanID.ToString(), order.OrderID.ToString(), string.Empty);
                    }
                    else
                    {
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.OrderUnblocked;
                        NotificationDTO notification = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OrderUnblocked, order.User.FarmerProfile.FirstOrDefault().SelectedLangCode);
                        notifications = getNotifiedUsers(notification, order.User, order.PlanID.ToString(), order.OrderID.ToString(), req.nextExpiryDate);
                    }

                    replacementDTO.PlanID = order.PlanID.ToString().PadLeft(10, '0');
                    replacementDTO.OrderID = order.OrderID.ToString().PadLeft(10, '0');
                    replacementDTO.NewPaymentDueDate = order.OrderStatus == EOrderStatus.Blocked ? string.Empty : DateTime.ParseExact(req.nextExpiryDate, "yyyy-MM-dd", null).ToString("dd-MM-yyyy");

                    if (notifications != null && notifications.Count() > 0)
                        await _notificationService.SendNotifications(notifications, replacementDTO);
                    #endregion
                }


            }


            return resp;
        }

        private List<NotificationRequest> getNotifiedUsers(NotificationDTO notificationDTO, TblUser user, string planID, string orderID, string? nextDueDate)
        {
            List<NotificationRequest> notifications = new();

            if (notificationDTO != null)
            {

                var farmerEmail = new NotificationRequest
                {
                    Type = ENotificationType.Email,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user?.Email, Name = user?.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.body
                };
                notifications.Add(farmerEmail);
                var farmerFCM = new NotificationRequest
                {
                    Type = ENotificationType.FCM,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user?.DeviceToken, Name = user?.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.fcmBody.Replace("[PlanID]", planID.ToString().PadLeft(10, '0'))
                                .Replace("<br/>", Environment.NewLine)
                                .Replace("[OrderID]", orderID.ToString().PadLeft(10, '0'))
                                .Replace("[New Payment Due Date]", DateTime.ParseExact(nextDueDate, "yyyy-MM-dd", null).ToString("dd-MM-yyyy"))
                };
                notifications.Add(farmerFCM);
                var farmerSMS = new NotificationRequest
                {
                    Type = ENotificationType.SMS,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user?.PhoneNumber, Name = user?.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.smsBody.Replace("[PlanID]", planID.ToString().PadLeft(10, '0'))
                                .Replace("<br/>", Environment.NewLine)
                                .Replace("[OrderID]", orderID.ToString().PadLeft(10, '0'))
                                .Replace("[New Payment Due Date]", DateTime.ParseExact(nextDueDate, "yyyy-MM-dd", null).ToString("dd-MM-yyyy"))
                };
                notifications.Add(farmerSMS);
                var farmerDevice = new NotificationRequest
                {
                    Type = ENotificationType.Device,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = user?.Id, Name = user?.FirstName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.deviceBody.Replace("[PlanID]", planID.ToString().PadLeft(10, '0'))
                                .Replace("<br/>", Environment.NewLine)
                                .Replace("[OrderID]", orderID.ToString().PadLeft(10, '0'))
                                .Replace("[New Payment Due Date]", DateTime.ParseExact(nextDueDate, "yyyy-MM-dd", null).ToString("dd-MM-yyyy"))
                };
                notifications.Add(farmerDevice);
            }
            return notifications;
            //tblDeviceNotifications? deviceNotification = await _repoWrapper.NotificationRepo.getDeviceNotificationByType(EDeviceNotificationType.Farmer_PlanStatusUpdated);
            //if (deviceNotification != null)
            //{
            //    tblNotification newNotification = new tblNotification()
            //    {
            //        ClickedOn = DateTime.UtcNow,
            //        CreatedOn = DateTime.UtcNow,
            //        DeviceNotificationID = deviceNotification.NotificationID,
            //        FarmID = null,
            //        PlanID = req.planID,
            //        OrderID = null,
            //        IsClicked = false,
            //        UserID = plan.UserID,
            //        NotificationRequestStatus = GetNotificationRequestStatus((EPlanChangeRequest)req.statusID)
            //    };
            //    _repoWrapper.NotificationRepo.addDeviceNotification(newNotification);
            //    await _repoWrapper.SaveAsync();
            //}
        }
    }
}
