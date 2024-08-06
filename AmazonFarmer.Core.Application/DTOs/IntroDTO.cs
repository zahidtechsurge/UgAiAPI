using System;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for introduction content.
    /// </summary>
    public class IntroDTO
    {
        // The file path of the introduction content
        public string filePath { get; set; }

        // The content of the introduction
        public string content { get; set; }
    }
    public class getIntroDTO : LanguageReq
    {
        public string basePath { get; set; } = string.Empty;
    }

    public class GetIntroAdminResponse
    {
        public string languageCode { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string intro { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty; // Property for file path of the banner image
        public int translationID { get; set; }
    }
    public class UpdateIntroAdminRequest : AddIntroAdminRequest
    {
        public int translationID { get; set; }
    }
    public class AddIntroAdminRequest
    {
        public string content { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
        public string intro { get; set; } = string.Empty;
        public string languageCode { get; set; } = string.Empty;

    }
}
