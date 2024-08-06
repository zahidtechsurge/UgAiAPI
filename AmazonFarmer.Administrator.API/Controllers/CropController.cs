using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class CropController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public CropController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("getCrops")]
        public async Task<APIResponse> GetCrops(pagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblCrop> crops = _repoWrapper.CropRepo.GetCrops();
            if (!string.IsNullOrEmpty(req.search))
            {
                crops = crops.Where(x =>
                    x.Name.ToLower().Contains(req.search)
                );
            }
            InResp.totalRecord = crops.Count();
            crops = crops.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = crops.Count();
            InResp.list = await crops.Select(x => new UpdateCropRequest
            {
                cropID = x.ID,
                cropName = x.Name,
                status = (int)x.Status
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }
        [HttpGet("getTranslations/{cropID}")]
        public async Task<APIResponse> GetTranslations(int cropID)
        {
            APIResponse resp = new APIResponse();
            List<tblCropTranslation> cropTrans = await _repoWrapper.CropRepo.GetCropTranslationByCropID(cropID);
            resp.response = cropTrans.Select(x => new GetCropTranslationsResponse
            {
                translationID = x.ID,
                cropID = x.CropID,
                languageCode = x.LanguageCode,
                language = x.Language.LanguageName,
                filePath = x.Image,
                text = x.Text
            }).ToList();
            return resp;
        }
    }
}
