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
    public class AddCityRequest
    {
        public int districtID { get; set; }
        public required string cityName { get; set; }
        public string cityCode { get; set; }
    }
    public class UpdateCityRequest : AddCityRequest
    {
        public int cityID { get; set; }
        public int status { get; set; }
    }
    public class AddCityTranslationRequest
    {
        public int cityID { get; set; }
        public required string languageCode { get; set; }
        public required string text { get; set; }
    }
    public class UpdateCityTranslationRequest : AddCityTranslationRequest
    {
        public int translationID { get; set; }
    }
    public class GetCityTranslations : UpdateCityTranslationRequest
    {
        public required string language { get; set; }
    }

}
