using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ILocationRepo // Defining the interface for handling locations
    {
        Task<List<LocationDTO>> getWarehouseLocationsByLanguageCode(string languageCode); // Method signature for getting warehouse locations by language code
    }
}
