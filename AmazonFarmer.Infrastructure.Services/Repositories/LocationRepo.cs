using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class LocationRepo : ILocationRepo
    {
        private readonly AmazonFarmerContext _context;

        public LocationRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves warehouse locations by language code.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>A list of warehouse locations.</returns>
        public async Task<List<LocationDTO>> getWarehouseLocationsByLanguageCode(string languageCode)
        {
            return await _context.WarehouseTranslation
                .Include(x => x.Warehouse)
                .Where(x => x.LanguageCode == languageCode && x.Warehouse.Status == Core.Domain.Entities.EActivityStatus.Active)
                .Select(x => new LocationDTO
                {
                    locationID = x.WarehouseID,
                    locationName = x.Text,
                    latitude = x.Warehouse.latitude,
                    longitude = x.Warehouse.longitude
                })
                .ToListAsync();
        }
    }
}
