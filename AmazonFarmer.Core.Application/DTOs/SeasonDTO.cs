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
}
