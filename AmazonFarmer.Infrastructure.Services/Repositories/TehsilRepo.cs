/*
   This class implements the ITehsilRepo interface and provides methods for retrieving tehsils from the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
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
        public IQueryable<tblTehsil> GetTehsils()
        {
            return _context.Tehsils;
        }
        public async Task<tblTehsil?> GetTehsilByID(int id)
        {
            return await _context.Tehsils.Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public async Task<tblTehsil?> GetTehsilByID(string name, string code)
        {
            return await _context.Tehsils.Where(x => x.Name == name || x.TehsilCode == code).FirstOrDefaultAsync();
        }
        public void AddTehsil(tblTehsil tehsil)
        {
            _context.Tehsils.Add(tehsil);
        }
        public void UpdateTehsil(tblTehsil tehsil)
        {
            _context.Tehsils.Update(tehsil);
        }
        public async Task<List<tblTehsilLanguages>> GetTehsilLanguagesByTehsilID(int id)
        {
            return await _context.TehsilLanguages.Include(x => x.Language).Where(x => x.TehsilID == id).ToListAsync();
        }
        public async Task<tblTehsilLanguages?> GetTehsilLanguageByID(int id)
        {
            return await _context.TehsilLanguages.Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public async Task<tblTehsilLanguages?> GetTehsilLanguageByID(int tehsilID, string code)
        {
            return await _context.TehsilLanguages.Where(x => x.TehsilID == tehsilID && x.LanguageCode == code).FirstOrDefaultAsync();
        }
        public void AddTehsilTrasnaltion(tblTehsilLanguages tehsilLanguages)
        {
            _context.TehsilLanguages.Add(tehsilLanguages);
        }
        public void UpdateTehsilTranslation(tblTehsilLanguages tehsilLanguages)
        {
            _context.TehsilLanguages.Update(tehsilLanguages);
        }
    }
}
