using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class IntroRepo : IIntroRepo
    {
        private readonly AmazonFarmerContext _context;

        public IntroRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves intros based on the specified language.
        /// </summary>
        /// <param name="req">The language request.</param>
        /// <returns>A list of intros.</returns>
        public async Task<List<IntroDTO>> getIntros(getIntroDTO req)
        {
            return await _context.IntroLanguages
                .Where(x => x.LanguageCode == req.languageCode)
                .Select(x => new IntroDTO
                {
                    filePath = string.Concat(req.basePath, x.Image),
                    content = x.Text
                })
                .ToListAsync();
        }
    }
}
