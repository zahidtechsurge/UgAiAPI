/*
   This controller manages requests related to farms.
   It includes endpoints for setting up farms, retrieving farms, retrieving farm requests, and verifying farms.
   Authentication is required for accessing all methods in this controller.

   [ApiController] attribute indicates that this class is a controller for Web APIs.
   [Authorize] attribute specifies that authentication is required for accessing controller methods, using the Bearer authentication scheme.
   [Route] attribute defines the base route for all endpoints in this controller.

   The FarmController class inherits from ControllerBase, which provides common functionality for MVC controllers.
*/

using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using CreateCustomer;
using FirebaseAdmin.Auth;
using Google.Api;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System.Globalization;
using System.Net.Mail;
using System.Security.Claims;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class FarmController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        private readonly GoogleLocationExtension _googleLocationExtension; // Google location extension for Lat Lng
        private readonly AmazonFarmerContext _dbContext;
        private readonly NotificationService _notificationService;
        // Constructor injection of IRepositoryWrapper.
        public FarmController(IRepositoryWrapper repoWrapper, GoogleLocationExtension googleLocationExtension,
            AmazonFarmerContext dbContext, NotificationService notificationService)
        {
            _repoWrapper = repoWrapper;
            _googleLocationExtension = googleLocationExtension;
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        // Endpoint for setting up farms.
        [HttpPost("setupFarm")]
        public async Task<APIResponse> setupFarm(FarmDTO req)
        {
            APIResponse resp = new APIResponse();
                await validateSetupFarm(req);
                // Initialize farm setup response object
                farmSetup_Resp application = new farmSetup_Resp();

                // Get the user ID from the token
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!User.IsInRole("Farmer"))
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);

                TblUser user = await _repoWrapper.UserRepo.getUserByUserID(userID);
                if (user.isOTPApproved == null || !user.isOTPApproved.Value)
                {
                    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                }

                // get applicationID from any previous Farm
                req.applicationID = await _repoWrapper.FarmRepo.getApplicationIDByFarmerID(userID);

                if (req.applicationID == null || req.applicationID == 0)
                {
                    // Add farm application
                    application = await _repoWrapper.FarmRepo.addFarmApplication();
                    // Set application ID and request type for each farm
                    req.applicationID = Convert.ToInt32(application.applicationID);
                }
                //req.requestType = EFarmStatus.Draft;//req.requestType;

                DistictCityTehsilDTO? distictCityTehsilDTO = await _repoWrapper.DistrictRepo
                    .GetDistricCityTehsilAsync(req.districtID, req.cityID, req.tehsilID);

                if (distictCityTehsilDTO == null)
                    throw new AmazonFarmerException("District City Tehsil not found");
                string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims
                
                #region get Farm Location by Google geocode
                if (
                    (req.latitude == null && req.longitude == null) || 
                    (req.latitude == 0 && req.longitude == 0)
                )
                {
                    FarmAddressDTO dto = MapFarmToFarmAddress(req, distictCityTehsilDTO);
                    getFarmLocation farmLocation = await _googleLocationExtension.GetlatLngForAddress(dto);
                    req.latitude = farmLocation.latitude;
                    req.longitude = farmLocation.longitude;
                }
                #endregion
                if (req.farmID == 0)
                {
                    // Add farm request
                    req.farmID = await _repoWrapper.FarmRepo.addFarmRequest(req, userID);

                    // Add farms
                    req.farmID = await _repoWrapper.FarmRepo.addFarm(req, userID);
                }
                else
                {
                    // update farm request
                    await _repoWrapper.FarmRepo.updateFarmRequest(req, userID);

                    // update farms
                    await _repoWrapper.FarmRepo.updateFarm(req, userID);
                }

                await _repoWrapper.SaveAsync();

                tblfarm Farm = await _repoWrapper.FarmRepo.getFarmByFarmID(req.farmID, userID, languageCode);
                if (Farm == null)
                {
                    throw new AmazonFarmerException(_exceptions.farmNotFound);
                }
                _repoWrapper.FarmRepo.AddFarmUpdateLogs(Farm, userID);
                // Upload farm setup documents
                await uploadFarmSetupDocument(req.attachments, userID, Farm);
                await _repoWrapper.SaveAsync();
                // Pad application ID to 10 characters
                application.applicationID = req.applicationID.ToString().PadLeft(10, '0');
                // Set response
                resp.response = application;

            return resp;
        }
        private async Task validateSetupFarm(FarmDTO req)
        {
            if (string.IsNullOrEmpty(req.farmName))
                throw new AmazonFarmerException(_exceptions.farmNameRequired);
            else if (string.IsNullOrEmpty(req.address1))
                throw new AmazonFarmerException(_exceptions.addressRequired);
            else if (string.IsNullOrEmpty(req.farmerComment))
                throw new AmazonFarmerException(_exceptions.farmerCommentRequired);
        }

        private FarmAddressDTO MapFarmToFarmAddress(FarmDTO req, DistictCityTehsilDTO distictCityTehsilDTO)
        {
            return new FarmAddressDTO
            {
                Address1 = req.address1,
                Address2 = req.address2,
                City = distictCityTehsilDTO.CityName,
                District = distictCityTehsilDTO.DistrictName,
                Tehsil = distictCityTehsilDTO.TehsilName,
                Name = req.farmName
            };
        }
        // Method to upload farm setup documents.
        //private async Task uploadFarmSetupDocument(List<farmAttachment> attachments, string UserID, int FarmID)
        //{
        //    foreach (var attachment in attachments)
        //    {
        //        if (!string.IsNullOrEmpty(attachment.content))
        //        {
        //            // Decode the base64 string into a byte array
        //            byte[] imageBytes = Convert.FromBase64String(attachment.content);
        //            // Generate a unique file name
        //            string fileName = DateTime.UtcNow.ToString("yyyy.MM.dd") + "-" + Guid.NewGuid().ToString() + ".png";
        //            // Specify the directory where you want to save the image
        //            string filePath = Path.Combine(ConfigExntension.GetConfigurationValue("Locations:AttachmentURL"), fileName);
        //            // Write the image bytes to the file
        //            System.IO.File.WriteAllBytes(filePath, imageBytes);
        //            // Return the URL of the saved image
        //            string imageUrl = $"{ConfigExntension.GetConfigurationValue("Locations:BaseURL")}/Attachments/{fileName}";
        //            AttachmentsDTO attachmentsDTO = new AttachmentsDTO()
        //            {
        //                attachmentType = EAttachmentType.Farm_Document,
        //                filePath = imageUrl,
        //                fileType = "PNG"
        //            };
        //            tblAttachment attachmentResp = await _repoWrapper.AttachmentRepo.uploadAttachment(attachmentsDTO);
        //            attachAttachment attachWithUser = new attachAttachment
        //            {
        //                attachmentID = attachmentResp.ID,
        //                userID = UserID,
        //                farmID = FarmID
        //            };
        //            await _repoWrapper.AttachmentRepo.attachFarmAttachment(attachWithUser);
        //        }
        //    }
        //}
        //Method to upload farm setup documents.
        private async Task uploadFarmSetupDocument(List<uploadAttachmentResp> AttachmentIDs, string UserID, tblfarm Farm)
        {
            List<tblFarmAttachments> attachments = Farm.FarmAttachments.ToList();

            foreach (tblFarmAttachments farmAttachment in attachments)
            {
                farmAttachment.Status = EActivityStatus.DeActive;
                await _repoWrapper.AttachmentRepo.updateFarmAttachment(farmAttachment);
            }

            foreach (var attachment in AttachmentIDs)
            {
                tblFarmAttachments farmAttachment = attachments.Where(a => a.AttachmentID == attachment.id).FirstOrDefault();
                if (farmAttachment != null)
                {
                    farmAttachment.Status = EActivityStatus.Active;
                    await _repoWrapper.AttachmentRepo.updateFarmAttachment(farmAttachment);
                }
                else
                {
                    attachAttachment attachWithUser = new attachAttachment
                    {
                        attachmentID = attachment.id,
                        userID = UserID,
                        farmID = Farm.FarmID
                    };
                    await _repoWrapper.AttachmentRepo.attachFarmAttachment(attachWithUser);
                }
            }

        }

        // Endpoint for retrieving farm requests.
        [HttpGet("getFarmRequests")]
        public async Task<APIResponse> getFarmRequests()
        {
            APIResponse resp = new APIResponse();
                // Get the user ID from the token
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // Retrieve farm requests by user ID
                List<tblfarm> Farms = await _repoWrapper.FarmRepo.getFarmsByUserID(userID);

                List<farms_Resp> farmsResponse = new List<farms_Resp>();
                foreach (var farm in Farms)
                {
                    farms_Resp farmResponse = new farms_Resp()
                    {
                        address = farm.Address1 ?? string.Empty,
                        farmID = farm.FarmID,
                        farmName = farm.FarmName,
                        isApproved = farm.isApproved,
                        isPrimary = farm.isPrimary,
                        acreage = farm.Acreage
                    };

                    farmsResponse.Add(farmResponse);
                }
                resp.response = farmsResponse;
            return resp;
        }

        // Endpoint for verifying farms.
        [HttpPost("submitForApproval")]
        public async Task<APIResponse> submitForApproval(farm_SumitForApproval req)
        {
            APIResponse resp = new APIResponse();
            // Check if farm ID is valid
            if (req.farmID <= 0)
                throw new AmazonFarmerException(_exceptions.farmIDRequired);
            else if (!req.isAllDataCorrect)
                throw new AmazonFarmerException(_exceptions.isAllInformationCorrect);
            else if (!req.canUseData)
                throw new AmazonFarmerException(_exceptions.canUseDataRequired);
            else
            {
                // Initialize farm setup response object
                farmSetup_Resp application = new farmSetup_Resp();
                // Get the user ID from the token
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userID))
                    throw new AmazonFarmerException(_exceptions.userIDNotFound);
                List<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmsByfarmerID(userID);
                if (farms != null && farms.Count() > 0)
                {
                    TblUser farmer = await _repoWrapper.UserRepo.getUserByUserID(userID);

                    if (farmer.isOTPApproved == null || !farmer.isOTPApproved.Value)
                    {
                        throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    }

                    NotificationDTO notificationDTO = new NotificationDTO();
                    NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                    //if (farms.Any(x => x.Status == EFarmStatus.Approved))
                    //{
                    //    throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    //}
                    application.applicationID = farms.OrderBy(x => x.FarmID).First().ApplicationID.ToString().PadLeft(10, '0');

                    foreach (tblfarm farm in farms)
                    {
                        if (farm.FarmID == req.farmID)
                        {
                            farm.isPrimary = true;
                        }
                        else
                        {
                            farm.isPrimary = false;
                        }
                        if (farm.UserID == userID && (farm.Status == EFarmStatus.Draft || farm.Status == EFarmStatus.SendBack))
                        {
                            replacementDTO.FarmID = farm.FarmID.ToString().PadLeft(10,'0');
                            replacementDTO.NotificationBodyTypeID = ENotificationBody.Employee_farmSubmitForApproval;
                            notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.Employee_farmSubmitForApproval, "EN");
                            farm.Status = EFarmStatus.PendingForTSO;

                        }
                        await _repoWrapper.FarmRepo.changeFarmRegistrationStatus(farm, userID);
                    }

                    farmer.FarmerProfile.First().isApproved = EFarmerProfileStatus.Pending;
                    await _repoWrapper.UserRepo.updateUser(farmer);
                    await _repoWrapper.SaveAsync();

                    List<int> districtIds = farms.Select(f => f.DistrictID).Distinct().ToList();

                    // Create notifications for all TSOs for all farms added

                    List<TblUser> employees = await _repoWrapper.UserRepo.getTSOsByDistrictIDs(districtIds);

                    if (employees != null && employees.Count > 0)
                    {
                        List<NotificationRequestRecipient> emails = employees.Select(e => new NotificationRequestRecipient { Email = e.Email, Name = e.FirstName }).Distinct().ToList();
                        List<NotificationRequestRecipient> deviceToken = employees.Where(e => e.DeviceToken != null).Select(e => new NotificationRequestRecipient { Email = e.DeviceToken, Name = e.FirstName }).Distinct().ToList();
                        List<NotificationRequestRecipient> empUserIDs = employees.Select(e => new NotificationRequestRecipient { Email = e.Id, Name = e.FirstName }).Distinct().ToList();
                        List<NotificationRequestRecipient> phoneNumbers = employees.Where(e => e.PhoneNumber != null).Select(e => new NotificationRequestRecipient { Email = e.PhoneNumber, Name = e.FirstName }).Distinct().ToList();

                        //Farmer name
                        string farmerName = farms.FirstOrDefault().Users.FirstName + " " + farms.FirstOrDefault().Users.LastName;
                        //Added Farms
                        //var farmsNames = farms.Select(f => ("<br/> " + f.FarmName)).ToList();

                        if (notificationDTO != null)
                        {
                            List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                                    Recipients = emails,
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Device,
                                    Recipients = empUserIDs,
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.deviceBody
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients = phoneNumbers,
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.FCM,
                                    Recipients = deviceToken,
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.fcmBody
                                }

                                };
                            replacementDTO.ApplicationId = farms.FirstOrDefault().ApplicationID.ToString().PadLeft(10, '0');
                            replacementDTO.FarmName = string.Join("<br/> ", farms.Where(x => x.Status == EFarmStatus.PendingForTSO).Select(f => f.FarmName));
                            replacementDTO.farmerName = farmerName;

                            await _notificationService.SendNotifications(notifications, replacementDTO);
                        }
                    }
                    // Set response message
                    resp.message = "Farm has been submitted for approval";
                    resp.response = application;
                }
            }

            return resp;
        }

        // Endpoint for acknowledging farm approval.
        //[HttpPost("farmApprovalAcknowledgement")]
        //public async Task<JSONResponse> farmApprovalAcknowledgement(farmApprovalAcknowledgement_Req req)
        //{
        //    JSONResponse resp = new JSONResponse();
        //    try
        //    {
        //        // Check if application ID is provided
        //        if (string.IsNullOrEmpty(req.applicationID))
        //           throw new AmazonFarmerException(_exceptions.farmIDRequired);
        //        else
        //        {
        //            // Get the user ID from the token
        //            string userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //            // Trim leading zeros and convert to integer
        //            int applicationID = Convert.ToInt32(req.applicationID.TrimStart('0'));
        //            // Acknowledge farm approval
        //            await _repoWrapper.FarmRepo.farmApprovalAcknowledgement(applicationID, userID);
        //            // Set response message
        //            resp.message = "Farm approval is now acknowledged!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        resp.isError = true;
        //        resp.message = ex.Message;
        //    }
        //    return resp;
        //}


        //// Endpoint for retrieving farms.
        [HttpGet("getFarms")]
        public async Task<APIResponse> getFarms()
        {
            APIResponse resp = new APIResponse();
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<tblfarm> Farms = await _repoWrapper.FarmRepo.getFarmsByUserID(userID);

                List<farms_Resp> farmsResponse = new List<farms_Resp>();
                foreach (var farm in Farms)
                {
                    farms_Resp farmResponse = new farms_Resp()
                    {
                        address = farm.Address1 ?? string.Empty,
                        farmID = farm.FarmID,
                        farmName = farm.FarmName,
                        isApproved = farm.isApproved,
                        isPrimary = farm.isPrimary,
                        acreage = farm.Acreage
                    };
                    farmsResponse.Add(farmResponse);
                }
                resp.response = farmsResponse;
            return resp;
        }


        [HttpDelete("removeFarm")]
        public async Task<JSONResponse> removeFarm(farmDetail_Req req)
        {
            JSONResponse resp = new JSONResponse();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving user ID from user claims
            string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims
            List<EFarmStatus> farmStatus = new List<EFarmStatus>() { EFarmStatus.Approved, EFarmStatus.PendingForPatwari, EFarmStatus.PendingforRSM, EFarmStatus.PendingForTSO };
            if (string.IsNullOrEmpty(userID)) // Checking if user ID is provided
                throw new AmazonFarmerException(_exceptions.userIDNotFound); // Throws exception if user ID is not found
            else if (req.farmID == 0)
                throw new AmazonFarmerException(_exceptions.farmIDRequired);
            tblfarm farm = await _repoWrapper.FarmRepo.getFarmByFarmID(req.farmID, userID, languageCode);
            if (farm == null)
                throw new AmazonFarmerException(_exceptions.farmNotFound);
            else if (farmStatus.Contains(farm.Status))
                throw new AmazonFarmerException(_exceptions.farmInValidState);
            else if (farm.UserID != userID)
                throw new AmazonFarmerException(_exceptions.userNotAuthorized);

            farm.Status = EFarmStatus.Deleted;
            farm.isApproved = false;
            await _repoWrapper.FarmRepo.updateFarm(farm);
            _repoWrapper.FarmRepo.AddFarmUpdateLogs(farm, userID);
            await _repoWrapper.SaveAsync();

            resp.message = "farm has been deleted";
            return resp;
        }

        //Endpoint for farmAcknowledgement
        [HttpPost("farmApprovalAcknowledgement")]
        public async Task<JSONResponse> farmApprovalAcknowledgement()
        {
            JSONResponse resp = new JSONResponse();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userID))
            {
                await _repoWrapper.FarmRepo.farmApprovalAcknowledgement(userID);
                await _repoWrapper.SaveAsync();
                resp.message = "updated!";
            }
            else
                throw new AmazonFarmerException(_exceptions.userNotFound);


            return resp;
        }

        [HttpGet("getDraftedFarms")]
        public async Task<APIResponse> getDraftedFarms()
        {
            APIResponse resp = new();

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string languageCode = User.FindFirst("languageCode")?.Value; // Retrieving language code from user claims

            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);

            resp.response = await _repoWrapper.FarmRepo.getDraftedFarmsByFarmerID(userID, languageCode);

            return resp;
        }

        [HttpPost("getFarmDetails")]
        public async Task<APIResponse> getFarmDetails(farmDetail_Req req)
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
                    string designationID = User.FindFirst("designationID")?.Value; // Retrieving designation ID from user claims
                    tblfarm farm = await _repoWrapper.FarmRepo.getFarmByFarmID(req.farmID, userID, languageCode);
                    if (farm == null)
                    {
                        throw new AmazonFarmerException(_exceptions.farmNotFound);
                    }
                    if (User.IsInRole("Farmer") && farm.UserID != userID)
                        throw new AmazonFarmerException(_exceptions.farmNotFound);
                    else if (User.IsInRole("Employee") && 1 == 2) // add condition to check employee is authorized to get the farm details or not
                        throw new AmazonFarmerException(_exceptions.farmNotFound);
                    else
                    {
                        farm.FarmAttachments = await _repoWrapper.FarmRepo.getFarmAttachmentsByFarmID(req.farmID);
                        resp.response = new farmDetail_Resp()
                        {
                            farmID = farm.FarmID,
                            farmName = farm.FarmName,
                            acreage = farm.Acreage,
                            address1 = farm.Address1,
                            address2 = farm.Address2,
                            city = farm.City.CityLanguages.First().Translation,
                            district = farm.District.DistrictLanguages.First().Translation,
                            tehsil = farm.Tehsil.TehsilLanguagess.First().Translation,
                            districtID = farm.DistrictID,
                            cityID = farm.CityID,
                            tehsilID = farm.TehsilID,
                            isApproved = farm.isApproved,
                            isLeased = farm.isLeased,
                            isPrimary = farm.isPrimary,
                            revertedReason = farm.RevertedReason,
                            sapFarmID = farm.SAPFarmCode.PadLeft(10, '0'),
                            status = (int)farm.Status,
                            statusDescription = ConfigExntension.GetEnumDescription(farm.Status),
                            farmerComment = farm.FarmerComment,
                            attachmentGUID = farm.FarmAttachments.Where(x => x.Status == EActivityStatus.Active).Select(x => new uploadAttachmentResp
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
    }
}
