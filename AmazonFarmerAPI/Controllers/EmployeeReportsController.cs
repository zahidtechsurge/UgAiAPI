using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Claims;

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

        [AllowAnonymous]
        [HttpPost("GetSeasonProductReport")]
        public async Task<APIResponse> GetSeasonProductReport(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            //var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            //List<PlanStatusResult> report = await _repoWrapper.PlanRepo.GetPlanStatusPagedAsync(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search, userID);
            List<PlanStatusResult> report = await _repoWrapper.PlanRepo.GetPlanStatusPagedAsync(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search, string.Empty);
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

        [AllowAnonymous]
        [HttpPost("getSeasonCropReport")]
        public async Task<APIResponse> GetSeasonCropReport(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();

            //var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieving UserID from user claims
            //List<PlanSeasonCropResult> report = await _repoWrapper.PlanRepo.GetPlanSeasonCropPagedAsync(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search, userID);
            List<PlanSeasonCropResult> report = await _repoWrapper.PlanRepo.GetPlanSeasonCropPagedAsync(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search, string.Empty);
            if (report != null && report.Count() > 0)
            {
                InResp.totalRecord = report.Count > 0 ? report.First().TotalRows : 0;
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
    }
}
