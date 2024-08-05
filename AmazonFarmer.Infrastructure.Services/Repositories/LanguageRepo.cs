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
    public class LanguageRepo : ILanguageRepo
    {
        private AmazonFarmerContext _context;

        public LanguageRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
        public async Task<List<LanguageDTO>> GetLanguages()
        {
            return await _context.Languages.Where(x => x.Status == Core.Domain.Entities.EActivityStatus.Active).Select(x => new LanguageDTO
            {
                languageCode = x.LanguageCode,
                languageName = x.LanguageName
            }).ToListAsync();
        }
    }
}
