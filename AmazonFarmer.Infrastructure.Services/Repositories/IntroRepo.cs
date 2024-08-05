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
    public class IntroRepo : IIntroRepo
    {
        private AmazonFarmerContext _context;

        public IntroRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<IntroDTO>> getIntros(LanguageReq req)
        {
            return await _context.IntroLanguages.Where(x=>x.LanguageCode == req.languageCode).Select(x => new IntroDTO
            {
                filePath = x.Image,
                content = x.Text
            }).ToListAsync();
        } 
    }
}
