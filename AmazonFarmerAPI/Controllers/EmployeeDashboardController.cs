using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/Employee/Dashboard")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class EmployeeDashboardController : Controller
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        public EmployeeDashboardController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }
        [HttpGet("getDashboard")]
        public async Task<APIResponse> GetDashboard()
        {
            APIResponse resp = new APIResponse();
            EmployeeDashboardResponse InResp = new EmployeeDashboardResponse();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int designationID = Convert.ToInt32(User.FindFirst("designationID")?.Value); // Retrieving designation ID from user claims
            if (string.IsNullOrEmpty(userID) && !User.IsInRole("Employee"))
                throw new AmazonFarmerException(_exceptions.APINotAuthorized);
            List<int> territoryIds = new List<int>();

            #region Fetch Pending/completed Farms / Plans Count
            IQueryable<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmsByApplicationIDandLanguageCode(); // Retrieving farms 
            IQueryable<tblPlan> plans = await _repoWrapper.PlanRepo.getPlanList(); // Retriving Plans
            if (designationID == (int)EDesignation.Territory_Sales_Officer)
            {
                territoryIds = !territoryIds.Any() ? await _repoWrapper.UserRepo.GetDistrictIDsForTSO(userID) : territoryIds;
                farms = farms.Where(f => territoryIds.Contains(f.DistrictID));
                plans = plans.Where(x => territoryIds.Contains(x.Farm.DistrictID));
                InResp.completedFarms = farms.Count();
                InResp.pendingFarms = farms.Where(f => (f.Status == EFarmStatus.PendingForTSO || f.Status == EFarmStatus.PendingForPatwari)).Count();
                InResp.completedPlans = plans.Count();
                InResp.pendingPlans = plans.Where(x => x.Status == EPlanStatus.TSOProcessing && (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined)).Count();
                InResp.changeRequestPlans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending && x.Status == EPlanStatus.TSOProcessing).Count();
            }
            else if (designationID == (int)EDesignation.Regional_Sales_Manager)
            {
                territoryIds = !territoryIds.Any() ? await _repoWrapper.UserRepo.GetRegionIDsForRSM(userID) : territoryIds;
                farms = farms.Where(f => territoryIds.Contains(f.DistrictID));
                plans = plans.Where(x => territoryIds.Contains(x.Farm.DistrictID));
                InResp.completedFarms = farms.Count();
                InResp.pendingFarms = farms.Where(f => f.Status == EFarmStatus.PendingforRSM).Count();
                InResp.completedPlans = plans.Count();
                InResp.pendingPlans = plans.Where(x => x.Status == EPlanStatus.RSMProcessing && (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined)).Count();
                InResp.changeRequestPlans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending && x.Status == EPlanStatus.RSMProcessing).Count();
            }
            else if (designationID == (int)EDesignation.National_Sales_Manager)
            {
                InResp.completedFarms = farms.Count();
                InResp.pendingFarms = farms.Count();
                InResp.completedPlans = plans.Count();
                InResp.pendingPlans = plans.Where(x => x.Status == EPlanStatus.NSMProcessing && (x.PlanChangeStatus == EPlanChangeRequest.Default || x.PlanChangeStatus == EPlanChangeRequest.Declined)).Count();
                InResp.changeRequestPlans = plans.Where(x => x.PlanChangeStatus == EPlanChangeRequest.Pending && x.Status == EPlanStatus.NSMProcessing).Count();
            }
            #endregion

            #region Fetch Blocked Orders Count
            IQueryable<TblOrders> orders = await _repoWrapper.OrderRepo.getOrders();
            InResp.blockedOrders = orders.Where(x => territoryIds.Contains(x.Plan.Farm.DistrictID) && x.PaymentStatus == EOrderPaymentStatus.NonPaid && x.DuePaymentDate < DateTime.UtcNow).Count();
            #endregion

            //int totalPlans = (InResp.pendingPlans + InResp.completedPlans + InResp.changeRequestPlans);
            //InResp.pendingPlans = ((InResp.pendingPlans / totalPlans) * 100);
            //InResp.completedPlans = ((InResp.completedPlans / totalPlans) * 100);
            //InResp.changeRequestPlans = ((InResp.changeRequestPlans / totalPlans) * 100);

            resp.response = InResp;

            return resp;
        }
    }
}
