using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
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
        private readonly IAzureFileShareService _azureFileShareService;
        public CropController(IRepositoryWrapper repoWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repoWrapper;
            _azureFileShareService = azureFileShareService;
        }

        #region Crop
        [HttpPost("addCrop")]
        public async Task<JSONResponse> AddCrop(AddCropRequest req)
        {
            tblCrop? crop = await _repoWrapper.CropRepo.GetCropByName(req.cropName);
            if (crop == null)
            {
                JSONResponse resp = new JSONResponse();
                crop = new tblCrop()
                {
                    Name = req.cropName,
                    Status = EActivityStatus.Active
                };
                _repoWrapper.CropRepo.AddCrop(crop);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop added";
                return resp;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.cropAlreadyExist);
            }
        }
        [HttpPost("getCrops")]
        public async Task<APIResponse> GetCrops(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblCrop> crops = _repoWrapper.CropRepo.GetCrops();

            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("cropID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        crops = crops.OrderBy(x => x.ID);
                    }
                    else
                    {
                        crops = crops.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("cropName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        crops = crops.OrderBy(x => x.Name);
                    }
                    else
                    {
                        crops = crops.OrderByDescending(x => x.Name);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        crops = crops.OrderBy(x => x.Status);
                    }
                    else
                    {
                        crops = crops.OrderByDescending(x => x.Status);
                    }
                }

            }
            else
            {
                crops = crops.OrderByDescending(x => x.ID);
            }
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
        [HttpPut("updateCrop")]
        public async Task<JSONResponse> UpdateCrop(UpdateCropRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblCrop? crop = await _repoWrapper.CropRepo.GetCropByID(req.cropID);
            if (crop != null)
            {
                crop.Name = req.cropName;
                crop.Status = (EActivityStatus)req.status;
                _repoWrapper.CropRepo.UpdateCrop(crop);
                await _repoWrapper.SaveAsync();
                resp.message = "crop updated";
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.cropNotFound);
            }
            return resp;
        }
        #endregion

        #region Crop Translation
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
                filePath = string.Concat(ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"), x.Image.Replace("/", "%2F").Replace(" ", "%20")),
                text = x.Text
            }).ToList();
            return resp;
        }
        //[Obsolete]
        //[HttpPost("addCropTranslation")]
        //public async Task<JSONResponse> AddCropTranslation(AddCropTranslationRequest req)
        //{
        //    JSONResponse resp = new JSONResponse();
        //    tblCropTranslation? ct = await _repoWrapper.CropRepo.GetCropTranslationByCropID(req.cropID, req.languageCode);
        //    if (ct != null)
        //    {
        //        ct.Image = req.filePath ?? await getImagePathByContent(req.fileName ?? "untitledCrop.svg", (req.content ?? string.Empty));
        //        ct.Text = req.text;
        //        //ct.Status = EActivityStatus.Active;
        //        _repoWrapper.CropRepo.UpdateCropTranslation(ct);
        //        await _repoWrapper.SaveAsync();
        //        resp.message = "Translation updated";
        //    }
        //    else
        //    {
        //        ct = new tblCropTranslation()
        //        {
        //            CropID = req.cropID,
        //            LanguageCode = req.languageCode,
        //            Image = req.filePath ?? await getImagePathByContent(req.fileName ?? "untitledCrop.svg", (req.content ?? string.Empty)),
        //            Text = req.text,
        //            //Status = EActivityStatus.Active
        //        };
        //        _repoWrapper.CropRepo.AddCropTranslation(ct);
        //        await _repoWrapper.SaveAsync();
        //        resp.message = "Translation added";
        //    }
        //    return resp;
        //}
        //[Obsolete]
        //[HttpPut("updateCropTranslation")]
        //public async Task<JSONResponse> UpdateCropTranslation(UpdateCropTranslationRequest req)
        //{
        //    JSONResponse resp = new JSONResponse();
        //    tblCropTranslation? ct = await _repoWrapper.CropRepo.GetCropTranslationByCropID(req.cropID, req.languageCode);
        //    if (ct != null)
        //    {
        //        ct.Image = req.filePath ?? await getImagePathByContent(req.fileName ?? "untitledCrop.svg", (req.content ?? string.Empty));
        //        ct.Text = req.text;
        //        //ct.Status = EActivityStatus.Active;
        //        _repoWrapper.CropRepo.UpdateCropTranslation(ct);
        //        await _repoWrapper.SaveAsync();
        //        resp.message = "Translation updated";
        //    }

        //    return resp;
        //}

        [HttpPatch("syncCropTranslation")]
        public async Task<JSONResponse> SyncCropTranslation(UpdateCropTranslationRequest req)
        {
            JSONResponse resp = new JSONResponse();
            tblCropTranslation? ct = await _repoWrapper.CropRepo.GetCropTranslationByCropID(req.cropID, req.languageCode);
            if (ct != null)
            {
                ct.Image = !string.IsNullOrEmpty(req.filePath) ? req.filePath : await getImagePathByContent(req.fileName ?? "untitledCrop.svg", (req.content ?? string.Empty));
                ct.Text = req.text;
                //ct.Status = EActivityStatus.Active;
                _repoWrapper.CropRepo.UpdateCropTranslation(ct);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                ct = new tblCropTranslation()
                {
                    CropID = req.cropID,
                    LanguageCode = req.languageCode,
                    Text = req.text,
                    Image = !string.IsNullOrEmpty(req.filePath) ? req.filePath : await getImagePathByContent(req.fileName ?? "untitledCrop.svg", (req.content ?? string.Empty)),
                };
                //ct.Status = EActivityStatus.Active;
                _repoWrapper.CropRepo.AddCropTranslation(ct);
                await _repoWrapper.SaveAsync();
            }
            resp.message = "Translation updated";
            return resp;
        }
        #endregion

        #region Crop Timings
        [HttpPost("getCropTimings")]
        public async Task<APIResponse> GetCropTimings(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<tblCropTimings> cropTimings = _repoWrapper.CropRepo.GetCropTimings();
            if (req.rootID.HasValue)
            {
                cropTimings = cropTimings.Where(x => x.CropID == req.rootID.Value);
                //List<tblCropTimings> cropTimings = await _repoWrapper.CropRepo.GetCropTimingsByCropID(req.rootID.Value);
            }
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("recID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.ID);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.ID);
                    }
                }
                else if (req.sortColumn.Contains("cropID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.CropID);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.CropID);
                    }
                }
                else if (req.sortColumn.Contains("cropName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.Crop.Name);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.Crop.Name);
                    }
                }
                else if (req.sortColumn.Contains("districtID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.DistrictID);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.DistrictID);
                    }
                }
                else if (req.sortColumn.Contains("districtName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.District.Name);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.District.Name);
                    }
                }
                else if (req.sortColumn.Contains("seasonID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.SeasonID);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.SeasonID);
                    }
                }
                else if (req.sortColumn.Contains("seasonName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.Season.Name);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.Season.Name);
                    }
                }
                else if (req.sortColumn.Contains("fromMonth"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.FromDate);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.FromDate);
                    }
                }
                else if (req.sortColumn.Contains("toMonth"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        cropTimings = cropTimings.OrderBy(x => x.ToDate);
                    }
                    else
                    {
                        cropTimings = cropTimings.OrderByDescending(x => x.ToDate);
                    }
                }

            }
            else
            {
                cropTimings = cropTimings.OrderByDescending(x => x.ID);
            }
            if (!string.IsNullOrEmpty(req.search))
            {
                cropTimings = cropTimings.Where(x =>
                    x.Crop.Name.ToLower().Contains(req.search.ToLower()) ||
                    x.District.Name.ToLower().Contains(req.search.ToLower()) ||
                    x.Season.Name.ToLower().Contains(req.search.ToLower())
                );
            }

            InResp.totalRecord = cropTimings.Count();
            cropTimings = cropTimings.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = cropTimings.Count();

            InResp.list = await cropTimings.Select(ct => new GetCropTimingsResponse
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
                fromDate_String = ct.FromDate.ToString("MMM"),
                toDate_String = ct.ToDate.ToString("MMM")
                //statusID = (int)ct.Status
            }).ToListAsync();
            resp.response = InResp;
            return resp;
        }

        [HttpPost("syncCropTiming")]
        public async Task<JSONResponse> SyncCropTiming(UpdateCropTiming req)
        {
            JSONResponse resp = new JSONResponse();
            int currentYear = DateTime.UtcNow.Year;
            tblCropTimings? ct = await _repoWrapper.CropRepo.GetCropTimingByID(req.recID);
            if (ct != null)
            {
                ct.CropID = req.cropID;
                ct.DistrictID = req.districtID;
                ct.SeasonID = req.seasonID;
                ct.FromDate = new DateTime(currentYear, req.fromMonth, 1);
                ct.ToDate = new DateTime(currentYear, req.toMonth, DateTime.DaysInMonth(currentYear, req.toMonth));
                //ct.Status = EActivityStatus.Active;
                _repoWrapper.CropRepo.UpdateCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop timing updated";
            }
            else
            {
                ct = new tblCropTimings()
                {
                    CropID = req.cropID,
                    DistrictID = req.districtID,
                    SeasonID = req.seasonID,
                    FromDate = new DateTime(currentYear, req.fromMonth, 1),
                    ToDate = new DateTime(currentYear, req.toMonth, DateTime.DaysInMonth(currentYear, req.toMonth)),
                    //Status = EActivityStatus.Active
                };
                _repoWrapper.CropRepo.AddCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop timing added";
            }

            return resp;
        }
        [Obsolete]
        [HttpPut("updateCropTiming")]
        public async Task<JSONResponse> UpdateCropTiming(UpdateCropTiming req)
        {
            JSONResponse resp = new JSONResponse();
            tblCropTimings? ct = await _repoWrapper.CropRepo.GetCropTimingByID(req.recID);
            if (ct != null)
            {
                int currentYear = DateTime.UtcNow.Year;
                ct.SeasonID = req.seasonID;
                ct.DistrictID = req.districtID;
                ct.CropID = req.cropID;
                ct.FromDate = new DateTime(currentYear, req.fromMonth, 1);
                ct.ToDate = new DateTime(currentYear, req.fromMonth, DateTime.DaysInMonth(currentYear, req.toMonth));
                //ct.Status = (EActivityStatus)req.statusID;
                _repoWrapper.CropRepo.UpdateCropTiming(ct);
                await _repoWrapper.SaveAsync();
                resp.message = "Crop Timing Updated";
            }

            return resp;
        }
        //[Obsolete]
        //[HttpDelete("deleteCropTiming/{recID}")]
        //public async Task<JSONResponse> DeleteCropTiming(int recID)
        //{
        //    JSONResponse resp = new JSONResponse();
        //    tblCropTimings? ct = await _repoWrapper.CropRepo.GetCropTimingByID(recID);
        //    if (ct != null)
        //    {
        //        //ct.Status = EActivityStatus.DeActive;
        //        _repoWrapper.CropRepo.UpdateCropTiming(ct);
        //        await _repoWrapper.SaveAsync();
        //        resp.message = "Crop Timing Deleted";
        //    }
        //    return resp;
        //}
        /// <summary>
        /// Updates crop timings for all active districts and seasons based on the given cropID.
        /// It fetches the existing crop timings and determines which new crop timings need to be added.
        /// All new crop timings are then saved to the database.
        /// </summary>
        /// <param name="cropID">The ID of the crop for which timings need to be set.</param>
        /// <returns>A JSON response indicating the result of the operation.</returns>

        [HttpPut("setCropForAll")]
        public async Task<JSONResponse> SetCropForAll(CropTimingValues req)
        {
            JSONResponse resp = new JSONResponse();
            // Fetching data from repositories
            List<tblSeason> seasons = await _repoWrapper.SeasonRepo.getSeasons();
            List<tblDistrict> districts = await _repoWrapper.DistrictRepo.GetDistricts();
            var existingCropTimings = await _repoWrapper.CropRepo.GetCropTimingsByCropID(req.cropID);
            // Preparing new crop timings list
            List<tblCropTimings> cropTimings = new List<tblCropTimings>();
            int currentYear = DateTime.UtcNow.Year;
            // Filtering active seasons
            foreach (var season in seasons.Where(s => s.Status == EActivityStatus.Active))
            {
                // Adding new crop timings for the filtered districts
                cropTimings.AddRange(
                    // Filtering active districts that are not already in the existing crop timings
                    districts
                    .Where(d =>
                        d.Status == EActivityStatus.Active &&
                        !existingCropTimings.Any(
                            ect => ect.DistrictID == d.ID && ect.SeasonID == season.ID
                        )
                    )
                    .Select(d => new tblCropTimings()
                    {
                        CropID = req.cropID,
                        DistrictID = d.ID,
                        SeasonID = season.ID,
                        FromDate = new DateTime(currentYear, req.fromMonth, 1),
                        ToDate = new DateTime(currentYear, req.fromMonth, DateTime.DaysInMonth(currentYear, req.toMonth))
                    }).ToList()
                );
            }
            // Saving all new crop timings to the database
            if (cropTimings.Any())
            {
                _repoWrapper.CropRepo.AddCropTimings(cropTimings);
                await _repoWrapper.SaveAsync();
            }

            return resp;
        }
        #endregion



        private async Task<string> getImagePathByContent(string name, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("data:image/png;base64,", "");
                AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
                AttachmentsDTO attachment = await attachmentExt.UploadAttachment(name: name, content: content, requestTypeID: EAttachmentType.Crop);
                return string.Concat("/", attachment.filePath.Replace("\\", "/"));
            }
            throw new AmazonFarmerException("file path or content not found");
        }
    }
}
