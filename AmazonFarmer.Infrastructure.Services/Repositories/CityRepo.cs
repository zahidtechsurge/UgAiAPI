﻿using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing cities in the database.
    /// </summary>
    public class CityRepo : ICityRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the CityRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public CityRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public IQueryable<tblCity> GetCities()
        {
            return _context.Cities;
        }
        public async Task<tblCity?> GetCityByID(int cityID)
        {
            return await _context.Cities.Where(x => x.ID == cityID).FirstOrDefaultAsync();
        }
        public async Task<tblCity?> GetCityByID(string cityName, string cityCode)
        {
            return await _context.Cities.Where(x=>x.Name == cityCode || x.CityCode == cityCode).FirstOrDefaultAsync();
        }
        public void UpdateCity(tblCity city)
        {
            _context.Cities.Update(city);
        }
        public void AddCity(tblCity city)
        {
            _context.Cities.Add(city);
        }
        public async Task<List<tblCityLanguages>> GetCityLanguagesByCityID(int CityID)
        {
            return await _context.CityLanguages.Include(x=>x.Language).Where(x=>x.CityID == CityID).ToListAsync();
        }
        public async Task<tblCityLanguages?> GetCityLanguageByID(int transID)
        {
            return await _context.CityLanguages.Where(x=>x.ID == transID).FirstOrDefaultAsync();
        }
        public async Task<tblCityLanguages?> GetCityLanguageByID(int cityID, string languageCode)
        {
            return await _context.CityLanguages.Where(x=>x.CityID == cityID && x.LanguageCode == languageCode).FirstOrDefaultAsync();
        }
        public void UpdateCityLanguage(tblCityLanguages city)
        {
            _context.CityLanguages.Update(city);
        }
        public void AddCityLanguage(tblCityLanguages city)
        {
            _context.CityLanguages.Add(city);
        }

        /// <summary>
        /// Retrieves a list of cities based on the language code.
        /// </summary>
        /// <param name="req">The request containing the language code.</param>
        /// <returns>A list of CityDTOs.</returns>
        public async Task<List<CityDTO>> getCities(getCity_Req req)
        {
            return await _context.CityLanguages
                .Include(x => x.City)
                .ThenInclude(x => x.District)
                .Include(x => x.Language)
                .Where(x => x.City.Status == Core.Domain.Entities.EActivityStatus.Active && x.LanguageCode == req.languageCode)
                .Select(x => new CityDTO
                {
                    districtID = x.City.DistrictID,
                    languageCode = x.LanguageCode,
                    cityID = x.City.ID,
                    cityName = x.Translation
                }).ToListAsync();
        }

        public async Task<string> getCityNameByCityIDandLanguageCode(int cityID, string languageCode)
        {
            return await _context.CityLanguages.Where(x => x.CityID == cityID && x.LanguageCode == languageCode).Select(x=>x.Translation).FirstOrDefaultAsync();
        }

    }
}
