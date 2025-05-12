using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for Tehsil information.
    /// </summary>
    public class TehsilDTO
    {
        // The ID of the city associated with the tehsil
        public int cityID { get; set; }

        // The ID of the tehsil
        public int tehsilID { get; set; }

        // The name of the tehsil
        public string tehsilName { get; set; } = string.Empty;

        // The language code for localization
        public string languageCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO to get Tehsil information.
    /// </summary>
    public class getTehsil_Req
    {
        // The language code for localization
        public string languageCode { get; set; }
    }
    public class AddTehsilRequest
    {
        public int cityID { get; set; }
        public string tehsilName { get; set; } = string.Empty;
        public string tehsilCode { get; set; } = string.Empty;
    }
    public class UpdateTehsilRequest : AddTehsilRequest
    {
        public int tehsilID { get; set; }
        public int status { get; set; }
    }
    public class AddTehsilTranslationRequest
    {
        public int tehsilID { get; set; }
        public string languageCode { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }
    public class UpdateTehsilTranslationRequest : AddTehsilTranslationRequest
    {
        public int translationID { get; set; }
    }
    public class GetTehsilTranslationRequest : UpdateTehsilTranslationRequest
    {
        public string language { get; set; } = string.Empty;
    }
}
