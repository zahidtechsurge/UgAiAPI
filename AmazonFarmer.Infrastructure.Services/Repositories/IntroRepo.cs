using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
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
                .Where(x => x.LanguageCode == req.languageCode /*&& x.Status == EActivityStatus.Active*/)
                .Select(x => new IntroDTO
                {
                    filePath = string.Concat(req.basePath, x.Image.Replace("/", "%2F").Replace(" ", "%20")),
                    content = x.Text
                })
                .ToListAsync();
        }
        public IQueryable<tblIntroLanguages> getIntroLanguageQueryable()
        {
            return _context.IntroLanguages.Include(x => x.Intro).Include(x=>x.Language);
        }
        public void addIntroLanguage(tblIntroLanguages req)
        {
            _context.IntroLanguages.Add(req);
        }
        public void updateIntroLanguage(tblIntroLanguages req)
        {
            _context.IntroLanguages.Update(req);
        }
        public async Task<tblIntroLanguages?> GetIntroLanguagesByID(int Id)
        {
            return await _context.IntroLanguages.Where(x=>x.ID ==  Id).FirstOrDefaultAsync();
        }
        public void addIntro(tblIntro req)
        {
            _context.Intros.Add(req);
        }
    }
}
