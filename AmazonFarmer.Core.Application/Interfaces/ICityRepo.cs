using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ICityRepo // Defining the interface for handling cities
    {
        IQueryable<tblCity> GetCities();
        Task<tblCity?> GetCityByID(int cityID);
        Task<tblCity?> GetCityByID(string cityName, string cityCode);
        void UpdateCity(tblCity city);
        void AddCity(tblCity city);
        Task<List<tblCityLanguages>> GetCityLanguagesByCityID(int CityID);
        Task<tblCityLanguages?> GetCityLanguageByID(int transID);
        Task<tblCityLanguages?> GetCityLanguageByID(int cityID, string languageCode);
        void UpdateCityLanguage(tblCityLanguages city);
        void AddCityLanguage(tblCityLanguages city);

        Task<List<CityDTO>> getCities(getCity_Req req); // Method signature for getting cities based on request
        Task<string> getCityNameByCityIDandLanguageCode(int cityID, string languageCode); // Method signature for getting city Name based on cityID and LanguageCode
    }
}
