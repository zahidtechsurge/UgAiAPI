using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmerAPI.Extensions;
using CreateCustomer;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using System;
using System.Globalization;
using System.IdentityModel.Claims;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AmazonFarmerAPI.Controllers
{ /// <summary>
  /// Controller for managing authority letter operations.
  /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Employee/Farm")]
    public class EmployeeFarmController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly NotificationService _notificationService;

        private WsdlConfig _wsdlConfig;
        // Constructor injection of IRepositoryWrapper.
        public EmployeeFarmController(IRepositoryWrapper repoWrapper, NotificationService notificationService, IOptions<WsdlConfig> wsdlConfig)
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _wsdlConfig = wsdlConfig.Value;
        }
        [HttpPost("getFarmApplications")]
        public async Task<APIResponse> getFarmApplications(getFarmApplications_Req req)
        {


            // Get the user ID from the token
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims

            APIResponse resp = new APIResponse();
            pagination_Resp pagResp = new pagination_Resp();
                var farms = await _repoWrapper.FarmRepo.getFarmApplications();

                List<int> territoryIds = new List<int>();
                if (designationID == (int)EDesignation.Territory_Sales_Officer)
                {
                    territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                    farms = farms.Where(f => territoryIds.Contains(f.DistrictID));
                    if (req.requestTypeID == (int)EFarmStatus.Approved)
                    {
                        farms = farms.Where(f => f.Status == EFarmStatus.Approved);
                    }
                    else if (req.requestTypeID == (int)EFarmStatus.PendingForTSO)
                    {
                        farms = farms.Where(f => (f.Status == EFarmStatus.PendingForTSO));
                    }
                    else if (req.requestTypeID == (int)EFarmStatus.PendingForPatwari)
                    {
                        farms = farms.Where(f => (f.Status == EFarmStatus.PendingForPatwari));
                    }
                    else
                    {
                        farms = farms.Where(f => (f.Status == EFarmStatus.PendingForTSO || f.Status == EFarmStatus.PendingForPatwari));
                    }
                }
                else if (designationID == (int)EDesignation.Regional_Sales_Manager)
                {
                    territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
                    farms = farms.Where(f => territoryIds.Contains(f.DistrictID));
                    if (req.requestTypeID == (int)EFarmStatus.Approved)
                    {
                        farms = farms.Where(f => f.Status == EFarmStatus.Approved);
                    }
                    else
                    {
                        farms = farms.Where(f => f.Status == EFarmStatus.PendingforRSM);
                    }
                }
                //get farm applications


                #region Search by Date Range
                if (!string.IsNullOrEmpty(req.startDate) && !string.IsNullOrEmpty(req.endDate))
                {
                    farms = farms.Where(x =>
                        x.CreatedOn >= DateTime.ParseExact(req.startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) &&
                        x.CreatedOn <= DateTime.ParseExact(req.endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1)
                    );
                }
                else if (!string.IsNullOrEmpty(req.startDate))
                {
                    farms = farms.Where(x => x.CreatedOn >= DateTime.ParseExact(req.startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                }
                else if (!string.IsNullOrEmpty(req.endDate))
                {
                    farms = farms.Where(x => x.CreatedOn <= DateTime.ParseExact(req.endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                }
                #endregion

                var farmList = farms.OrderByDescending(x => x.ApplicationID).ToList();

                var query = farmList
                .GroupBy(x => x.ApplicationID)
                .Select(group => group.First())
                .Select(x => new
                {
                    ApplicationID = x.ApplicationID,
                    Farmer = x.Users
                }).ToList();

                if (!string.IsNullOrEmpty(req.search))
                {
                    query = query.Where(x =>
                        x.ApplicationID.ToString().PadLeft(10, '0').Contains(req.search.ToLower()) ||
                        (x.Farmer.FirstName.ToLower() + " " + x.Farmer.LastName.ToLower()).Contains(req.search.ToLower()) ||
                        x.Farmer.FarmerProfile.FirstOrDefault().CNICNumber.ToLower().Contains(req.search.ToLower()) ||
                        x.Farmer.PhoneNumber.ToLower().Contains(req.search.ToLower())
                    ).ToList();
                }

                var result = query
                    .Skip(req.skip)
                    .Take(req.take)
                    .ToList();

                // Now, convert the result to farmApplication_List objects
                var lst = result.Select(x => new farmApplication_List
                {
                    applicationID = x.ApplicationID.ToString().PadLeft(10, '0'),
                    farmerName = $"{x.Farmer.FirstName} {x.Farmer.LastName}",
                    farmerCNIC = x.Farmer.FarmerProfile.FirstOrDefault().CNICNumber,
                    farmerPhone = x.Farmer.PhoneNumber
                }).ToList();

                pagResp = new pagination_Resp()
                {
                    filteredRecord = lst.Count(),
                    totalRecord = lst.Count(),
                    list = lst
                };
                resp.response = pagResp;
            return resp;
        }
        //Endpoint for getFarmRegistrationRequest
        [HttpPost("getFarmRegistrationRequest")]
        public async Task<APIResponse> getFarmRegistrationRequest(farmSetup_Resp req)
        {

            // Get the user ID from the token
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            APIResponse resp = new APIResponse(); // Initializing API response object

            string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims
            if (string.IsNullOrEmpty(req.applicationID)) // Checking if application ID is provided
                throw new AmazonFarmerException(_exceptions.getExceptionByLanguageCode(languageCode, "applicationIDRequired")); // Throws exception if application ID is not provided
            IQueryable<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmsByApplicationIDandLanguageCode(); // Retrieving farms 

            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims


            List<int> territoryIds = new List<int>();

            if (designationID == (int)EDesignation.Territory_Sales_Officer)
            {
                territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                farms = farms.Where(f => territoryIds.Contains(f.DistrictID));
                if (req.requestTypeID == (int)EFarmStatus.Approved)
                {
                    farms = farms.Where(f => f.Status == EFarmStatus.Approved);
                }
                else if (req.requestTypeID == (int)EFarmStatus.PendingForTSO)
                {
                    farms = farms.Where(f => (f.Status == EFarmStatus.PendingForTSO));
                }
                else if (req.requestTypeID == (int)EFarmStatus.PendingForPatwari)
                {
                    farms = farms.Where(f => (f.Status == EFarmStatus.PendingForPatwari));
                }
                else
                {
                    farms = farms.Where(f => (f.Status == EFarmStatus.PendingForTSO || f.Status == EFarmStatus.PendingForPatwari));
                }

            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager)
            {
                territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
                farms = farms.Where(f => territoryIds.Contains(f.DistrictID));
                if (req.requestTypeID == (int)EFarmStatus.Approved)
                {
                    farms = farms.Where(f => f.Status == EFarmStatus.Approved);
                }
                else
                {
                    farms = farms.Where(f => f.Status == EFarmStatus.PendingforRSM);
                }
            }

            var farmlist = farms
                .Where(x =>
                    x.ApplicationID == Convert.ToInt32(req.applicationID.TrimStart('0'))
                )
                .ToList();

            resp.response = farmlist
            .Select(x => new farmRegistrationRequest_Resp
            {
                farmID = x.FarmID,
                applicationID = x.ApplicationID,
                isPrimary = x.isPrimary,
                acreage = x.Acreage,
                address1 = x.Address1,
                address2 = x.Address2,
                cityID = x.CityID,
                city = x.City.CityLanguages.Where(y => y.LanguageCode == languageCode).FirstOrDefault().Translation,
                districtID = x.DistrictID,
                district = x.District.DistrictLanguages.Where(y => y.LanguageCode == languageCode).FirstOrDefault().Translation,
                farmName = x.FarmName,
                isLeased = x.isLeased,
                tehsilID = x.TehsilID,
                tehsil = x.Tehsil.TehsilLanguagess.Where(y => y.LanguageCode == languageCode).FirstOrDefault().Translation,
                requestType = x.Status,
                statusID = (int)x.Status,
                attachments = x.FarmAttachments.Select(y => new uploadAttachmentResp
                {
                    id = y.Attachment.ID,
                    type = y.Attachment.FileType,
                    name = y.Attachment.Name,
                    guid = y.Attachment.Guid.ToString()
                }).ToList(),
                farmerProfile = new farmerProfileDTO
                {
                    firstName = x.Users.FirstName,
                    LastName = x.Users.LastName,
                    email = x.Users.Email,
                    phone = x.Users.PhoneNumber,
                    dateOfBirth = x.Users.FarmerProfile.FirstOrDefault().DateOfBirth,
                    fatherName = x.Users.FarmerProfile.FirstOrDefault().FatherName,
                    strnNumber = x.Users.FarmerProfile.FirstOrDefault().STRNNumber,
                    cnicNumber = x.Users.FarmerProfile.FirstOrDefault().CNICNumber,
                    cnicAttachment = x.Users.UserAttachments
                    .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document)
                    .Select(y => new uploadAttachmentResp
                    {
                        id = y.Attachment.ID,
                        type = y.Attachment.FileType,
                        name = y.Attachment.Name,
                        guid = y.Attachment.Guid.ToString()
                    }).ToList(),
                    ntnNumber = x.Users.FarmerProfile.FirstOrDefault().NTNNumber,
                    ntnAttachment = x.Users.UserAttachments
                    .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document)
                    .Select(y => new uploadAttachmentResp
                    {
                        id = y.Attachment.ID,
                        type = y.Attachment.FileType,
                        name = y.Attachment.Name,
                        guid = y.Attachment.Guid.ToString()
                    }).ToList(),
                }
            }).ToList();
            return resp; // Returning the API response
        }

        [HttpPost("getFarmDetail")]
        public async Task<APIResponse> getFarmDetail(farmDetail_Req req)
        {
            APIResponse resp = new APIResponse();
            if (req.farmID == 0)
                throw new AmazonFarmerException(_exceptions.farmIDRequired);
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving user ID from user claims
                string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                    throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);
                else
                {
                    List<int> territoryIds = new List<int>();
                    int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims
                    tblfarm farm = await _repoWrapper.FarmRepo.getFarmByFarmID(req.farmID);
                    if (farm == null)
                    {
                        throw new AmazonFarmerException(_exceptions.farmNotFound);
                    }
                    else if (!User.IsInRole("Employee")) // add condition to check employee is authorized to get the farm details or not
                        throw new AmazonFarmerException(_exceptions.farmNotFound);
                    else
                    {
                        if (designationID == (int)EDesignation.Territory_Sales_Officer)
                        {
                            territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
                            if (!territoryIds.Contains(farm.DistrictID))
                            {
                                throw new AmazonFarmerException(_exceptions.farmNotFound);
                            }
                        }
                        if (designationID == (int)EDesignation.Regional_Sales_Manager)
                        {
                            territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
                            if (!territoryIds.Contains(farm.DistrictID))
                            {
                                throw new AmazonFarmerException(_exceptions.farmNotFound);
                            }
                        }
                        farm.FarmAttachments = await _repoWrapper.FarmRepo.getFarmAttachmentsByFarmID(req.farmID);
                        resp.response = new farmerScreen_farmDetail_Resp()
                        {
                            cityID = farm.CityID,
                            districtID = farm.DistrictID,
                            tehsilID = farm.TehsilID,
                            farmID = farm.FarmID,
                            farmName = farm.FarmName,
                            acreage = farm.Acreage,
                            address1 = farm.Address1,
                            address2 = farm.Address2,
                            city = farm.City.Name,
                            district = farm.District.Name,
                            tehsil = farm.Tehsil.Name,
                            isLeased = farm.isLeased,
                            isPrimary = farm.isPrimary,
                            sapFarmID = farm.SAPFarmCode.PadLeft(10, '0'),
                            status = farm.Status,
                            farmerComment = farm.FarmerComment,
                            attachments = farm.FarmAttachments.Where(x => x.Status == EActivityStatus.Active).Select(x => new uploadAttachmentResp
                            {
                                id = x.Attachment.ID,
                                type = x.Attachment.FileType,
                                name = x.Attachment.Name,
                                guid = x.Attachment.Guid.ToString()
                            }).ToList(),
                            farmerProfile = new farmerProfileDTO
                            {
                                firstName = farm.Users.FirstName,
                                LastName = farm.Users.LastName,
                                email = farm.Users.Email,
                                phone = farm.Users.PhoneNumber,
                                dateOfBirth = farm.Users.FarmerProfile.FirstOrDefault().DateOfBirth,
                                fatherName = farm.Users.FarmerProfile.FirstOrDefault().FatherName,
                                strnNumber = farm.Users.FarmerProfile.FirstOrDefault().STRNNumber,
                                cnicNumber = farm.Users.FarmerProfile.FirstOrDefault().CNICNumber,
                                cnicAttachment = farm.Users.UserAttachments
                                    .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document && y.Status == EActivityStatus.Active)
                                    .Select(y => new uploadAttachmentResp
                                    {
                                        id = y.Attachment.ID,
                                        type = y.Attachment.FileType,
                                        name = y.Attachment.Name,
                                        guid = y.Attachment.Guid.ToString()
                                    }).ToList(),
                                ntnNumber = farm.Users.FarmerProfile.FirstOrDefault().NTNNumber,
                                ntnAttachment = farm.Users.UserAttachments
                                    .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document && y.Status == EActivityStatus.Active)
                                    .Select(y => new uploadAttachmentResp
                                    {
                                        id = y.Attachment.ID,
                                        type = y.Attachment.FileType,
                                        name = y.Attachment.Name,
                                        guid = y.Attachment.Guid.ToString()
                                    }).ToList(),
                            }
                        };
                    }
                }
            }

            return resp;
        }
        //Endpoint for farmRegistrationRequest_ChangeStatus
        [HttpPost("farmRegistrationRequest_ChangeStatus")]
        public async Task<APIResponse> farmRegistrationRequest_ChangeStatus(farmRegistrationRequest_ChangeStatus_Req req)
        {
            APIResponse resp = new APIResponse(); // Initializing API response object

            if (!User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);


            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims

            var languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims
            var roleID = User.FindFirst("roleID")?.Value; // Retrieving roleID from user claims
            var role = await _repoWrapper.UserRepo.getRoleByRoleID(roleID);
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving user ID from user claims

            List<int> territoryIds = new List<int>();
            if (designationID == (int)EDesignation.Territory_Sales_Officer)
            {
                territoryIds = await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID);
            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager)
            {
                territoryIds = await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID);
            }

            if (string.IsNullOrEmpty(req.farmID)) // Checking if farm ID is provided
                throw new AmazonFarmerException(_exceptions.farmIDRequired); // Throws exception if farm ID is not provided
            else if (req.statusID == 0) // Checking if status ID is provided
                throw new AmazonFarmerException(_exceptions.statusIDRequired); // Throws exception if status ID is not provided
            else if (string.IsNullOrEmpty(userID)) // Checking if user ID is provided
                throw new AmazonFarmerException(_exceptions.userIDNotFound); // Throws exception if user ID is not found
            else
            {
                List<NotificationRequest> notifications = new();
                tblfarm farm = await _repoWrapper.FarmRepo.getFarmByFarmIDForEmployee(Convert.ToInt32(req.farmID.TrimStart('0')), territoryIds);
                if (farm == null)
                {
                    throw new AmazonFarmerException(_exceptions.farmNotFound);
                }
                NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                replacementDTO.FarmName = farm.FarmName;

                if ((req.statusID == (int)EFarmStatus.SendBack || req.statusID == (int)EFarmStatus.Rejected) && string.IsNullOrEmpty(req.revertedReason))
                {
                    throw new AmazonFarmerException(_exceptions.reasonRequired);
                }
                else if ((req.statusID == (int)EFarmStatus.SendBack || req.statusID == (int)EFarmStatus.Rejected) && req.reasonID == 0)
                {
                    throw new AmazonFarmerException(_exceptions.reasonIDNotFound);
                }
                else if (req.statusID == (int)EFarmStatus.Approved)
                {

                    if (farm.Status == EFarmStatus.PendingForTSO && designationID == (int)EDesignation.Territory_Sales_Officer)
                    {
                        farm.Status = EFarmStatus.PendingforRSM;
                    }
                    else if (farm.Status == EFarmStatus.PendingforRSM && designationID == (int)EDesignation.Regional_Sales_Manager)
                    {
                        farm.Status = EFarmStatus.PendingForPatwari;
                    }
                    else if (farm.Status == EFarmStatus.PendingForPatwari && designationID == (int)EDesignation.Territory_Sales_Officer)
                    {
                        TblUser user = farm.Users;
                        tblFarmerProfile profile = user.FarmerProfile.FirstOrDefault();

                        ResponseType? wsdlResponse = await CallCreateCustomerWSDL(profile, user);

                        if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0 && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S")
                        {
                            profile.SAPFarmerCode = wsdlResponse.custNum.ToString();
                        }
                        if (string.IsNullOrEmpty(profile.SAPFarmerCode))
                        {
                            throw new AmazonFarmerException(_exceptions.createCustomerFailure);
                        }

                        await SendNotificationForFormApproval(user.FirstName + " " + user.LastName, user.Email, profile.CellNumber,
                            user.DeviceToken, replacementDTO.FarmName, user.Id, farm.ApplicationID.ToString().PadLeft(10, '0'), profile.SelectedLangCode);


                        farm.Status = EFarmStatus.Approved;
                        farm.isApproved = true;
                        profile.isApproved = EFarmerProfileStatus.Approved;
                        await _repoWrapper.UserRepo.approverFarmerProfile(profile);
                    }
                    else
                    {
                        throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    }

                    if (farm.Status == EFarmStatus.PendingForTSO || farm.Status == EFarmStatus.PendingForPatwari)
                    {
                        List<TblUser> employees = await _repoWrapper.UserRepo.getTSOsByFarmID(farm.FarmID);
                        foreach (var item in employees)
                        {
                            notifications.AddRange(await SendNotificationForStatusChange(item.FirstName,
                                item.Email, item.PhoneNumber, item.DeviceToken,
                                farm.FarmName, ConfigExntension.GetEnumDescription(farm.Status), item.Id, farm.ApplicationID.ToString().PadLeft(10, '0')));
                            //notifications.AddRange(empNotification);
                            replacementDTO.ApplicationId = farm.ApplicationID.ToString().PadLeft(10, '0');
                            replacementDTO.NotificationBodyTypeID = ENotificationBody.Employee_FarmApplicationPendingForApproval;


                            //replacementDTO.FarmName = farm.FarmName;
                        }
                    }
                    else if (farm.Status == EFarmStatus.PendingforRSM)
                    {
                        List<TblUser> employees = await _repoWrapper.UserRepo.getRSMsByFarmID(farm.FarmID);
                        foreach (var item in employees)
                        {
                            notifications.AddRange(await SendNotificationForStatusChange(item.FirstName,
                                item.Email, item.PhoneNumber, item.DeviceToken,
                                farm.FarmName, ConfigExntension.GetEnumDescription(farm.Status), item.Id, farm.ApplicationID.ToString().PadLeft(10, '0')));

                            replacementDTO.NotificationBodyTypeID = ENotificationBody.Employee_FarmApplicationPendingForApproval;
                            replacementDTO.ApplicationId = farm.ApplicationID.ToString().PadLeft(10, '0');
                            //notifications.AddRange(empNotification);

                            //replacementDTO.FarmName = farm.FarmName;
                        }

                    }
                    TblUser farmer = await _repoWrapper.UserRepo.getFarmerByFarmApplicationID(farm.ApplicationID!.Value);
                    NotificationDTO notificationDTO = null;// await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.farmApplicationSubmittedForApproval, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);


                    if (farm.Status != EFarmStatus.Approved && notificationDTO != null && farmer != null && !string.IsNullOrEmpty(notificationDTO.body) && !string.IsNullOrEmpty(notificationDTO.title))
                    {
                        if (notificationDTO != null)
                        {
                            var farmerEmail = new NotificationRequest
                            {
                                Type = ENotificationType.Email,
                                Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Email, Name = farmer.FirstName } },
                                Subject = notificationDTO.title,
                                Message = notificationDTO.body
                                    .Replace("[FarmName]", farm.FarmName)
                                    .Replace("[APPID]", farm.ApplicationID.ToString().PadLeft(10, '0'))
                            };
                            notifications.Add(farmerEmail);
                            var farmerFCM = new NotificationRequest
                            {
                                Type = ENotificationType.FCM,
                                Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.DeviceToken, Name = farmer.FirstName } },
                                Subject = notificationDTO.title,
                                Message = notificationDTO.fcmBody
                                        .Replace("[FarmName]", farm.FarmName)
                                        .Replace("[APPID]", farm.ApplicationID.ToString().PadLeft(10, '0'))
                            };
                            notifications.Add(farmerFCM);
                            var farmerSMS = new NotificationRequest
                            {
                                Type = ENotificationType.SMS,
                                Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.PhoneNumber, Name = farmer.FirstName } },
                                Subject = notificationDTO.title,
                                Message = notificationDTO.smsBody
                                        .Replace("[FarmName]", farm.FarmName)
                                        .Replace("[APPID]", farm.ApplicationID.ToString().PadLeft(10, '0'))
                            };
                            notifications.Add(farmerSMS);
                            var farmerDevice = new NotificationRequest
                            {
                                Type = ENotificationType.Device,
                                Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Id, Name = farmer.FirstName } },
                                Subject = notificationDTO.title,
                                Message = notificationDTO.deviceBody
                                        .Replace("[FarmName]", farm.FarmName)
                                        .Replace("[APPID]", farm.ApplicationID.ToString().PadLeft(10, '0'))
                            };
                            notifications.Add(farmerDevice);
                            replacementDTO.NotificationBodyTypeID = ENotificationBody.farmApplicationSubmittedForApproval;
                        }
                        
                    }

                    await _repoWrapper.FarmRepo.changeFarmRegistrationStatus(farm, userID); // Changing farm registration status
                    await _repoWrapper.SaveAsync();
                    //if (notifications != null && notifications.Count() > 0)
                    //    await _notificationService.SendNotifications(notifications, replacementDTO);
                    resp.message = "Farm has been " + ((EFarmStatus)req.statusID).ToString(); // Setting response message
                }
                else
                {
                    tblReasons reason = await _repoWrapper.ReasonRepo.getReasonByReasonID(req.reasonID);

                    if (reason == null)
                        throw new AmazonFarmerException(_exceptions.reasonNotFound);
                    NotificationDTO notificationDTO = new NotificationDTO();
                    List<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmerAllFarmsByApplicationID(farm.ApplicationID!.Value);
                    TblUser farmer = await _repoWrapper.UserRepo.getFarmerByFarmApplicationID(farm.ApplicationID!.Value);

                    if (farmer != null && req.statusID == (int)EFarmStatus.SendBack)
                    {
                        notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.farmSendBackByEmployee, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.farmSendBackByEmployee;
                        if (farms.Count() > 0)
                        {
                            if (!farms.Any(x => x.Status == EFarmStatus.Approved))
                            {

                                farmer.FarmerProfile.First().isApproved = EFarmerProfileStatus.Editable;
                                await _repoWrapper.UserRepo.updateFarmerProfile(farmer.FarmerProfile.First());
                            }
                        }
                    }
                    if (req.statusID == (int)EFarmStatus.Rejected)
                    {
                        notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.farmRejectionByEmployee, farmer.FarmerProfile.FirstOrDefault().SelectedLangCode);
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.farmRejectionByEmployee;
                    }
                    //NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.farmChangeRequest);

                    if (notificationDTO != null && farmer != null && !string.IsNullOrEmpty(notificationDTO.body) && !string.IsNullOrEmpty(notificationDTO.title))
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
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.DeviceToken, Name = farmer.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.fcmBody
                        };
                        notifications.Add(farmerFCM);
                        var farmerSMS = new NotificationRequest
                        {
                            Type = ENotificationType.SMS,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.PhoneNumber, Name = farmer.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.smsBody
                        };
                        notifications.Add(farmerSMS);
                        var farmerDevice = new NotificationRequest
                        {
                            Type = ENotificationType.Device,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmer?.Id, Name = farmer.FirstName } },
                            Subject = notificationDTO.title,
                            Message = notificationDTO.deviceBody
                        };
                        notifications.Add(farmerDevice);

                        replacementDTO.ApplicationId = farm.ApplicationID.ToString().PadLeft(10, '0');
                        //replacementDTO.FarmName = farm.FarmName;
                        replacementDTO.ReasonDropDownOptionId = reason.ReasonTranslation.Where(x => x.LanguageCode == "EN").FirstOrDefault() != null  ? reason.ReasonTranslation.Where(x => x.LanguageCode == "EN").FirstOrDefault().ReasonID.ToString() : string.Empty;
                        replacementDTO.ReasonDropDownOption = reason.ReasonTranslation.Where(x => x.LanguageCode == "EN").FirstOrDefault().Text;
                        replacementDTO.ReasonComment = req.revertedReason;
                    }


                    farm.ReasonID = req.reasonID;
                    farm.RevertedReason = req.revertedReason;
                    farm.Status = (EFarmStatus)req.statusID;
                    await _repoWrapper.FarmRepo.changeFarmRegistrationStatus(farm, userID); // Changing farm registration status
                    await _repoWrapper.SaveAsync();

                    //Send Notification to Farmer for application approval.

                    resp.message = "Farm has been " + ((EFarmStatus)req.statusID).ToString(); // Setting response message
                }

                if (notifications != null && notifications.Count() > 0)
                {
                    await _notificationService.SendNotifications(notifications, replacementDTO);
                }

                tblDeviceNotifications? deviceNotification = null;// await _repoWrapper.NotificationRepo.getDeviceNotificationByType(EDeviceNotificationType.Farmer_FarmStatusUpdated);
                if (deviceNotification != null)
                {
                    tblNotification newNotification = new tblNotification()
                    {
                        ClickedOn = DateTime.UtcNow,
                        CreatedOn = DateTime.UtcNow,
                        DeviceNotificationID = deviceNotification.NotificationID,
                        FarmID = farm.FarmID,
                        PlanID = null,
                        OrderID = null,
                        IsClicked = false,
                        UserID = farm.UserID,
                        NotificationRequestStatus = GetNotificationRequestStatus((EFarmStatus)req.statusID)
                    };
                    _repoWrapper.NotificationRepo.addDeviceNotification(newNotification);
                    await _repoWrapper.SaveAsync();
                }

            }
            return resp; // Returning the API response
        }
        private async Task<List<NotificationRequest>> SendNotificationForStatusChange(string employeeName,
                string email, string cellNumber, string deviceToken,
                string farmName, string status, string employeeUserID, string applicationID)
        {
            List<NotificationRequest> notifications = new List<NotificationRequest>();
            // Create notifications for all TSOs for all farms added
            NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.Employee_FarmApplicationPendingForApproval, "EN");
            if (notificationDTO != null)
            {

                var farmerEmail = new NotificationRequest
                {
                    Type = ENotificationType.Email,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = email, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.body
                };
                notifications.Add(farmerEmail);
                var farmerFCM = new NotificationRequest
                {
                    Type = ENotificationType.FCM,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = deviceToken, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.fcmBody
                };
                notifications.Add(farmerFCM);
                var farmerSMS = new NotificationRequest
                {
                    Type = ENotificationType.SMS,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = cellNumber, Name = employeeName } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.smsBody
                };
                notifications.Add(farmerSMS);
                var farmerDevice = new NotificationRequest
                {
                    Type = ENotificationType.Device,
                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = employeeUserID, Name = employeeUserID } },
                    Subject = notificationDTO.title,
                    Message = notificationDTO.deviceBody
                };
                notifications.Add(farmerDevice);
            }
            return notifications;

        }
        private async Task SendNotificationForFormApproval(string farmerName,
                string email, string cellNumber, string deviceToken,
                string farmName, string farmerUserID, string applicationID, string languageCode)
        {
            // Create notifications for all TSOs for all farms added
            NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.farmApproved, languageCode);


            NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();

            replacementDTO.ApplicationId = applicationID;
            replacementDTO.FarmName = farmName;
            replacementDTO.NotificationBodyTypeID = ENotificationBody.farmApproved;

            if (notificationDTO != null)
            {
                List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = email, Name = farmerName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.FCM,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = deviceToken, Name = farmerName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.fcmBody
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = cellNumber, Name = farmerName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Device,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = farmerUserID, Name = farmerName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.deviceBody
                                },
                            };

                await _notificationService.SendNotifications(notifications, replacementDTO);
            }

        }
        private async Task<ResponseType?> CallCreateCustomerWSDL(tblFarmerProfile profile, TblUser user)
        {



            var request = new CreateCustomer.RequestType
            {
                city = profile.City.Name,
                cnic = profile.CNICNumber,
                condGrp1 = "",
                condGrp2 = "",
                condGrp3 = "",
                condGrp4 = "",
                district = profile.District.Name,
                email = user.Email,
                fax = "",
                mobileNum = profile.CellNumber,
                name = user.FirstName + " " + user.LastName,
                ntn = profile.NTNNumber,
                phoneNum = user.PhoneNumber,
                postalCode = "77777",
                salePoint = profile.City != null && profile.City.Tehsils.FirstOrDefault() != null ? profile.City.Tehsils.FirstOrDefault().TehsilCode : "",
                searchTerm1 = "Ugai",
                searchTerm2 = "AMAZON FARMER",
                street = profile.Address1,
                street2 = profile.Address2,
                street4 = "",
                strn = profile.STRNNumber
            };
            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            ResponseType? wsdlResponse = await wSDLFunctions.CreateCustomerWSDLAsync(request);


            return wsdlResponse;

        }


        private EDeviceNotificationRequestStatus GetNotificationRequestStatus(EFarmStatus eFarmStatus)
        {
            switch (eFarmStatus)
            {
                case EFarmStatus.Draft:
                    return EDeviceNotificationRequestStatus.Farm_Draft;
                case EFarmStatus.PendingForTSO:
                    return EDeviceNotificationRequestStatus.Farm_PendingForTSO;
                case EFarmStatus.PendingforRSM:
                    return EDeviceNotificationRequestStatus.Farm_PendingforRSM;
                case EFarmStatus.PendingForPatwari:
                    return EDeviceNotificationRequestStatus.Farm_PendingForPatwari;
                case EFarmStatus.Approved:
                    return EDeviceNotificationRequestStatus.Farm_Approved;
                case EFarmStatus.Rejected:
                    return EDeviceNotificationRequestStatus.Farm_Rejected;
                case EFarmStatus.SendBack:
                    return EDeviceNotificationRequestStatus.Farm_SendBack;
                case EFarmStatus.Deleted:
                    return EDeviceNotificationRequestStatus.Farm_Deleted;
                default:
                    return EDeviceNotificationRequestStatus.Farm_Draft;
            }
        }
    }
}
