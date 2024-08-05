using System;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for language information.
    /// </summary>
    public class LanguageDTO
    {
        // The language code
        public string languageCode { get; set; } = string.Empty;

        // The name of the language
        public string language { get; set; } = string.Empty;
    }
    public class FarmerLanguageDTO
    {
        // The language code
        public string languageCode { get; set; } = string.Empty;

        // The name of the language
        public string languageName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for language code.
    /// </summary>
    public class LanguageReq
    {
        // The language code
        public string languageCode { get; set; } = string.Empty;
    }
    public class GetProductDTO_Internal_req
    {
        public string languageCode { get; set; } = "EN";
        public string basePath { get; set; } = string.Empty;
    }

    public class GetLanguageResponse_Admin
    {
        public string languageCode { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public int status { get; set; } = 0;
    }
    public class UpdateLanguageRequest_Admin
    {
        public required string languageCode { get; set; }
        public required string language { get; set; }
        public int status { get; set; } = 1;
    }

}
