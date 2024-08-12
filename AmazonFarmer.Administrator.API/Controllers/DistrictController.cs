using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
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
    [Route("api/Admin/District")]
    public class DistrictController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public DistrictController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        [HttpGet("getDistricts")]
        public async Task<APIResponse> GetDistricts()
        {
            APIResponse resp = new APIResponse();
            resp.message = "Fetched Districts";
            IQueryable<tblDistrict> districts = _repoWrapper.DistrictRepo.getDistricts();
            districts.Where(x => x.Status == EActivityStatus.Active).ToList();
            resp.response = await districts.Select(x => new GetDIstrict_AdminResponse
            {
                districtName = x.Name,
                districtID = x.ID
            }).ToListAsync();
            return resp;
        }

        [HttpPost("getDistricts")]
        public async Task<APIResponse> GetDistricts(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblDistrict> districts = _repoWrapper.DistrictRepo.getDistricts();
            resp.message = "Fetched paginated Districts";
            if (!string.IsNullOrEmpty(req.search))
                districts = districts.Where(x => x.Name.ToLower().Contains(req.search.ToLower()) || x.DistrictCode.ToLower().Contains(req.search.ToLower()) || x.Region.Name.ToLower().Contains(req.search.ToLower()));
            InResp.totalRecord = districts.Count();
            districts = districts.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = districts.Count();
            InResp.list = districts.Select(d => new GetPaginatedDIstrict_AdminResponse
            {
                districtID = d.ID,
                districtName = d.Name,
                districtCode = d.DistrictCode,
                status = d.Status == EActivityStatus.Active ? true : false,
                translations = d.DistrictLanguages.Select(x => new GetDistrictTranslations
                {
                    translationID = x.ID,
                    districtID = x.DistrictID,
                    languageCode = x.LanguageCode,
                    language = x.Languages.LanguageName,
                    text = x.Translation
                }).ToList(),
            });
            resp.response = InResp;
            return resp;
        }

        [HttpGet("getTranslations/{districtID}")]
        public async Task<APIResponse> GetTranslations(int districtID)
        {
            APIResponse resp = new APIResponse();
            List<tblDistrictLanguages> DistrictTranslations = await _repoWrapper.DistrictRepo.GetDistrictLanguagesByID(districtID);
            resp.response = DistrictTranslations.Select(x => new GetDistrictTranslations
            {
                translationID = x.ID,
                districtID = x.DistrictID,
                text = x.Translation,
                languageCode = x.LanguageCode,
                language = x.Languages.LanguageName
            }).ToList();
            return resp;
        }
        [HttpPost("addDistrict")]
        public async Task<APIResponse> AddDistrict()
        {
            APIResponse resp = new APIResponse();

            return resp;
        }

    }
}
