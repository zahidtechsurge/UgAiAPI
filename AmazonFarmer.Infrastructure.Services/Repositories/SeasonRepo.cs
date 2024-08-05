/*
   This class implements the ISeasonRepo interface and provides methods for retrieving seasons from the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
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
                    .ThenInclude(x=>x.Months)
                        .ThenInclude(x=>x.MonthTranslations)
                .Where(x => x.LanguageCode == req.languageCode)
                .Select(x => new SeasonDTO
                {
                    seasonID = x.SeasonID,
                    seasonName = x.Translation,
                    filePath = x.Image
                }).ToListAsync();
        }
    }
}
