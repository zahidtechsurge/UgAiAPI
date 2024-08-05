/*
   This class implements the ITehsilRepo interface and provides methods for retrieving tehsils from the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class TehsilRepo : ITehsilRepo
    {
        private AmazonFarmerContext _context;

        // Constructor to initialize the TehsilRepo with an instance of the AmazonFarmerContext
        public TehsilRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to retrieve tehsils based on language code and other criteria
        public async Task<List<TehsilDTO>> getTehsils(getTehsil_Req req)
        {
            return await _context.TehsilLanguages
                .Include(x => x.Tehsil)
                .ThenInclude(x => x.City)
                .Where(x => x.Tehsil.Status == Core.Domain.Entities.EActivityStatus.Active && x.LanguageCode == req.languageCode)
                .Select(x => new TehsilDTO
                {
                    cityID = x.Tehsil.City.ID,
                    tehsilID = x.Tehsil.ID,
                    languageCode = x.LanguageCode,
                    tehsilName = x.Translation
                }).ToListAsync();
        }
    }
}
