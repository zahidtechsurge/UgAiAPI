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
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Parameters;
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
        // Constructor injection of IRepositoryWrapper.
        public FarmController(IRepositoryWrapper repoWrapper, GoogleLocationExtension googleLocationExtension)
        {
            _repoWrapper = repoWrapper;
            _googleLocationExtension = googleLocationExtension;
        }


        // Endpoint for setting up farms.
        [HttpPost("setupFarms")]
        public async Task<APIResponse> setupFarms(farmsList_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Initialize farm setup response object
                farmSetup_Resp application = new farmSetup_Resp();

                // Check if user can use data
                if (!req.canUseData)
                    throw new Exception(_exceptions.canUseDataRequired);
                // Check if all information provided is correct
                else if (!req.isAllInformationCorrect)
                    throw new Exception(_exceptions.isAllInformationCorrect);
                // Check if at least one farm is provided
                else if (req.farms == null || req.farms.Count <= 0)
                    throw new Exception(_exceptions.atleastOneFarm);
                // Check if there is at least one primary farm
                else if (req.farms == null || req.farms.FindAll(x => x.isPrimary).Count <= 0)
                    throw new Exception(_exceptions.OnePrimaryFarmRequired);
                else
                {
                    // Get the user ID from the token
                    var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    int farmID = 0;

                    // Add farm application
                    application = await _repoWrapper.FarmRepo.addFarmApplication();
                    if (application != null)
                    {
                        foreach (var item in req.farms)
                        {
                            // Set application ID and request type for each farm
                            item.applicationID = Convert.ToInt32(application.applicationID);
                            item.requestType = req.requestType;

                            DistictCityTehsilDTO? distictCityTehsilDTO = await _repoWrapper.DistrictRepo
                                .GetDistricCityTehsilAsync(item.districtID, item.cityID, item.tehsilID);

                            if (distictCityTehsilDTO == null)
                                throw new Exception("District City Tehsil not found");

                            #region get Farm Location by Google geocode
                            FarmAddressDTO dto = MapFarmToFarmAddress(item, distictCityTehsilDTO);
                            getFarmLocation farmLocation = await _googleLocationExtension.GetlatLngForAddress(dto);
                            item.latitude = farmLocation.latitude;
                            item.longitude = farmLocation.longitude;
                            #endregion

                            // Add farm request
                            farmID = await _repoWrapper.FarmRepo.addFarmRequest(item, userID);

                            // Upload farm setup documents
                            await uploadFarmSetupDocument(item.attachments, userID, farmID);

                            // Add farms
                            farmID = await _repoWrapper.FarmRepo.addFarms(item, userID);
                        }
                        // Pad application ID to 10 characters
                        application.applicationID = application.applicationID.PadLeft(10, '0');
                        // Set response
                        resp.response = application;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
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
                Name =req.farmName
            };
        }
        // Method to upload farm setup documents.
        private async Task uploadFarmSetupDocument(List<farmAttachment> attachments, string UserID, int FarmID)
        {
            foreach (var attachment in attachments)
            {
                if (!string.IsNullOrEmpty(attachment.content))
                {
                    // Decode the base64 string into a byte array
                    byte[] imageBytes = Convert.FromBase64String(attachment.content);
                    // Generate a unique file name
                    string fileName = DateTime.UtcNow.ToString("yyyy.MM.dd") + "-" + Guid.NewGuid().ToString() + ".png";
                    // Specify the directory where you want to save the image
                    string filePath = Path.Combine(ConfigExntension.GetConfigurationValue("Locations:AttachmentURL"), fileName);
                    // Write the image bytes to the file
                    System.IO.File.WriteAllBytes(filePath, imageBytes);
                    // Return the URL of the saved image
                    string imageUrl = $"{ConfigExntension.GetConfigurationValue("Locations:BaseURL")}/Attachments/{fileName}";
                    AttachmentsDTO attachmentsDTO = new AttachmentsDTO()
                    {
                        attachmentType = EAttachmentType.Farm_Document,
                        filePath = imageUrl,
                        fileType = "PNG"
                    };
                    tblAttachment attachmentResp = await _repoWrapper.AttachmentRepo.uploadAttachment(attachmentsDTO);
                    attachAttachment attachWithUser = new attachAttachment
                    {
                        attachmentID = attachmentResp.ID,
                        userID = UserID,
                        farmID = FarmID
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
            try
            {
                // Get the user ID from the token
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // Retrieve farm requests by user ID
                resp.response = await _repoWrapper.FarmRepo.getFarmsByUserID(userID);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }


        // Endpoint for verifying farms.
        [HttpPost("verifyFarm")]
        public async Task<JSONResponse> verifyFarm(farmVerification_Req req)
        {
            JSONResponse resp = new JSONResponse();
            try
            {
                // Check if farm ID is valid
                if (req.farmID <= 0)
                    throw new Exception(_exceptions.farmIDRequired);
                // Check if request ID is valid
                else if (req.requestID <= 0)
                    throw new Exception(_exceptions.statusIDRequired);
                else
                {
                    // Set response message
                    resp.message = "Farm [status]";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                resp.isError = true;
                resp.message = ex.Message;
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
        //            throw new Exception(_exceptions.farmIDRequired);
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

        [HttpGet("getFarmApplications")]
        public async Task<APIResponse> getFarmApplications()
        {
            APIResponse resp = new APIResponse();
            try
            {
                resp.response = await _repoWrapper.FarmRepo.getFarmApplications();
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }


        //// Endpoint for retrieving farms.
        [HttpGet("getFarms")]
        public async Task<APIResponse> getFarms()
        {
            APIResponse resp = new APIResponse();
            try
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                resp.response = await _repoWrapper.FarmRepo.getFarmsByUserID(userID);
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        [HttpPost("getFarmRegistrationRequest")]
        public async Task<APIResponse> getFarmRegistrationRequest(farmSetup_Resp req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                var languageCode = User.FindFirst("languageCode")?.Value;
                if (string.IsNullOrEmpty(req.applicationID))
                    throw new AmazonFarmerException(_exceptions.getExceptionByLanguageCode(languageCode, "applicationIDRequired"));
                    //throw new AmazonFarmerException(_exceptions.applicationIDRequired);
                resp.response = await _repoWrapper.FarmRepo.getFarmsByApplicationID(Convert.ToInt32(req.applicationID));
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }
        [HttpPost("farmRegistrationRequest_ChangeStatus")]
        public async Task<APIResponse> farmRegistrationRequest_ChangeStatus(farmRegistrationRequest_ChangeStatus_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                var languageCode = User.FindFirst("languageCode")?.Value;
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(req.farmID))
                    throw new AmazonFarmerException(_exceptions.getExceptionByLanguageCode(languageCode, "farmIDRequired"));
                //throw new AmazonFarmerException(_exceptions.farmIDRequired);
                else if (req.statusID == 0)
                    throw new AmazonFarmerException(_exceptions.getExceptionByLanguageCode(languageCode, "statusIDRequired"));
                //throw new AmazonFarmerException(_exceptions.statusIDRequired);
                else if (string.IsNullOrEmpty(userID))
                    throw new AmazonFarmerException(_exceptions.getExceptionByLanguageCode(languageCode, "userIDNotFound"));
                //throw new AmazonFarmerException(_exceptions.userIDNotFound);
                else
                {
                    await _repoWrapper.FarmRepo.changeFarmRegistrationStatus(Convert.ToInt32(req.farmID.TrimStart('0')), req.statusID, userID);
                    resp.response = "Farm has been " + ((EFarmStatus)req.statusID).ToString();
                }
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }
    }
}
