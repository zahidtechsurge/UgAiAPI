using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IDistrictRepo // Defining the interface for handling districts
    {
        Task<List<DistrictDTO>> getDistricts(getDistrict_Req req); // Method signature for getting districts
        Task<DistictCityTehsilDTO?> GetDistricCityTehsilAsync(int districtId, int cityId, int tehsilId); // Method signature for getting district, city, and tehsil
        IQueryable<tblDistrict> getDistricts();
        Task<List<tblDistrict>> GetDistricts();
        Task<List<tblDistrictLanguages>> GetDistrictLanguagesByID(int districtId);
        Task<tblDistrict?> GetDistrictByID(int districtId);
        Task<tblDistrict?> GetDistrictByID(string districtName, string districtCode);
        void AddDistrict(tblDistrict district);
        void UpdateDistrict(tblDistrict district);
        Task<tblDistrictLanguages?> GetDistrictLanguageByID(int Id);
        Task<tblDistrictLanguages?> GetDistrictLanguageByID(int Id, string LanguageCode);
        void AddDistrictLanguages(tblDistrictLanguages district);
        void UpdateDistrictLanguages(tblDistrictLanguages district);

    }
}
