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
    public class MonthRepo : IMonthRepo
    {
        private AmazonFarmerContext _context;

        // Constructor to initialize the MonthRepo with an instance of the AmazonFarmerContext
        public MonthRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
        public async Task<List<getMonths>> getMonthsByLanguageCode(string languageCode)
        {
            return await _context.MonthTranslations
                .Include(x => x.Month)
                .Where(x =>
                    x.LanguageCode == languageCode &&
                    x.Month.Status == EActivityStatus.Active
                ).Select(x => new getMonths
                {
                    monthID = x.MonthID,
                    month = x.Text
                }).ToListAsync();
        }
        public async Task<List<getMonths>> getMonthsByLanguageCodeAndSeasonID(string languageCode, int seasonID)
        {
            return await _context.MonthTranslations
                .Include(x => x.Month)
                .OrderBy(x=>x.Month.orderBy)
                .Where(x =>
                    x.LanguageCode == languageCode &&
                    x.Month.SeasonID == seasonID &&
                    x.Month.Status == EActivityStatus.Active
                ).Select(x => new getMonths
                {
                    monthID = x.MonthID,
                    month = x.Text
                }).ToListAsync();

        }
    }
}
