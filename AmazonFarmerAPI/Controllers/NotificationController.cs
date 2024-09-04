using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Claims;
using System.Runtime.InteropServices;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name

    public class NotificationController : ControllerBase // Inherits from ControllerBase for API controller functionality
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        public NotificationController(IRepositoryWrapper repoWrapper) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper
        }
        [HttpPost("getNotifications")]
        public async Task<APIResponse> getNotifications(DeviceNotification_ReqDTO req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp inResp = new pagination_Resp() { list = new List<DeviceNotificationDTO>() };

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            string languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from the user claims
            languageCode = languageCode ?? "EN";
            IQueryable<tblNotification> notifications = null;
            if (!string.IsNullOrEmpty(languageCode) && !string.IsNullOrEmpty(userID))
            {
                notifications = _repoWrapper.NotificationRepo.queryableNotificationByUserID(userID, languageCode);
                inResp.totalRecord = notifications.Count();
                notifications = notifications.OrderByDescending(x => x.ID).Skip(req.skip).Take(req.take);
                var temp = notifications.ToList();
                var lst = new List<DeviceNotificationDTO>();
                foreach (var item in temp)
                {
                    var lstItem = new DeviceNotificationDTO()
                    {
                        notificationID = item.ID,
                        body = item.Notification.EmailNotificationTranslations
                                .Where(t => t.LanguageCode == languageCode)
                                .Select(t => t.DeviceBody)
                                .FirstOrDefault() != null ? item.Notification.EmailNotificationTranslations
                                .Where(t => t.LanguageCode == languageCode)
                                .Select(t => t.DeviceBody)
                                .FirstOrDefault()
                                .Replace("[PlanID]", item.PlanID.ToString().PadLeft(10, '0'))
                                .Replace("[OrderID]", item.OrderID.ToString().PadLeft(10, '0'))
                                .Replace("[Order ID]", item.OrderID.ToString().PadLeft(10, '0'))
                                .Replace("[Warehouse Name]", item.Warehouse?.WarehouseTranslation.Where(rt => rt.LanguageCode == languageCode).Select(wt => wt.Text).FirstOrDefault())
                                .Replace("[UserName]", item.User.FirstName)
                                .Replace("[SendToName]", item.User.FirstName)
                                .Replace("[FarmerName]", item.User.FirstName)
                                .Replace("[Farmer]", item.User.FirstName)
                                .Replace("[ApplicationId]", item.FarmApplicationID.ToString().PadLeft(10, '0'))
                                .Replace("[AuthorityLetterNo]", item.AuthorityLetterNo)
                                .Replace("[FarmID]", item.FarmID.ToString().PadLeft(10, '0'))
                                .Replace("[FarmName]", item.FarmName)
                                .Replace("[ReasonDropDownOption]", item.Reason?.ReasonTranslation.Where(rt => rt.LanguageCode == languageCode).Select(rt => rt.Text).FirstOrDefault())
                                .Replace("[ReasonComment]", item.ReasonCommentBox)
                                .Replace("[NewPaymentDueDate]", item.NewPaymentDueDate)
                                .Replace("[PKRAmount]", item.PKRAmount)
                                .Replace("[PKR Amount]", item.PKRAmount)
                                .Replace("[WarehouseName]", item.Warehouse?.WarehouseTranslation.Where(rt => rt.LanguageCode == languageCode).Select(rt => rt.Text).FirstOrDefault())
                                .Replace("[GoogleMapLinkWithCoordinated]", item.GoogleMapLinkWithCoordinated)
                                .Replace("[Google Map link with coordinated]", $"https://www.google.com/maps?q={item.Warehouse?.latitude},{item.Warehouse?.longitude}")
                                .Replace("[PickupDate]", item.PickUPDate)
                                .Replace("[PickUP Date]", item.PickUPDate)
                                .Replace("[ConsumerNumber]", item.ConsumerNumber)
                                .Replace("[APPID]", item.FarmApplicationID.ToString().PadLeft(10, '0'))
                                .Replace("[Reasons Dropdown Option]", item.Reason?.ReasonTranslation.Where(rt => rt.LanguageCode == languageCode).Select(rt => rt.Text).FirstOrDefault())
                                .Replace("[Reason Comment Box]", item.ReasonCommentBox)
                                .Replace("[Authority Letter ID]", item.AuthorityLetterID.ToString().PadLeft(10,'0'))
                                //.Replace("[Authority Letter QTY]", item.AuthorityLetterID.ToString().PadLeft(10,'0'))
                                .Replace("<br/>", Environment.NewLine)
                                .Replace("<br>", Environment.NewLine)
                                : string.Empty,
                        // Select and cast the type of the device notification to an integer
                        typeID = (int)item.Notification.Type,
                        createdOn = item.CreatedOn,
                        isRead = item.IsClicked,
                        orderID = item.OrderID ?? 0,
                        planID = item.PlanID ?? 0,
                        farmID = item.FarmID ?? 0,
                        authorityLetterID = item.AuthorityLetterID ?? 0,
                        authorityLetterNo = item.AuthorityLetterNo ??string.Empty,
                        farmApplicationID = item.FarmApplicationID ?? 0,
                        farmName = item.FarmName ?? string.Empty,
                        newPaymentDueDate = item.NewPaymentDueDate ?? string.Empty,
                        consumerNumber = item.ConsumerNumber ?? string.Empty,
                        pKRAmount = item.PKRAmount ?? string.Empty,
                        googleMapLinkWithCoordinated = item.GoogleMapLinkWithCoordinated ?? string.Empty,
                        warehouseID = item.WarehouseID ?? 0,
                        pickUPDate = item.PickUPDate ?? string.Empty,
                        reasonsDropdownID = item.ReasonsDropdownID ?? 0,
                        reasonCommentBox = item.ReasonCommentBox ?? string.Empty
                    };
                    lst.Add(lstItem);
                }
                inResp.list = lst;

                //inResp.list = await notifications.Select(s => new DeviceNotificationDTO
                //{
                //    notificationID = s.ID,
                //    // Select the translated body text if available; otherwise, use the default body text
                //    body = s.Notification.EmailNotificationTranslations
                //                .Where(t => t.LanguageCode == languageCode)
                //                .Select(t => t.DeviceBody)
                //                .FirstOrDefault() != null ? s.Notification.EmailNotificationTranslations
                //                .Where(t => t.LanguageCode == languageCode)
                //                .Select(t => t.DeviceBody)
                //                .FirstOrDefault()
                //                .Replace("[PlanID]", s.PlanID.ToString().PadLeft(10, '0'))
                //                .Replace("[OrderID]", s.OrderID.ToString().PadLeft(10, '0'))
                //                .Replace("[UserName]", s.User.FirstName)
                //                .Replace("[SendToName]", s.User.FirstName)
                //                .Replace("[Farmer]", s.User.FirstName)
                //                .Replace("[ApplicationId]", s.FarmApplicationID.ToString().PadLeft(10, '0'))
                //                .Replace("[AuthorityLetterNo]", s.AuthorityLetterNo)
                //                .Replace("[FarmID]", s.FarmID.ToString().PadLeft(10, '0'))
                //                .Replace("[FarmName]", s.FarmName)
                //                .Replace("[ReasonDropDownOption]", s.Reason != null ? s.Reason.ReasonTranslation.Where(rt => rt.LanguageCode == languageCode).Select(rt => rt.Text).FirstOrDefault() : string.Empty)
                //                .Replace("[ReasonComment]", s.ReasonCommentBox)
                //                .Replace("[NewPaymentDueDate]", s.NewPaymentDueDate)
                //                .Replace("[PKRAmount]", s.PKRAmount)
                //                .Replace("[WarehouseName]", s.Warehouse != null ? s.Warehouse.WarehouseTranslation.Where(rt => rt.LanguageCode == languageCode).Select(rt => rt.Text).FirstOrDefault() : string.Empty)
                //                .Replace("[GoogleMapLinkWithCoordinated]", s.GoogleMapLinkWithCoordinated)
                //                .Replace("[PickupDate]", s.PickUPDate)
                //                .Replace("[ConsumerNumber]", s.ConsumerNumber)
                //                .Replace("[APPID]", s.FarmApplicationID.ToString().PadLeft(10, '0'))
                //                .Replace("[Reasons Dropdown Option]", s.Reason != null ? s.Reason.ReasonTranslation.Where(rt => rt.LanguageCode == languageCode).Select(rt => rt.Text).FirstOrDefault() : string.Empty)
                //                .Replace("[Reason Comment Box]", s.ReasonCommentBox)
                //                .Replace("<br/>", Environment.NewLine)
                //                .Replace("<br>", Environment.NewLine)
                //                : string.Empty,
                //    // Select and cast the type of the device notification to an integer
                //    typeID = (int)s.Notification.Type,
                //    createdOn = s.CreatedOn,
                //    isRead = s.IsClicked,
                //    orderID = s.OrderID ?? 0,
                //    planID = s.PlanID ?? 0
                //})
                //// Execute the query and convert the results to a list asynchronously
                //.ToListAsync();
                inResp.filteredRecord = notifications.Count();

                //resp.response = await _repoWrapper.NotificationRepo.getNotificationByUserID(userID,languageCode,req.skip,req.take);
            }
            //else if (string.IsNullOrEmpty(languageCode) && !string.IsNullOrEmpty(userID))
            //{
            //    notifications = _repoWrapper.NotificationRepo.queryableNotificationByUserID(userID);
            //    inResp.totalRecord = notifications.Count();
            //    notifications = notifications.OrderByDescending(x => x.ID).Skip(req.skip).Take(req.take);
            //    inResp.list = await notifications.Select(s => new DeviceNotificationDTO
            //    {
            //        notificationID = s.ID,
            //        // Assign the body of the device notification
            //        body = s.DeviceNotification.Body
            //        .Replace("[farmID]", s.FarmID.HasValue ? s.FarmID.Value.ToString().PadLeft(10, '0') : "")
            //        .Replace("[planID]", s.PlanID.HasValue ? s.PlanID.Value.ToString().PadLeft(10, '0') : "")
            //        .Replace("[status]", getNotificationRequestStatus(new GetNotificationRequestStatus { requestStatus = s.NotificationRequestStatus, languageCode = "EN" })),
            //        // Assign and cast the type of the device notification to an integer
            //        typeID = (int)s.DeviceNotification.Type,
            //        createdOn = s.CreatedOn,
            //        isRead = s.IsClicked,
            //        orderID = s.OrderID ?? 0,
            //        planID = s.PlanID ?? 0,
            //        farmID = s.FarmID ?? 0
            //    })
            //    // Execute the query and convert the results to a list asynchronously
            //    .ToListAsync();
            //    inResp.filteredRecord = notifications.Count();

            //    //resp.response = await _repoWrapper.NotificationRepo.getNotificationByUserID(userID, req.skip, req.take);
            //}
            resp.response = inResp;

            return resp;
        }
        [HttpPost("notificationClicked")]
        public async Task<JSONResponse> notificationClicked(NotificationClickedRequest req)
        {
            JSONResponse resp = new JSONResponse() { isError = true };
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            if (!string.IsNullOrEmpty(userID))
            {
                tblNotification notification = await _repoWrapper.NotificationRepo.getNotificationByNotificationID(req.notificationID, userID);
                if (notification != null)
                {
                    notification.IsClicked = true;
                    notification.ClickedOn = DateTime.UtcNow;
                    _repoWrapper.NotificationRepo.markNotificationAsRead(notification);
                    await _repoWrapper.SaveAsync();
                    resp.isError = false;
                }
            }

            return resp;
        }

        private static string getNotificationRequestStatus(GetNotificationRequestStatus req)
        {
            string resp = string.Empty;
            switch (req.requestStatus)
            {
                case EDeviceNotificationRequestStatus.Plan_TSOProcessing:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_RSMProcessing:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_NSMProcessing:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Approved:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Declined:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Completed:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Revert:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Removed:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Draft:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Plan_Rejected:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Order_Active:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Order_Blocked:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Order_Deleted:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Order_Expired:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Order_Locked:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_Draft:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_PendingForTSO:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_PendingforRSM:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_PendingForPatwari:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_Approved:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_Rejected:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_SendBack:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.Farm_Deleted:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.PlanChange_Default:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.PlanChange_Pending:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.PlanChange_Accept:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                case EDeviceNotificationRequestStatus.PlanChange_Declined:
                    if (req.languageCode == "EN")
                    {
                        resp = "";
                    }
                    else
                    {
                        resp = "";
                    }
                    break;
                default:
                    break;
            }
            return "";
        }
    }
}
