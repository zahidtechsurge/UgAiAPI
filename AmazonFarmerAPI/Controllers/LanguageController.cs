using AmazonFarmer.Core.Application; // Importing necessary namespaces
using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers // Defining namespace for the controller
{
    [ApiController] // Indicates that this class is an API controller
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class LanguageController : ControllerBase // Inherits from ControllerBase for API controller functionality
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data

        public LanguageController(IRepositoryWrapper repoWrapper) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper
        }

        // Endpoint to fetch available languages
        [AllowAnonymous] // Allowing anonymous access to this endpoint
        [HttpGet("getLanguages")] // Defines the HTTP GET method and endpoint route
        public async Task<APIResponse> getLanguages() // Method to handle GET requests for getting languages
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            try
            {
                // Fetching available languages using repository
                resp.response = await _repoWrapper.LanguageRepo.GetLanguages();
            }
            catch (Exception ex) // Handling exceptions
            {
                resp.isError = true; // Setting error flag in response
                resp.message = ex.Message; // Setting error message in response
            }
            return resp; // Returning the API response
        }

    }
}
