using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System; // Added to use Exception class
using System.Security.Claims; // Added to use ClaimsPrincipal
using System.Threading.Tasks; // Added to use Task

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class SeasonController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor to inject IRepositoryWrapper dependency
        public SeasonController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Action method to get seasons
        [HttpGet("getSeasons")]
        public async Task<APIResponse> getSeasons()
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Create LanguageReq object and set language code from user claims
                LanguageReq req = new LanguageReq();
                req.languageCode = User.FindFirst("languageCode")?.Value;

                // Call repository method to get seasons by language code
                List<SeasonDTO> seasons = await _repoWrapper.SeasonRepo.getSeasonsByLanguageCode(req);
                foreach (var season in seasons)
                {
                    season.months = await _repoWrapper.MonthRepo.getMonthsByLanguageCodeAndSeasonID(req.languageCode, season.seasonID);
                }
                resp.response = seasons;
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
