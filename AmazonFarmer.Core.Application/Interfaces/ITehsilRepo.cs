using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ITehsilRepo // Defining the interface for Tehsil repository
    {
        Task<List<TehsilDTO>> getTehsils(getTehsil_Req req); // Method signature for retrieving Tehsils
    }
}
