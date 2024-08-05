using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class LanguageRepo : ILanguageRepo
    {
        private readonly AmazonFarmerContext _context;

        public LanguageRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves active languages.
        /// </summary>
        /// <returns>A list of active languages.</returns>
        public async Task<List<FarmerLanguageDTO>> GetLanguages()
        {
            return await _context.Languages
                .Where(x => x.Status == Core.Domain.Entities.EActivityStatus.Active)
                .Select(x => new FarmerLanguageDTO
                {
                    languageCode = x.LanguageCode,
                    languageName = x.LanguageName
                })
                .ToListAsync();
        }

        public IQueryable<tblLanguages> getLangauges()
        {
            return _context.Languages;
        }
        public async Task<tblLanguages> getLanguageByCodeOrName(string Code, string Name)
        {
            return await _context.Languages.Where(x => x.LanguageCode == Code || x.LanguageName == Name).FirstOrDefaultAsync();
        }
        public void addLanguage(tblLanguages Languages)
        {
            _context.Languages.Add(Languages);
        }
        public void updateLanguage(tblLanguages Languages)
        {
            _context.Languages.Update(Languages);
        }
    }
}
