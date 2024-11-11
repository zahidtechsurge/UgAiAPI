using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Claims;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public ComplaintController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("getComplaints")]
        public async Task<APIResponse> GetComplaints(ReportPagination_Req Request)
        {
            APIResponse Response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblComplaint> Complaints = _repoWrapper.ComplaintRepo.GetComplaints();

            if (!string.IsNullOrEmpty(Request.search))
            {
                Complaints = Complaints.Where(x => 
                    x.ComplaintTitle.ToLower().Contains(Request.search.ToLower()) ||
                    x.ComplaintDesc.ToLower().Contains(Request.search.ToLower()) ||
                    x.CreatedBy.FirstName.ToLower().Contains(Request.search.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(Request.sortColumn))
            {
                if (Request.sortColumn.Contains("complaintID"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        Complaints = Complaints.OrderBy(x => x.ComplaintID);
                    }
                    else
                    {
                        Complaints = Complaints.OrderByDescending(x => x.ComplaintID);
                    }
                }
                else if (Request.sortColumn.Contains("title"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        Complaints = Complaints.OrderBy(x => x.ComplaintTitle);
                    }
                    else
                    {
                        Complaints = Complaints.OrderByDescending(x => x.ComplaintTitle);
                    }
                }
                else if (Request.sortColumn.Contains("desc"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        Complaints = Complaints.OrderBy(x => x.ComplaintDesc);
                    }
                    else
                    {
                        Complaints = Complaints.OrderByDescending(x => x.ComplaintDesc);
                    }
                }
                else if (Request.sortColumn.Contains("status"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        Complaints = Complaints.OrderBy(x => x.ComplaintStatus);
                    }
                    else
                    {
                        Complaints = Complaints.OrderByDescending(x => x.ComplaintStatus);
                    }
                }
                else if (Request.sortColumn.Contains("createdBy"))
                {
                    if (Request.sortOrder.Contains("ASC"))
                    {
                        Complaints = Complaints.OrderBy(x => x.CreatedBy.FirstName);
                    }
                    else
                    {
                        Complaints = Complaints.OrderByDescending(x => x.CreatedBy.FirstName);
                    }
                }
            }
            else
            {
                Complaints = Complaints.OrderByDescending(x => x.ComplaintID);
            }

            InResp.totalRecord = Complaints.Count();
            Complaints = Complaints.Skip(Request.pageNumber * Request.pageSize)
                         .Take(Request.pageSize);
            InResp.filteredRecord = Complaints.Count();


            InResp.list = await Complaints
                .Select(x => new GetComplaintsList
                {
                    complaintID = x.ComplaintID,
                    title = x.ComplaintTitle,
                    desc = x.ComplaintDesc,
                    statusID = (int)x.ComplaintStatus,
                    createdBy = x.CreatedBy.FirstName,
                    status = ConfigExntension.GetEnumDescription(x.ComplaintStatus),
                    createdOn = x.CreatedOn
                })
                .ToListAsync();

            Response.response = InResp;
            return Response;
        }

        [HttpPut("updateComplaint")]
        public async Task<APIResponse> UpdateComplaint(UpdateComplaintRequest Request)
        {
            APIResponse Response = new APIResponse();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            tblComplaint? Complaint = await _repoWrapper.ComplaintRepo.GetComplaintByID(Request.complaintID);
            if (Complaint != null && Complaint.ComplaintStatus != EComplaintStatus.Resolved)
            {
                Complaint.ResolvedByID = userID;
                Complaint.ResolvedOn = DateTime.UtcNow;
                Complaint.ComplaintStatus = (EComplaintStatus)Request.statusID;
                _repoWrapper.ComplaintRepo.UpdateComplaint(Complaint);
                await _repoWrapper.SaveAsync();
                Response.message = string.Concat("complaint has been marked in ", ConfigExntension.GetEnumDescription(Complaint.ComplaintStatus));
            }
            return Response;
        }
    }
}
