using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class DistrictDTO
    {
        public int districtID { get; set; }
        public string districtName { get; set; }
        public string languageCode { get; set; }
    }
    public class DistictCityTehsilDTO
    {
        public int DistrictId{ get; set; }
        public string DistrictName{ get; set; }
        public int CityId{ get; set; }
        public string CityName { get; set; } 
        public int TehsilId{ get; set; }
        public string TehsilName { get; set; } 
    }
    public class getDistrict_Req
    {
        public string languageCode { get; set; }
    }
}
