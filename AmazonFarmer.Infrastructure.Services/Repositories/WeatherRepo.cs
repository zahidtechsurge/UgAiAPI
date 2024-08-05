using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing attachments in the database.
    /// </summary>
    public class WeatherRepo : IWeatherRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the WeatherRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public WeatherRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves weather information by weather type asynchronously.
        /// </summary>
        /// <param name="weatherType">The type of weather to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. 
        /// The task result contains the weather icon associated with the specified weather type.
        /// </returns>
        public async Task<tblWeatherIcon> getWeatherByWeatherType(long weatherType)
        {
            // Retrieve the weather icon entity including its translations
            return await _context.WeatherIcon
                .Include(x => x.WeatherIconTranslations)
                // Filter by weather type and ensure the weather icon is active
                .Where(x => x.WeatherType == (EWeatherType)weatherType && x.Status == EActivityStatus.Active)
                // Retrieve the first matching weather icon asynchronously
                .FirstOrDefaultAsync();
        }

    }
}
