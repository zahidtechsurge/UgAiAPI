/*
   This controller manages requests related to districts.
   It includes an endpoint for retrieving districts and allows anonymous access.

   [ApiController] attribute indicates that this class is a controller for Web APIs.
   [Route] attribute defines the base route for all endpoints in this controller.

   The DistrictController class inherits from ControllerBase, which provides common functionality for MVC controllers.
*/

using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor injection of IRepositoryWrapper.
        public DistrictController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Endpoint for retrieving districts. Allows anonymous access.
        [AllowAnonymous]
        [HttpPost("getDistricts")]
        public async Task<APIResponse> getDistricts(getDistrict_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Check if language code is provided, throw exception if it's missing.
                if (string.IsNullOrEmpty(req.languageCode))
                    throw new Exception(_exceptions.languageCodeRequired);

                // Fetch districts asynchronously using repository.
                resp.response = await _repoWrapper.DistrictRepo.getDistricts(req);
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
