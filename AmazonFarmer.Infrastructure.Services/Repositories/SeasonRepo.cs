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

        public SeasonRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<SeasonDTO>> getSeasonsByLanguageCode(LanguageReq req)
        {
            return await _context.SeasonTranslations
                .Include(x => x.Season)
                .Where(x=>x.LanguageCode == req.languageCode)
                .Select(x => new SeasonDTO
                {
                    seasonID = x.SeasonID,
                    seasonName = x.Translation,
                    filePath = x.Image
                }).ToListAsync();
        }
    }
}
