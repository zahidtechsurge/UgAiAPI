using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing districts in the database.
    /// </summary>
    public class DistrictRepo : IDistrictRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the DistrictRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public DistrictRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of districts based on the provided request.
        /// </summary>
        /// <param name="req">The request containing the language code.</param>
        /// <returns>A list of DistrictDTO objects.</returns>
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

        /// <summary>
        /// Retrieves district, city, and tehsil information based on provided IDs.
        /// </summary>
        /// <param name="districtId">The ID of the district.</param>
        /// <param name="cityId">The ID of the city.</param>
        /// <param name="tehsilId">The ID of the tehsil.</param>
        /// <returns>A DistictCityTehsilDTO object containing the information.</returns>
        public async Task<DistictCityTehsilDTO?> GetDistricCityTehsilAsync(int districtId, int cityId, int tehsilId)
        {
            return await _context.District
                .Include(x => x.Cities.Where(c => c.ID == cityId))
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
        public IQueryable<tblDistrict> getDistricts()
        {
            return _context.District.Include(x=>x.DistrictLanguages).Include(x=>x.Region);
        }
    }
}
