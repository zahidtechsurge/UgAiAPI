using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IBannerRepo
    {
        Task<List<BannerDTO>> getBanners(LanguageReq req);
    }
}
