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
    public class CityRepo : ICityRepo
    {
        private AmazonFarmerContext _context;

        public CityRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<CityDTO>> getCities(getCity_Req req)
        {
            return await _context.CityLanguages
                .Include(x=>x.City)
                .ThenInclude(x=>x.District)
                .Include(x=>x.Language)
                .Where(x=>x.City.Status == Core.Domain.Entities.EActivityStatus.Active && x.LanguageCode == req.languageCode)
                .Select(x => new CityDTO
                {
                    districtID = x.City.DistrictID,
                    languageCode = x.LanguageCode,
                    cityID = x.City.ID,
                    cityName = x.Translation
                }).ToListAsync();
        }

    }
}
