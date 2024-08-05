/*
   This class implements the IBannerRepo interface and provides methods for interacting with banner data.

   It requires access to the AmazonFarmerContext, which is a DbContext for database operations.

   The getBanners method fetches banners based on the provided language code asynchronously.

   It retrieves banner information from the BannerLanguages table, includes related Banner entities, and filters based on the language code.
   Then, it maps the retrieved data to BannerDTO objects.

   Namespace imports:
   - AmazonFarmer.Core.Application.DTOs: Contains data transfer objects related to banners.
   - AmazonFarmer.Core.Application.Interfaces: Defines interfaces for repository services.
   - AmazonFarmer.Infrastructure.Persistence: Contains the AmazonFarmerContext class, which is the DbContext for database operations.
   - Microsoft.EntityFrameworkCore: Provides classes and methods for Entity Framework Core.
   - System: Contains fundamental classes and base types.
   - System.Collections.Generic: Provides interfaces and classes for working with collections of objects.
   - System.Linq: Provides classes and methods for querying data sources.
   - System.Text: Provides types and methods for working with text.

*/

using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class BannerRepo : IBannerRepo
    {
        private AmazonFarmerContext _context;

        // Constructor injection of AmazonFarmerContext.
        public BannerRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to fetch banners based on language code.
        public async Task<List<BannerDTO>> getBanners(LanguageReq req)
        {
            // Retrieve banners asynchronously based on language code.
            return await _context.BannerLanguages.Include(x => x.Banner)
                .Where(x => x.LanguageCode == req.languageCode && x.Languages.Status == EActivityStatus.Active)
                .Select(x => new BannerDTO
                {
                    bannerName = x.Banner.Name,
                    filePath = x.Image
                }).ToListAsync();
        }

        // Method to fetch banners.
        public async Task<List<tblBanner>> getBanners()
        {
            return await _context.Banners
                .Include(x => x.BannerLanguages)
                .Where(x => x.Status == EActivityStatus.Active)
                .ToListAsync();
        }
    }
}
