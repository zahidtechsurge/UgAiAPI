using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IDistrictRepo
    {
        Task<List<DistrictDTO>> getDistricts(getDistrict_Req req);
        Task<DistictCityTehsilDTO?> GetDistricCityTehsilAsync(int districtId, int cityId, int tehsilId);
    }
}
