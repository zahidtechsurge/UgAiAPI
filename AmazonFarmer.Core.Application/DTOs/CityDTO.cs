using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class CityDTO
    {
        public int districtID { get; set; }
        public int cityID { get; set; }
        public string cityName { get; set; }
        public string languageCode { get; set; }
    }
    public class getCity_Req
    {
        public string languageCode { get; set; }
    }
}
