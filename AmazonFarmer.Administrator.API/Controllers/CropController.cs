﻿using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
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
        [HttpGet("getCropTimings/{cropID}")]
        public async Task<APIResponse> GetCropTimings(int cropID)
        {
            APIResponse resp = new APIResponse();
            List<tblCropTimings> cropTimings = await _repoWrapper.CropRepo.GetCropTimingsByCropID(cropID);
            resp.response = cropTimings.Select(ct => new GetCropTimingsResponse
            {
                recID = ct.ID,
                cropID = ct.CropID,
                cropName = ct.Crop.Name,
                districtID = ct.DistrictID,
                districtName = ct.District.Name,
                seasonID = ct.SeasonID,
                seasonName = ct.Season.Name,
                fromMonth = ct.FromDate.Month,
                toMonth = ct.ToDate.Month,
                statusID = (int)ct.Status
            }).ToList();
            return resp;
        }
        [HttpPost("addCropTiming")]
        public async Task<JSONResponse> AddCropTiming(AddCropTiming req)
        {
            JSONResponse resp = new JSONResponse();
            tblCropTimings? ct = await _repoWrapper.CropRepo.GetCropTimingByID(req.cropID, req.seasonID, req.districtID, req.fromMonth, req.toMonth);
            if (ct != null)
            {
                ct.Status = EActivityStatus.Active;
                _repoWrapper.CropRepo.UpdateCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop timing updated";
            }
            else
            {
                int currentYear = DateTime.UtcNow.Year;
                ct = new tblCropTimings()
                {
                    CropID = req.cropID,
                    DistrictID = req.districtID,
                    SeasonID = req.seasonID,
                    FromDate = new DateTime(currentYear, req.fromMonth, 1),
                    ToDate = new DateTime(currentYear, req.fromMonth, DateTime.DaysInMonth(currentYear, req.toMonth)),
                    Status = EActivityStatus.Active
                };
                _repoWrapper.CropRepo.AddCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop timing added";
            }

            return resp;
        }
        [HttpPut("updateCropTiming")]
        public async Task<JSONResponse> UpdateCropTiming(UpdateCropTiming req)
        {
            JSONResponse resp = new JSONResponse();
            tblCropTimings? ct = await _repoWrapper.CropRepo.GetCropTimingByID(req.recID);
            if (ct != null)
            {
                ct.SeasonID = req.seasonID;
                ct.DistrictID = req.districtID;
                ct.CropID = req.cropID;
                ct.Status = (EActivityStatus)req.statusID;
                _repoWrapper.CropRepo.UpdateCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop Timing Updated";
            }

            return resp;
        }
        [HttpDelete("deleteCropTiming/{recID}")]
        public async Task<JSONResponse> DeleteCropTiming(int recID)
        {
            JSONResponse resp = new JSONResponse();
            tblCropTimings? ct = await _repoWrapper.CropRepo.GetCropTimingByID(recID);
            if (ct != null)
            {
                ct.Status = EActivityStatus.DeActive;
                _repoWrapper.CropRepo.UpdateCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop Timing Deleted";
            }
            return resp;
        }

    }
}