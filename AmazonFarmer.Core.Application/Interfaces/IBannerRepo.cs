using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IBannerRepo // Defining the interface for handling banners
    {
        Task<List<BannerDTO>> getBanners(LanguageReq req); // Method signature for getting banners based on language request
        Task<List<tblBanner>> getBanners(); // Method signature for getting active banners data from table directly
    }
}
