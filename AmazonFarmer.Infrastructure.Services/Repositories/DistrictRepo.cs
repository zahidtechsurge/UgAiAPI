using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class DistrictRepo : IDistrictRepo
    {
        private AmazonFarmerContext _context;

        public DistrictRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<DistrictDTO>> getDistricts(getDistrict_Req req)
        {
            return await _context.DistrictLanguages
                .Include(x => x.District)
                .Include(x => x.Languages)
                .Where(x => x.District.Status == Core.Domain.Entities.EActivityStatus.Active && x.LanguageCode == req.languageCode)
                .Select(x => new DistrictDTO
                {
                    districtID = x.District.ID,
                    districtName = x.Translation,
                    languageCode = x.LanguageCode
                }).ToListAsync();
        }
        public async Task<DistictCityTehsilDTO?> GetDistricCityTehsilAsync(int districtId, int cityId, int tehsilId)
        {
            return await _context.District
                .Include(x => x.Cities.Where(c=>c.ID == cityId))
                .ThenInclude(x => x.Tehsils.Where(t => t.ID == tehsilId))
                .Where(d => d.ID == districtId)
                .Select(x => new DistictCityTehsilDTO
                {
                     CityId = cityId,
                     CityName = x.Cities.FirstOrDefault().Name,
                     DistrictId = districtId,
                     DistrictName = x.Name,
                     TehsilId = tehsilId,
                     TehsilName = x.Cities.FirstOrDefault().Tehsils.FirstOrDefault().Name
                }).FirstOrDefaultAsync();
        }
    }
}
