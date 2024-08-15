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
    [Route("api/Admin/Tehsil")]
    public class TehsilController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public TehsilController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        #region Tehsil
        [HttpPost("getTehsils")]
        public async Task<APIResponse> GetTehsils(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblTehsil> tehsils = _repoWrapper.TehsilRepo.GetTehsils();
            resp.message = "Fetched paginated Districts";
            tehsils = tehsils.Where(x => x.CityID == req.rootID);
            if (!string.IsNullOrEmpty(req.search))
                tehsils = tehsils.Where(x => x.Name.ToLower().Contains(req.search.ToLower()) || x.TehsilCode.ToLower().Contains(req.search.ToLower()));
            InResp.totalRecord = tehsils.Count();
            tehsils = tehsils.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = tehsils.Count();
            InResp.list = await tehsils
                .Select(x => new UpdateTehsilRequest
                {
                    tehsilID = x.ID,
                    cityID = x.CityID,
                    tehsilCode = x.TehsilCode,
                    tehsilName = x.Name,
                    status = (int)EActivityStatus.Active
                })
                .ToListAsync();
            resp.response = InResp;
            return resp;
        }

        [HttpPost("addTehsil")]
        public async Task<JSONResponse> AddTehsil(AddTehsilRequest req)
        {
            tblTehsil? tehsil = await _repoWrapper.TehsilRepo.GetTehsilByID(req.tehsilName, req.tehsilCode);
            if (tehsil == null)
            {
                JSONResponse resp = new JSONResponse();
                tehsil = new tblTehsil()
                {
                    CityID = req.cityID,
                    Name = req.tehsilName,
                    TehsilCode = req.tehsilCode,
                    Status = EActivityStatus.Active
                };
                _repoWrapper.TehsilRepo.AddTehsil(tehsil);
                await _repoWrapper.SaveAsync();
                resp.message = "Tehsil Added";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.tehsilAlreadyExist);
        }
        [HttpPut("updateTehsil")]
        public async Task<JSONResponse> UpdateTehsil(UpdateTehsilRequest req)
        {
            tblTehsil? tehsil = await _repoWrapper.TehsilRepo.GetTehsilByID(req.tehsilID);
            if (tehsil != null)
            {
                JSONResponse resp = new JSONResponse();
                tehsil.CityID = req.cityID;
                tehsil.Name = req.tehsilName;
                tehsil.TehsilCode = req.tehsilCode;
                tehsil.Status = (EActivityStatus)req.status;
                _repoWrapper.TehsilRepo.UpdateTehsil(tehsil);
                await _repoWrapper.SaveAsync();
                resp.message = "Tehsil Updated";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.tehsilNotFound);
        }
        #endregion


        #region Tehsil Translation
        [HttpGet("getTranslations/{tehsilID}")]
        public async Task<APIResponse> GetTranslations(int tehsilID)
        {
            APIResponse resp = new APIResponse();
            List<tblTehsilLanguages> tehsilLanguages = await _repoWrapper.TehsilRepo.GetTehsilLanguagesByTehsilID(tehsilID);
            resp.response = tehsilLanguages.Select(x => new GetTehsilTranslationRequest
            {
                translationID = x.ID,
                tehsilID = x.TehsilID,
                languageCode = x.LanguageCode,
                language = x.Language.LanguageName,
                text = x.Translation
            }).ToList();
            return resp;
        }
        [HttpPost("addTranslation")]
        public async Task<JSONResponse> AddTranslation(AddTehsilTranslationRequest req)
        {
            tblTehsilLanguages? tehsilLanguage = await _repoWrapper.TehsilRepo.GetTehsilLanguageByID(req.tehsilID, req.languageCode);
            if (tehsilLanguage == null)
            {
                JSONResponse resp = new JSONResponse();
                tehsilLanguage = new tblTehsilLanguages()
                {
                    TehsilID = req.tehsilID,
                    LanguageCode = req.languageCode,
                    Translation = req.text
                };
                _repoWrapper.TehsilRepo.AddTehsilTrasnaltion(tehsilLanguage);
                await _repoWrapper.SaveAsync();
                resp.message = "Tehsil translation added";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.tehsilAlreadyExist);
        }
        [HttpPut("updateTranslation")]
        public async Task<JSONResponse> UpdateTranslation(UpdateTehsilTranslationRequest req)
        {
            tblTehsilLanguages? tehsilLanguage = await _repoWrapper.TehsilRepo.GetTehsilLanguageByID(req.translationID);
            if (tehsilLanguage != null)
            {
                JSONResponse resp = new JSONResponse();
                tehsilLanguage.TehsilID = req.tehsilID;
                tehsilLanguage.LanguageCode = req.languageCode;
                tehsilLanguage.Translation = req.text;
                _repoWrapper.TehsilRepo.UpdateTehsilTranslation(tehsilLanguage);
                await _repoWrapper.SaveAsync();
                resp.message = "Tehsil translation Updated";
                return resp;
            }
            else
                throw new AmazonFarmerException(_exceptions.tehsilNotFound);
        }



        #endregion
    }
}
