/*
   This controller handles requests related to crops.
   It includes an endpoint for retrieving crops based on season and district ID.
   Authentication is required for accessing all methods in this controller.

   [ApiController] attribute indicates that this class is a controller for Web APIs.
   [Authorize] attribute specifies that authentication is required for accessing controller methods, using the Bearer authentication scheme.
   [Route] attribute defines the base route for all endpoints in this controller.

   The CropsController class inherits from ControllerBase, which provides common functionality for MVC controllers.
*/

using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmerAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class CropsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor injection of IRepositoryWrapper.
        public CropsController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Endpoint for retrieving crops based on season and district ID.
        [HttpPost("getCrops")]
        public async Task<APIResponse> getCrops(CropDTO_req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Fetch crops asynchronously using repository.
                GetCropDTO_Internal_req inReq = new GetCropDTO_Internal_req()
                {
                    farmID = req.farmID,
                    seasonID = req.seasonID,
                    languageCode = User.FindFirst("languageCode")?.Value,
                    basePath = ConfigExntension.GetConfigurationValue("Locations:AdminBaseURL")
                };
                resp.response = await _repoWrapper.CropRepo.getCropsBySeasonAndDistrictID(inReq);
            }
            catch (Exception ex)
            {
                // If an exception occurs, set response properties accordingly.
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }
    }
}
