using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [EnableCors("corsPolicy")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/Seasons")]
    public class SeasonsController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public SeasonsController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }


        #region Season

        [HttpGet("getSeasons")]
        public async Task<APIResponse> GetSeasons()
        {
            APIResponse resp = new APIResponse();
            List<tblSeason> seasons = await _repoWrapper.SeasonRepo.getSeasons();
            resp.response = seasons
                .Where(s => s.Status == EActivityStatus.Active)
                .Select(s => new
                {
                    seasonID = s.ID,
                    seasonName = s.Name,
                    fromMonth = s.StartDate.Month,
                    toMonth = s.EndDate.Month
                })
                .ToList();
            return resp;
        }

        [HttpPost("getSeasons")]
        public async Task<APIResponse> GetSeasons(ReportPagination_Req req)
        {
            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();

            List<tblSeason> seasons = await _repoWrapper.SeasonRepo.getSeasons();

            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("seasonID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        seasons = seasons.OrderBy(x => x.ID).ToList();
                    }
                    else
                    {
                        seasons = seasons.OrderByDescending(x => x.ID).ToList();
                    }
                }
                else if (req.sortColumn.Contains("seasonName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        seasons = seasons.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        seasons = seasons.OrderByDescending(x => x.Name).ToList();
                    }
                }
                else if (req.sortColumn.Contains("fromMonth"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        seasons = seasons.OrderBy(x => x.StartDate).ToList();
                    }
                    else
                    {
                        seasons = seasons.OrderByDescending(x => x.StartDate).ToList();
                    }
                }
                else if (req.sortColumn.Contains("toMonth"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        seasons = seasons.OrderBy(x => x.EndDate).ToList();
                    }
                    else
                    {
                        seasons = seasons.OrderByDescending(x => x.EndDate).ToList();
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        seasons = seasons.OrderBy(x => x.Status).ToList();
                    }
                    else
                    {
                        seasons = seasons.OrderByDescending(x => x.Status).ToList();
                    }
                }
            }
            else
            {
                seasons = seasons.OrderByDescending(x => x.ID).ToList();
            }

            if (!string.IsNullOrEmpty(req.search))
            {
                seasons = seasons.Where(x =>
                    x.ID.ToString().ToLower().Contains(req.search.ToLower()) ||
                    x.Name.ToLower().Contains(req.search.ToLower()) ||
                    x.StartDate.ToString("MMMM").ToLower().Contains(req.search.ToLower()) ||
                    x.EndDate.ToString("MMMM").ToLower().Contains(req.search.ToLower())
                ).ToList();
            }

            InResp.totalRecord = seasons.Count();
            seasons = seasons.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize).ToList();
            InResp.filteredRecord = seasons.Count();
            InResp.list = seasons.Select(x => new SeasonResponse
            {
                seasonID = x.ID,
                seasonName = x.Name,
                fromMonthID = x.StartDate.Month,
                fromMonth = x.StartDate.ToString("MMMM"),
                toMonthID = x.EndDate.Month,
                toMonth = x.EndDate.ToString("MMMM"),
                status = (int)x.Status
            }).ToList();
            resp.response = InResp;

            return resp;
        }
        [Obsolete]
        [HttpPost("addSeason")]
        public async Task<JSONResponse> AddSeason(AddSeasonRequest req)
        {
            ValidateSeason(req);

            //if (await _repoWrapper.SeasonRepo.SeasonExistInDateRange(req.fromMonthID, req.toMonthID))
            if (true)
            {
                tblSeason? season = await _repoWrapper.SeasonRepo.GetSeasonByName(req.seasonName);
                if (season == null)
                {
                    JSONResponse resp = new JSONResponse();
                    int currentYear = DateTime.UtcNow.Year;

                    season = new tblSeason()
                    {
                        Name = req.seasonName,
                        StartDate = new DateTime(currentYear, req.fromMonthID, 1),
                        EndDate = new DateTime(currentYear, req.toMonthID, DateTime.DaysInMonth(currentYear, req.toMonthID)),
                        Status = EActivityStatus.Active
                    };
                    _repoWrapper.SeasonRepo.AddSeason(season);
                    await _repoWrapper.SaveAsync();
                    resp.message = "Season Added";
                    return resp;
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.seasonAlreadyExist);
                }
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.seasonRangeOverlaping);
            }
        }
        [HttpPut("updateSeason")]
        public async Task<JSONResponse> UpdateSeason(UpdateSeasonRequest req)
        {
            ValidateSeason(req);
            //if (await _repoWrapper.SeasonRepo.SeasonExistInDateRange(req.fromMonthID, req.toMonthID))
            if (true)
            {
                tblSeason? season = await _repoWrapper.SeasonRepo.GetSeasonByID(req.seasonID);
                if (season == null)
                {
                    throw new AmazonFarmerException(_exceptions.seasonNotFound);
                }
                else
                {
                    JSONResponse resp = new JSONResponse();
                    int currentYear = DateTime.UtcNow.Year;
                    season.Name = req.seasonName;
                    season.StartDate = new DateTime(currentYear, req.fromMonthID, 1);
                    season.EndDate = new DateTime(currentYear, req.toMonthID, DateTime.DaysInMonth(currentYear, req.toMonthID));
                    season.Status = (EActivityStatus)req.status;
                    _repoWrapper.SeasonRepo.UpdateSeason(season);
                    await _repoWrapper.SaveAsync();
                    resp.message = "Season Updated";
                    return resp;
                }
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.seasonRangeOverlaping);
            }
        }
        #endregion

        #region Season Translation
        [HttpGet("getTranslations/{seasonID}")]
        public async Task<APIResponse> GetTranslations(int seasonID)
        {
            APIResponse resp = new APIResponse();
            List<tblSeasonTranslation> tranlsation = await _repoWrapper.SeasonRepo.GetSeasonTranslationBySeasonID(seasonID);
            resp.response = tranlsation.Select(t => new SyncSeasonTranslationResponse
            {
                translationID = t.ID,
                seasonID = seasonID,
                languageCode = t.LanguageCode,
                language = t.Language.LanguageName,
                season = t.Season.Name,
                text = t.Translation
            }).ToList();
            return resp;
        }
        [HttpPatch("syncSeasonTranslation")]
        public async Task<JSONResponse> syncSeasonTranslation(SyncSeasonTranslationDTO req)
        {
            JSONResponse response = new JSONResponse();

            tblSeasonTranslation? seasonTranslation = await _repoWrapper.SeasonRepo.GetSeasonTranslationBySeasonID(req.seasonID, req.languageCode);
            if (seasonTranslation == null)
            {
                seasonTranslation = new tblSeasonTranslation()
                {
                    SeasonID = req.seasonID,
                    LanguageCode = req.languageCode,
                    Image = string.Empty,
                    Translation = req.text
                };
                _repoWrapper.SeasonRepo.AddSeasonTranslation(seasonTranslation);
                response.message = "Translation has been added";
            }
            else
            {
                seasonTranslation.Translation = req.text;
                _repoWrapper.SeasonRepo.UpdateSeasonTranslation(seasonTranslation);
                response.message = "Translation has been updated";
            }
            await _repoWrapper.SaveAsync();
            return response;
        }
        #endregion


        private void ValidateSeason(AddSeasonRequest request)
        {
            if (string.IsNullOrEmpty(request.seasonName))
            {
                throw new AmazonFarmerException(_exceptions.seasonNotFound);
            }
            else if (request.toMonthID <= 0 || request.toMonthID > 12)
            {
                throw new AmazonFarmerException(_exceptions.daterangeIsNotValid);
            }
            else if (request.fromMonthID <= 0 || request.fromMonthID > 12)
            {
                throw new AmazonFarmerException(_exceptions.daterangeIsNotValid);
            }
        }
    }
}
