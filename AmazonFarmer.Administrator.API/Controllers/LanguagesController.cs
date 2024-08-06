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
    [Route("api/Admin/Languages")]
    public class LanguagesController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public LanguagesController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }
        [HttpPost("getLanguages")]
        public async Task<APIResponse> GetLanguages(pagination_Req req)
        {
            APIResponse response = new APIResponse();
            try
            {
                pagination_Resp InResp = new pagination_Resp();
                IQueryable<tblLanguages> lang = _repoWrapper.LanguageRepo.getLangauges();
                if (!string.IsNullOrEmpty(req.search))
                    lang = lang.Where(x => x.LanguageCode.ToLower().Contains(req.search.ToLower()) || x.LanguageName.ToLower().Contains(req.search.ToLower()));
                InResp.totalRecord = lang.Count();
                lang = lang.Skip(req.pageNumber * req.pageSize)
                             .Take(req.pageSize);
                InResp.filteredRecord = lang.Count();
                InResp.list = await lang.Select(x => new GetLanguageResponse_Admin
                {
                    languageCode = x.LanguageCode,
                    language = x.LanguageName,
                    status = (int)x.Status
                }).ToListAsync();
                response.response = InResp;

            }
            catch (Exception ex)
            {
                response.isError = true;
                response.message = ex.Message;
            }

            return response;
        }
        [HttpPost("addLanguage")]
        public async Task<JSONResponse> AddLanguage(LanguageDTO req)
        {
            JSONResponse response = new JSONResponse();
            if (string.IsNullOrEmpty(req.languageCode) || string.IsNullOrEmpty(req.language))
                throw new AmazonFarmerException(_exceptions.languageNotFound);
            tblLanguages language = await _repoWrapper.LanguageRepo.getLanguageByCodeOrName(req.languageCode, req.language);
            if (language != null)
                throw new AmazonFarmerException(_exceptions.languageAlreadyExist);
            else
            {
                language = new tblLanguages { LanguageCode = req.languageCode, LanguageName = req.language, Status = EActivityStatus.Active };
                _repoWrapper.LanguageRepo.addLanguage(language);
                await _repoWrapper.SaveAsync();
                response.message = "language added";
            }

            return response;
        }
        [HttpPut("updateLanguage")]
        public async Task<JSONResponse> AddLanguage(UpdateLanguageRequest_Admin req)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (string.IsNullOrEmpty(req.languageCode) || string.IsNullOrEmpty(req.language))
                    throw new AmazonFarmerException(_exceptions.languageNotFound);
                tblLanguages language = await _repoWrapper.LanguageRepo.getLanguageByCodeOrName(req.languageCode, string.Empty);
                if (language == null || string.IsNullOrEmpty(language.LanguageCode))
                    throw new AmazonFarmerException(_exceptions.languageAlreadyExist);
                else
                {
                    language.LanguageCode = req.languageCode;
                    language.LanguageName = req.language;
                    language.Status = req.status == 1 ? EActivityStatus.Active : EActivityStatus.DeActive;
                    _repoWrapper.LanguageRepo.updateLanguage(language);
                    await _repoWrapper.SaveAsync();
                    response.message = "language updated";
                }
            }
            catch (Exception ex)
            {
                response.isError = true;
                response.message = ex.Message;
            }

            return response;
        }
        [HttpGet("getActiveLanguages")]
        public async Task<APIResponse> GetActiveLanguages()
        {
            APIResponse resp = new APIResponse();
            IQueryable<tblLanguages> lang = _repoWrapper.LanguageRepo.getLangauges();
            lang = lang.Where(x => x.Status == EActivityStatus.Active);
            resp.response = await lang.Select(x => new LanguageDTO
            {
                languageCode = x.LanguageCode,
                language = x.LanguageName,
            }).ToListAsync();

            return resp;
        }

    }
}
