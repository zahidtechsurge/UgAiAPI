using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ICropRepo // Defining the interface for handling crops
    {
        Task<List<Crops_Res>> getCropsBySeasonAndDistrictID(GetCropDTO_Internal_req req); // Method signature for getting crops by season and district ID
    }
}
