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
    [Route("api/Admin/City")]
    public class CityController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public CityController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        [HttpPost("addCity")]
        public async Task<JSONResponse> AddCity(AddCityRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblCity? city = await _repoWrapper.CityRepo.GetCityByID(req.cityName, req.cityCode);
            if (city == null)
            {
                city = new tblCity()
                {
                    DistrictID = req.districtID,
                    Name = req.cityName,
                    CityCode = req.cityCode,
                    Status = EActivityStatus.Active
                };
                _repoWrapper.CityRepo.AddCity(city);
                await _repoWrapper.SaveAsync();
                resp.message = "City Added";
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.cityAlreadyExist);
            }
            return resp;
        }
        [HttpPost("getCities")]
        public async Task<APIResponse> GetCities(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblCity> cities = _repoWrapper.CityRepo.GetCities();
            cities = cities.Where(c => c.DistrictID == req.rootID);
            if (!string.IsNullOrEmpty(req.search))
                cities = cities.Where(x => x.Name.Contains(req.search) || x.CityCode.Contains(req.search));
            InResp.totalRecord = cities.Count();
            cities = cities.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = cities.Count();
            InResp.list = await cities.Select(c => new UpdateCityRequest
            {
                districtID = c.DistrictID,
                cityID = c.ID,
                cityCode = c.CityCode,
                cityName = c.Name,
                status = (int)c.Status
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [HttpPut("updateCity")]
        public async Task<JSONResponse> UpdateCity(UpdateCityRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblCity? city = await _repoWrapper.CityRepo.GetCityByID(req.cityID);
            if (city != null)
            {
                city.DistrictID = req.districtID;
                city.Name = req.cityName;
                city.CityCode = req.cityCode;
                city.Status = (EActivityStatus)req.status;
                _repoWrapper.CityRepo.UpdateCity(city);
                await _repoWrapper.SaveAsync();
                resp.message = "City Update";
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.cityNotFound);
            }
            return resp;
        }

        [HttpPost("addCityTranslation")]
        public async Task<JSONResponse> AddCityTranslation(AddCityTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblCityLanguages? cityLanguages = await _repoWrapper.CityRepo.GetCityLanguageByID(req.cityID, req.languageCode);
            if (cityLanguages == null)
            {
                cityLanguages = new tblCityLanguages()
                {
                    Translation = req.text,
                    LanguageCode = req.languageCode,
                    CityID = req.cityID
                };
                _repoWrapper.CityRepo.AddCityLanguage(cityLanguages);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("City translation has been added");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.cityLanguageAlreadyExist);
            }

            return resp;
        }
        [HttpGet("getTranslations/{cityID}")]
        public async Task<APIResponse> GetTranslations(int cityID)
        {
            APIResponse resp = new APIResponse();
            List<tblCityLanguages> cityLanguages = await _repoWrapper.CityRepo.GetCityLanguagesByCityID(cityID);
            resp.response = cityLanguages.Select(cl => new GetCityTranslations
            {
                translationID = cl.ID,
                cityID = cl.ID,
                languageCode = cl.LanguageCode,
                text = cl.Translation,
                languge = cl.Language.LanguageName
            }).ToList();
            return resp;
        }
        [HttpPut("updateCityTranslation")]
        public async Task<JSONResponse> UpdateCityTranslation(UpdateCityTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblCityLanguages? cityLanguages = await _repoWrapper.CityRepo.GetCityLanguageByID(req.translationID);
            if (cityLanguages != null)
            {
                cityLanguages.LanguageCode = req.languageCode;
                cityLanguages.CityID = req.cityID;
                cityLanguages.Translation = req.text;
                _repoWrapper.CityRepo.UpdateCityLanguage(cityLanguages);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat("City translation has been updated");
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.cityNotFound);
            }
            return resp;
        }
    }
}
