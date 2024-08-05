using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System; // Added to use Exception class
using System.Threading.Tasks; // Added to use Task

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TehsilController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor to inject IRepositoryWrapper dependency
        public TehsilController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Action method to get tehsils
        [AllowAnonymous]
        [HttpPost("getTehsils")]
        public async Task<APIResponse> getTehsils(getTehsil_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Check if language code is null or empty
                if (string.IsNullOrEmpty(req.languageCode))
                   throw new AmazonFarmerException(_exceptions.languageCodeRequired);

                // Call repository method to get tehsils
                resp.response = await _repoWrapper.TehsilRepo.getTehsils(req);
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }
    }
}
