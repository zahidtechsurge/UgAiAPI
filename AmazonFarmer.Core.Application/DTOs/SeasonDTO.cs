using System;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for Season information.
    /// </summary>
    public class SeasonDTO
    {
        // The ID of the season
        public int seasonID { get; set; }

        // The name of the season
        public string seasonName { get; set; }

        // The file path for the season
        public string filePath { get; set; }
        public List<getMonths> months { get; set; }
    }
    public class AddSeasonRequest
    {
        public string seasonName { get; set; } = string.Empty;
        public int fromMonthID { get; set; }
        public int toMonthID { get; set; }
    }
    public class UpdateSeasonRequest : AddSeasonRequest
    {
        public int seasonID { get; set; }
        public int status { get; set; }
    }
    public class SeasonResponse : UpdateSeasonRequest
    {
        public string fromMonth { get; set; } = string.Empty;
        public string toMonth { get; set; } = string.Empty;
    }

    public class SyncSeasonTranslationDTO
    {
        public int seasonID { get; set; }
        public string text { get; set; } = string.Empty;
        public string languageCode { get; set; } = string.Empty;
    }
    public class SyncSeasonTranslationResponse : SyncSeasonTranslationDTO
    {
        public int translationID { get; set; }
        public string language { get; set; } = string.Empty;
        public string season { get; set; } = string.Empty;

    }

}
