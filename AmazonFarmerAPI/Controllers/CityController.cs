/*
   This controller manages requests related to cities.
   It includes an endpoint for retrieving cities and requires authentication for accessing all methods except getCities.

   [ApiController] attribute indicates that this class is a controller for Web APIs.
   [Authorize] attribute specifies that authentication is required for accessing controller methods, using the Bearer authentication scheme.
   [Route] attribute defines the base route for all endpoints in this controller.

   The CityController class inherits from ControllerBase, which provides common functionality for MVC controllers.
*/

using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor injection of IRepositoryWrapper.
        public CityController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Endpoint for retrieving cities. Allows anonymous access.
        [AllowAnonymous]
        [HttpPost("getCities")]
        public async Task<APIResponse> getCities(getCity_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Check if language code is provided, throw exception if it's missing.
                if (string.IsNullOrEmpty(req.languageCode))
                    throw new Exception(_exceptions.languageCodeRequired);

                // Fetch cities asynchronously using repository.
                resp.response = await _repoWrapper.CityRepo.getCities(req);
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