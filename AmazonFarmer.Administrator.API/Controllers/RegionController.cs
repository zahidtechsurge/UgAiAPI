using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Administrator.API.Controllers
{
    /// <summary>
    /// Controller for managing language-related operations.
    /// </summary>
    [EnableCors("corsPolicy")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/Region")]
    public class RegionController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public RegionController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }
        [HttpGet("getRegions")]
        public async Task<APIResponse> getRegions()
        {
            APIResponse resp = new APIResponse();
            IQueryable<tblRegion> regions = _repoWrapper.RegionRepo.GetRegions();
            regions = regions.Where(x => x.Status == EActivityStatus.Active);
            resp.response = await regions.Select(x => new
            {
                regionID = x.ID,
                regionName = x.Name
            }).ToListAsync();
            return resp;
        }

        [HttpPost("addRegion")]
        public async Task<JSONResponse> AddRegion(AddRegionRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblRegion? region = await _repoWrapper.RegionRepo.GetRegion(req.regionName, req.regionCode);
            if (region == null)
            {
                region = new tblRegion()
                {
                    Name = req.regionName,
                    RegionCode = req.regionCode,
                    Status = EActivityStatus.Active
                };
                _repoWrapper.RegionRepo.AddRegion(region);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("Region: ", region.Name, " has been added");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.regionAlreadyExist);
            }

            return resp;
        }

        [HttpPost("getRegions")]
        public async Task<APIResponse> GetRegions(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblRegion> regions = _repoWrapper.RegionRepo.GetRegions();
            resp.message = "Fetched paginated Regions";
            if (!string.IsNullOrEmpty(req.search))
                regions = regions.Where(x => x.Name.Contains(req.search) || x.RegionCode.Contains(req.search));
            InResp.totalRecord = regions.Count();
            regions = regions.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = regions.Count();
            InResp.list = await regions.Select(x => new UpdateRegionRequest
            {
                regionID = x.ID,
                regionName = x.Name,
                regionCode = x.RegionCode,
                status = (int)x.Status
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [HttpPut("updateRegion")]
        public async Task<JSONResponse> UpdateRegion(UpdateRegionRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblRegion? region = await _repoWrapper.RegionRepo.GetRegion(req.regionID);
            if (region != null)
            {
                region.Name = req.regionName;
                region.RegionCode = req.regionCode;
                region.Status = (EActivityStatus)req.status;
                _repoWrapper.RegionRepo.UpdateRegion(region);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("Region: ", region.Name, " has been updated");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.regionNotFound);
            }
            return resp;
        }
    }
}
