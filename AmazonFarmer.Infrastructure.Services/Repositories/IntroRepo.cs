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
            return _context.IntroLanguages.Include(x => x.Intro).Include(x => x.Language);
        }
        public IQueryable<tblIntro> getIntroQueryable()
        {
            return _context.Intros.Include(x => x.IntroLanguages);
        }
        public void addIntroLanguage(tblIntroLanguages req) //---
        {
            _context.IntroLanguages.Add(req);
        }
        public void updateLanguage(tblIntroLanguages existingIntroLanguage)//----
        {
            //  _context.Set<tblIntroLanguages>().Update(existingIntroLanguage);
            _context.IntroLanguages.Update(existingIntroLanguage);
        }
        public void updateIntroLanguage(tblIntroLanguages req)
        {
            _context.IntroLanguages.Update(req);
        }
        public async Task<tblIntroLanguages?> GetIntroLanguagesByID(int Id)
        {
            return await _context.IntroLanguages.Where(x => x.ID == Id).FirstOrDefaultAsync();
        }
        public async Task<tblIntroLanguages?> GetIntroLanguageByIntroIDAndLanguage(int introID, string languageCode)
        {
            return await _context.Set<tblIntroLanguages>()
                .FirstOrDefaultAsync(il => il.IntroID == introID && il.LanguageCode == languageCode);
        }


        //public void addIntro(tblIntro req)
        //{
        //    _context.IntroLanguages.Add(req);
        //}

        public async Task<tblIntro?> GetIntroByName(string name)
        {
            return await _context.Set<tblIntro>()
       .FirstOrDefaultAsync(i => i.Name == name);
            // return await _context.Intros.Where(x => x.Name == name).FirstOrDefaultAsync();
        }
        public void AddIntro(tblIntro intro)
        {
            _context.Set<tblIntro>().Add(intro);
            // _context.intros.Add(intro);
        }

        //public async Task<tblIntro?> GetIntroByNameAndStatus(int id)
        //{
        //    return await _context.Intros
        //        .FirstOrDefaultAsync(i => i.Name == name && i.Status == status);
        //}
        public async Task<tblIntro?> GetIntroByNameAndStatus(int ID)
        {
            return await _context.Intros.Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public void UpdateIntro(tblIntro intro)
        {
            _context.Intros.Update(intro);
        }

    }
}
