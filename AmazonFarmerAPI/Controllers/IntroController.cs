using AmazonFarmer.Core.Application; // Importing necessary namespaces
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers // Defining namespace for the controller
{
    [ApiController] // Indicates that this class is an API controller.
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class IntroController : ControllerBase // Inherits from ControllerBase for API controller functionality
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data

        public IntroController(IRepositoryWrapper repoWrapper) // Constructor for initializing repository wrapper
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper
        }

        // Endpoint to fetch intros for different languages
        [AllowAnonymous] // Allowing anonymous access to this endpoint
        [HttpPost("getIntros")] // Defines the HTTP POST method and endpoint route
        public async Task<APIResponse> getIntros(LanguageReq req) // Method to handle POST requests for getting intros
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
                                                  // Checking if the language code is provided
            if (req == null || string.IsNullOrEmpty(req.languageCode))
                throw new AmazonFarmerException(_exceptions.languageCodeRequired); // Throwing exception if language code is missing
            //throw new AmazonFarmerException(_exceptions.getExceptionByLanguageCode(req.languageCode, "languageCodeRequired"));


            // Fetching intros for the provided language code using repository
            getIntroDTO inReq = new getIntroDTO()
            {
                languageCode = req.languageCode,
                basePath = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL")
            };
            resp.response = await _repoWrapper.IntroRepo.getIntros(inReq);
            return resp; // Returning the API response
        }
    }
}
