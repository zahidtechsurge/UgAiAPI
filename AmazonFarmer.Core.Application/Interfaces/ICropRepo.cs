using AmazonFarmer.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface ICropRepo
    {
        Task<List<Crops_Res>> getCropsBySeasonAndDistrictID(GetCropDTO_Internal_req req);
    }
}
