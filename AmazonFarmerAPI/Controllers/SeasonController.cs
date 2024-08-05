using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class SeasonController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public SeasonController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        //[Authorize]
        [HttpGet("getSeasons")]
        public async Task<APIResponse> getSeasons()
        {
            APIResponse resp = new APIResponse();
            try
            {
                LanguageReq req = new LanguageReq();
                req.languageCode = User.FindFirst("languageCode")?.Value;
                resp.response = await _repoWrapper.SeasonRepo.getSeasonsByLanguageCode(req);
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
