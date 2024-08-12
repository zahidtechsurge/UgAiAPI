using System; // Importing necessary namespaces
using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// DTO for district details
    /// </summary>
    public class DistrictDTO
    {
        public int districtID { get; set; } // Property for district ID
        public string districtName { get; set; } // Property for district name
        public string languageCode { get; set; } // Property for language code
    }

    /// <summary>
    /// DTO for district, city, and tehsil details
    /// </summary>
    public class DistictCityTehsilDTO
    {
        public int DistrictId { get; set; } // Property for district ID
        public string DistrictName { get; set; } // Property for district name
        public int CityId { get; set; } // Property for city ID
        public string CityName { get; set; } // Property for city name
        public int TehsilId { get; set; } // Property for tehsil ID
        public string TehsilName { get; set; } // Property for tehsil name
    }

    /// <summary>
    /// Request DTO for retrieving districts
    /// </summary>
    public class getDistrict_Req
    {
        public string languageCode { get; set; } // Property for language code
    }
    public class GetDIstrict_AdminResponse
    {
        public int districtID { get; set; }
        public required string districtName { get; set; } // Property for district name
        public int regionID { get; set; } // Property for region name
        public required string regionName { get; set; } // Property for region name
    }
    public class GetPaginatedDIstrict_AdminResponse : GetDIstrict_AdminResponse
    {
        public required string districtCode { get; set; } // Property for district name
        public bool status { get; set; } = false;// Property for district name
        public List<GetDistrictTranslations> translations { get; set; } = new List<GetDistrictTranslations>();
    }
    public class GetDistrictTranslations
    {
        public int translationID { get; set; }
        public int districtID { get; set; }
        public required string languageCode { get; set; }
        public required string language { get; set; }
        public required string text { get; set; }
    }
    public class AddDistrictRequest
    {
        public required string districtName { get; set; }
        public required string districtCode { get; set; }
        public int regionID { get; set; }
    }
    public class UpdateDistrictRequest : AddDistrictRequest
    {
        public int districtID { get; set; }
        /// <summary>
        /// DeActive = 0,
        /// Active = 1,
        /// </summary>
        public int status { get; set; }
    }
    public class AddDistrictTranslationRequest
    {
        public int districtID { get; set; }
        public required string languageCode { get; set; }
        public required string text { get; set; }
    }
    public class UpdateDistrictTranslationRequest : AddDistrictTranslationRequest
    {
        public int translationID { get; set; }

    }
}
