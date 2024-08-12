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
        Task<List<tblDistrictLanguages>> GetDistrictLanguagesByID(int districtId);
    }
}
