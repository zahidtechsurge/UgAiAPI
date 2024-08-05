using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IServiceRepo // Defining the interface for service repository
    {
        Task<List<ServiceDTO>> getServicesByLanguageID(getServicesRequestDTO req, int postDeliveryIn); // Method signature for retrieving services by language ID
        Task<List<tblService>> getServicesByIDs(List<int> serviceIDs, string languageCode); // Method signature for retrieving services by service IDs
    }
}
