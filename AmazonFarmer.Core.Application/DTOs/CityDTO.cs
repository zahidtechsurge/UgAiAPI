using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for City
    /// </summary>
    public class CityDTO
    {
        public int districtID { get; set; } // Property for district ID
        public int cityID { get; set; } // Property for city ID
        public string cityName { get; set; } // Property for city name
        public string languageCode { get; set; } // Property for language code
    }

    /// <summary>
    /// Request DTO for retrieving cities
    /// </summary>
    public class getCity_Req
    {
        public string languageCode { get; set; } // Property for language code
    }
}
