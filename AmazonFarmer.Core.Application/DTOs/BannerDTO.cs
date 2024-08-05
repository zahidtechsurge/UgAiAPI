using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Banner
    /// </summary>
    public class BannerDTO
    {
        public string bannerName { get; set; } // Property for banner name
        public string filePath { get; set; } // Property for file path of the banner image
    }
}
