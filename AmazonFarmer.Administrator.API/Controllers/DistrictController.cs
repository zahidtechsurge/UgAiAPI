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
                regionName = x.Region == null ? string.Empty : x.Region.Name,
                districtName = x.Name,
                districtID = x.ID
            }).ToListAsync();
            return resp;
        }


        [HttpPost("addDistrict")]
        public async Task<JSONResponse> AddDistrict(AddDistrictRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblDistrict? district = await _repoWrapper.DistrictRepo.GetDistrictByID(req.districtName, req.districtCode);
            if (district == null)
            {
                district = new tblDistrict()
                {
                    Name = req.districtName,
                    DistrictCode = req.districtCode,
                    RegionId = req.regionID,
                    Status = EActivityStatus.Active
                };
                _repoWrapper.DistrictRepo.AddDistrict(district);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("District: ", district.Name, " has been added");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.districtAlreadyExist);
            }
            return resp;
        }
        [HttpPost("getDistricts")]
        public async Task<APIResponse> GetDistricts(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblDistrict> districts = _repoWrapper.DistrictRepo.getDistricts();
            resp.message = "Fetched paginated Districts";
            districts = districts.Where(x => x.RegionId == req.rootID);
            if (!string.IsNullOrEmpty(req.search))
                districts = districts.Where(x => x.Name.ToLower().Contains(req.search.ToLower()) || x.DistrictCode.ToLower().Contains(req.search.ToLower()) || x.Region.Name.ToLower().Contains(req.search.ToLower()));
            InResp.totalRecord = districts.Count();
            districts = districts.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = districts.Count();
            InResp.list = await districts.Select(d => new GetPaginatedDIstrict_AdminResponse
            {
                regionID = d.RegionId ?? 0,
                regionName = d.Region == null ? string.Empty : d.Region.Name,
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
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [HttpPut("updateDistrict")]
        public async Task<JSONResponse> UpdateDistrict(UpdateDistrictRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblDistrict? district = await _repoWrapper.DistrictRepo.GetDistrictByID(req.districtName, req.districtCode);
            if (district != null)
            {
                district.Name = req.districtName;
                district.DistrictCode = req.districtCode;
                district.RegionId = req.regionID;
                district.Status = (EActivityStatus)req.status;
                _repoWrapper.DistrictRepo.AddDistrict(district);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("District: ", district.Name, " has been updated");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.districtNotFound);
            }
            return resp;
        }


        [HttpPost("addDistrictTranslation")]
        public async Task<JSONResponse> AddDistrictTranslation(AddDistrictTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblDistrictLanguages? districtLanguage = await _repoWrapper.DistrictRepo.GetDistrictLanguageByID(req.districtID, req.languageCode);
            if (districtLanguage == null)
            {
                districtLanguage = new tblDistrictLanguages()
                {
                    DistrictID = req.districtID,
                    LanguageCode = req.languageCode,
                    Translation = req.text
                };
                _repoWrapper.DistrictRepo.AddDistrictLanguages(districtLanguage);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("District translation has been added");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.districtLanguageAlreadyExist);
            }
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
        [HttpPut("updateDistrictTranslation")]
        public async Task<JSONResponse> UpdateDistrictTranslation(UpdateDistrictTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblDistrictLanguages? districtLanguage = await _repoWrapper.DistrictRepo.GetDistrictLanguageByID(req.districtID, req.languageCode);
            if (districtLanguage != null)
            {
                districtLanguage.Translation = req.text;
                _repoWrapper.DistrictRepo.UpdateDistrictLanguages(districtLanguage);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("District translation has been updated");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.districtLanguageAlreadyExist);
            }
            return resp;
        }


    }
}
