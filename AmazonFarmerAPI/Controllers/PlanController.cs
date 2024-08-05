using AmazonFarmer.Core.Application; // Importing necessary namespaces
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazonFarmerAPI.Controllers // Defining namespace for the controller
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class PlanController : ControllerBase // Inherits from ControllerBase for API controller functionality
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data

        public PlanController(IRepositoryWrapper repoWrapper) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper
        }

        // Endpoint for retrieving farms.
        [HttpGet("getFarms")]
        public async Task<APIResponse> getFarms()
        {
            APIResponse resp = new APIResponse();
            try
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userID))
                {
                    resp.response = await _repoWrapper.FarmRepo.getFarmsByUserID(userID);
                }
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = $"{ex.Message}";
            }
            return resp;
        }

        // Endpoint to save a plan
        [HttpPost("savePlan")] // Defines the HTTP POST method and endpoint route
        public async Task<APIResponse> savePlan(PlanDTO req) // Method to handle POST requests for saving a plan
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            try
            {
                // Checking if crops are provided
                if (req.crops == null || req.crops.Count() <= 0)
                {
                    throw new Exception(_exceptions.cropsRequired); // Throws exception if crops are not provided
                }
                // Checking if each crop has products
                else if (req.crops.Where(x => x.products.Count() <= 0 || x.products == null).Count() > 0)
                {
                    throw new Exception(_exceptions.productsRequired); // Throws exception if any crop does not have products
                }
                // Checking if season ID is provided
                else if (req.seasonID == 0)
                {
                    throw new Exception(_exceptions.seasonRequired); // Throws exception if season ID is not provided
                }
                // Checking if farm ID is provided
                else if (req.farmID == 0)
                {
                    throw new Exception(_exceptions.atleastOneFarm); // Throws exception if farm ID is not provided
                }
                else
                {
                    // Add logic here to save the plan to the database or perform other operations
                    var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userID))
                    {
                        getPlanOrder_Req _resp = new getPlanOrder_Req();
                        //resp.message = "Plan has been " + ((ERequestType)req.requestType).ToString();
                        _resp.planID = await _repoWrapper.PlanRepo.addPlan(req, userID);
                        resp.response = _resp;
                    }
                    else
                    {
                        throw new Exception(_exceptions.userIDNotFound);
                    }
                }
            }
            catch (Exception ex) // Handling exceptions
            {
                resp.isError = true; // Setting error flag in response
                resp.message = ex.Message; // Setting error message in response
            }
            return resp; // Returning the API response
        }

        [HttpPut("editPlan")]
        public async Task<APIResponse> editPlan(EditPlanDTO req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Checking if planID are provided
                if (req.planID == 0)
                    throw new Exception(_exceptions.planIDRequired);
                // Checking if crops are provided
                else if (req.crops == null || req.crops.Count() <= 0)
                {
                    throw new Exception(_exceptions.cropsRequired); // Throws exception if crops are not provided
                }
                // Checking if each crop has products
                else if (req.crops.Where(x => x.products.Count() <= 0 || x.products == null).Count() >= 0)
                {
                    throw new Exception(_exceptions.productsRequired); // Throws exception if any crop does not have products
                }
                // Checking if season ID is provided
                else if (req.seasonID == 0)
                {
                    throw new Exception(_exceptions.seasonRequired); // Throws exception if season ID is not provided
                }
                // Checking if farm ID is provided
                else if (req.farmID == 0)
                {
                    throw new Exception(_exceptions.atleastOneFarm); // Throws exception if farm ID is not provided
                }
                else
                {
                    var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(userID))
                        resp.response = await _repoWrapper.PlanRepo.editPlan(req, userID);
                    else
                        throw new Exception(_exceptions.userIDNotFound);

                }
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = $"{ex.Message}";
            }
            return resp;
        }

        [HttpGet("getPlans")]
        public async Task<APIResponse> getPlans()
        {
            APIResponse resp = new APIResponse();
            try
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var languageCode = User.FindFirst("languageCode")?.Value;
                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(languageCode))
                {
                    resp.response = await _repoWrapper.PlanRepo.getPlansByUserIDandLanguageCode(userID, languageCode);
                }
                else
                {
                    throw new Exception(_exceptions.userIDorLanguageCodeNotFound);
                }
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        [HttpPost("getPlanOrders")]
        [AllowAnonymous]
        public async Task<APIResponse> getPlanOrders(getPlanOrder_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                if (string.IsNullOrEmpty(req.planID))
                    throw new Exception(_exceptions.planIDRequired);
                else
                {
                    var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var languageCode = User.FindFirst("languageCode")?.Value;
                    if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(languageCode))
                        resp.response = await _repoWrapper.PlanRepo.getPlanOrderByUserIDandLanguageCode(userID, languageCode, Convert.ToInt32(req.planID));
                    else
                        throw new Exception(_exceptions.userIDorLanguageCodeNotFound);
                }
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        [HttpPost("updatePlanStatus")]
        public async Task<APIResponse> updatePlanStatus(updatePlanStatus_Req req)
        {
            APIResponse resp = new APIResponse();
            if (string.IsNullOrEmpty(req.planID))
                throw new Exception(_exceptions.planIDRequired);
            else if (req.statusID == 0)
                throw new Exception(_exceptions.statusIDRequired);
            else
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userID))
                    throw new Exception(_exceptions.userIDNotFound);
                else
                {
                    updatePlanStatus_Internal_Req inReq = new updatePlanStatus_Internal_Req()
                    {
                        planID = Convert.ToInt32(req.planID.TrimStart('0')),
                        statusID = req.statusID,
                        userID = userID
                    };
                    await _repoWrapper.PlanRepo.updatePlanStatus(inReq);
                    resp.response = "udpated";
                }
            }
            return resp;

        }




    }
}
