using AmazonFarmer.Core.Domain.Entities;
using System; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Banner
    /// </summary>
    public class BannerDTO
    {
        public string bannerName { get; set; } = string.Empty; // Property for banner name
        public string filePath { get; set; } = string.Empty; // Property for file path of the banner image
    }
    public class GetBannerAdminRequest : pagination_Req
    {
        public string? languageCode { get; set; } = "EN";
    }
    public class GetBannerAdminResponse
    {
        public string languageCode { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string banner { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty; // Property for file path of the banner image
        public int translationID { get; set; }
    }
    public class AddBanner
    {
        public string languageCode { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public EBannerType bannerType { get; set; } = EBannerType.loginScreen;
    }
    public class UpdateBanner : AddBanner
    {
        public int translationID { get; set; }
    }
}
