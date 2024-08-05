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
    public class ServiceController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public ServiceController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        //[Authorize]
        [HttpGet("getServices")]
        public async Task<APIResponse> getServices()
        {
            APIResponse resp = new APIResponse();
            try
            {

                LanguageReq req = new LanguageReq() { languageCode = User.FindFirst("languageCode")?.Value };

                resp.response = await _repoWrapper.ServiceRepo.getServicesByLanguageID(req, Convert.ToInt32(ConfigExntension.GetConfigurationValue("productSettings:ServicePostDeliveryIn")));
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
