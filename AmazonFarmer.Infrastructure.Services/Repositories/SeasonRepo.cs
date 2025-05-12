/*
   This class implements the ISeasonRepo interface and provides methods for retrieving seasons from the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class SeasonRepo : ISeasonRepo
    {
        private AmazonFarmerContext _context;

        // Constructor to initialize the SeasonRepo with an instance of the AmazonFarmerContext
        public SeasonRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to retrieve seasons based on language code
        public async Task<List<SeasonDTO>> getSeasonsByLanguageCode(LanguageReq req)
        {
            return await _context.SeasonTranslations
                .Include(x => x.Season)
                    .ThenInclude(x => x.Months)
                        .ThenInclude(x => x.MonthTranslations)
                .Where(x => x.LanguageCode == req.languageCode && x.Season.Status == EActivityStatus.Active)
                .Select(x => new SeasonDTO
                {
                    seasonID = x.SeasonID,
                    seasonName = x.Translation,
                    filePath = x.Image
                }).ToListAsync();
        }
        public async Task<List<tblSeason>> getSeasons()
        {
            return await _context.Season.Include(x => x.SeasonTranslations).ToListAsync();
        }

        public async Task<tblSeason?> GetSeasonByID(int ID)
        {
            return await _context.Season.Where(s => s.ID == ID).FirstOrDefaultAsync();
        }
        public async Task<bool> SeasonExistInDateRange(int fromMonth, int toMonth)
        {
            int season = await _context.Season.Where(x => x.StartDate.Month >= fromMonth && x.EndDate.Month <= toMonth).CountAsync();
            return season > 0 ? true : false;
        }
        public async Task<tblSeason?> GetSeasonByName(string name)
        {
            return await _context.Season.Where(s => s.Name == name).FirstOrDefaultAsync();
        }
        public void AddSeason(tblSeason season)
        {
            _context.Season.Add(season);
        }
        public void UpdateSeason(tblSeason season)
        {
            _context.Season.Update(season);
        }

        public async Task<List<tblSeasonTranslation>> GetSeasonTranslationBySeasonID(int seasonID)
        {
            return await _context.SeasonTranslations
                .Include(x => x.Season)
                .Include(x => x.Language)
                .Where(s => s.SeasonID == seasonID)
                .ToListAsync();
        }
        public async Task<tblSeasonTranslation?> GetSeasonTranslationBySeasonID(int seasonID, string languageCode)
        {
            return await _context.SeasonTranslations
                .Where(x => x.SeasonID == seasonID && x.LanguageCode == languageCode)
                .FirstOrDefaultAsync();
        }
        public void AddSeasonTranslation(tblSeasonTranslation season)
        {
            _context.SeasonTranslations.Add(season);
        }
        public void UpdateSeasonTranslation(tblSeasonTranslation season)
        {
            _context.SeasonTranslations.Update(season);
        }
    }
}
