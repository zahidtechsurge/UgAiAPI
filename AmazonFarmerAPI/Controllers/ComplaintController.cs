using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmerAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Claims;
using System.Numerics;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing Complaint operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public ComplaintController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        [HttpPost("addComplaint")]
        public async Task<APIResponse> AddComplaint(AddComplaintRequest Request)
        {
            APIResponse Response = new APIResponse();
            // Get the user ID from the token
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            tblComplaint Complaint = new tblComplaint()
            {
                ComplaintTitle = Request.title,
                ComplaintDesc = Request.desc,
                ComplaintStatus = EComplaintStatus.Pending,
                CreatedByID = userID
            };
            _repoWrapper.ComplaintRepo.AddComplaint(Complaint);
            await _repoWrapper.SaveAsync();
            Response.message = "Complaint added";
            return Response;
        }
        
        [HttpPost("myComplaints")]
        public async Task<APIResponse> MyComplaints(GetComplaintRequest Request)
        {
            APIResponse Response = new APIResponse();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            pagination_Resp inResp = new();
            IQueryable<tblComplaint> complaints = _repoWrapper.ComplaintRepo.GetComplaints();
            complaints = complaints.Where(x => x.CreatedByID == userID);
            inResp.totalRecord = complaints.Count();

            if (!string.IsNullOrEmpty(Request.search))
            {
                complaints = complaints.Where(x =>
                    x.ComplaintTitle.ToLower().Contains(Request.search.ToLower()) ||
                    x.ComplaintDesc.ToLower().Contains(Request.search.ToLower())
                );
            }
            complaints = complaints.OrderByDescending(x => x.ComplaintID);

            complaints = complaints.Skip(Request.skip).Take(Request.take);
            inResp.filteredRecord = complaints.Count();
            inResp.list = await complaints
                .Select(x=> new GetComplaintsList
                {
                    complaintID = x.ComplaintID,
                    title = x.ComplaintTitle,
                    desc = x.ComplaintDesc,
                    statusID = (int)x.ComplaintStatus,
                    createdBy = x.CreatedBy.FirstName,
                    status = ConfigExntension.GetEnumDescription(x.ComplaintStatus)
                })
                .ToListAsync();
            Response.response = inResp;
            return Response;
        }

    }
}
