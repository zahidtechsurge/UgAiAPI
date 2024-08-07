using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing authority letter operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Employee/Reports")]
    public class EmployeeReportsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public EmployeeReportsController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("GetSeasonProductReport")]
        public async Task<APIResponse> GetSeasonProductReport()
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();

            //List<tblSeason> seasons = await _repoWrapper.SeasonRepo.getSeasons();
            IQueryable<tblSeason> seasons = _repoWrapper.PlanRepo.getSeasonProductReport();

            InResp.totalRecord = seasons.Count();
            List<GetSeasonProductResponse> lst = new List<GetSeasonProductResponse>();
            foreach (var season in seasons)
            {
                //string sName = season.Name;
                //foreach (var item in season.plans)
                //{

                //}
                GetSeasonProductResponse se = new GetSeasonProductResponse()
                {
                    season = season.Name,
                    product = string.Empty,
                    paid = season.plans.Where(p => p.IsPlanPaid).Count()
                };
                lst.Add(se);
            }
            InResp.list = lst;
            InResp.filteredRecord = lst.Count();

            resp.response = InResp;
            return resp;
        }

    }
}
