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

        [HttpPost("GetSeasonProductReport")]
        public async Task<APIResponse> GetSeasonProductReport(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            List<PlanStatusResult> report = await _repoWrapper.PlanRepo.GetPlanStatusPagedAsync(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search);
            if (report != null && report.Count() > 0)
            {
                InResp.totalRecord = report.First().TotalRows;
                InResp.filteredRecord = report.Count();
                InResp.list = report.Select(x => new PlanStatusResponse
                {
                    season = x.Season,
                    product = x.Product,
                    plannedPlan = x.PlannedPlan,
                    paidPlan = x.PaidPlan,
                    shippedPlan = x.ShippedPlan,
                    toBeShippedPlan = x.ToBeShippedPlan,
                    toBePaidPlan = x.ToBePaidPlan
                }).ToList();
            }
            else
            {
                InResp.list = new List<PlanStatusResponse>();
            }
            resp.response = InResp;
            return resp;
        }

        [HttpPost("getSeasonCropReport")]
        public async Task<APIResponse> GetSeasonCropReport(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            try
            {

                List<PlanSeasonCropResult> report = await _repoWrapper.PlanRepo.GetPlanSeasonCropPagedAsync(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search);
                if (report != null && report.Count() > 0)
                {
                    InResp.totalRecord = report.First().TotalRows;
                    InResp.filteredRecord = report.Count();
                    InResp.list = report.Select(x => new SeasonCropResponse
                    {
                        season = x.SeasonName,
                        farm = x.FarmName,
                        acreage = (int)x.Acre,
                        month = x.DeliveryMonth,
                        crop = x.CropName,
                        product = x.ProductName,
                        bag = (int)x.Bags,
                        price = x.Value
                    }).ToList();
                }
                else
                {
                    InResp.list = new List<SeasonCropResponse>();
                }
                resp.response = InResp;
                return resp;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
