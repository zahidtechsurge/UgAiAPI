using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ICityRepo // Defining the interface for handling cities
    {
        Task<List<CityDTO>> getCities(getCity_Req req); // Method signature for getting cities based on request
        Task<string> getCityNameByCityIDandLanguageCode(int cityID, string languageCode); // Method signature for getting city Name based on cityID and LanguageCode
    }
}
