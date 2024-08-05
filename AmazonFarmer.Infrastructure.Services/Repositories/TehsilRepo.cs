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
    public class TehsilRepo : ITehsilRepo
    {
        private AmazonFarmerContext _context;

        public TehsilRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<TehsilDTO>> getTehsils(getTehsil_Req req)
        {
            return await _context.TehsilLanguages
                .Include(x=>x.Tehsil)
                .ThenInclude(x=>x.City)
                .Where(x=>x.Tehsil.Status == Core.Domain.Entities.EActivityStatus.Active && x.LanguageCode == req.languageCode)
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
