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
}
