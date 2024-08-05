/*
   This class implements the IServiceRepo interface and provides methods for retrieving services from the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class ServiceRepo : IServiceRepo
    {
        private AmazonFarmerContext _context;

        // Constructor to initialize the ServiceRepo with an instance of the AmazonFarmerContext
        public ServiceRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to retrieve services based on language ID and post-delivery time
        public async Task<List<ServiceDTO>> getServicesByLanguageID(getServicesRequestDTO req, int postDeliveryIn)
        {
            return await _context.ServiceTranslation
                .Include(x => x.Service)
                .Where(x => x.LanguageCode == req.languageCode)
                .Select(x => new ServiceDTO
                {
                    serviceID = x.ServiceID,
                    serviceName = x.Text,
                    filePath = string.Concat(req.basePath, x.Image),
                    postDeliveryIn = postDeliveryIn
                })
                .ToListAsync();
        }

        public async Task<List<tblService>> getServicesByIDs(List<int> serviceIDs, string languageCode)
        {
            return await _context.Service
                .Include(x => x.ServiceTranslations.Where(x => x.LanguageCode == languageCode))
                .Where(x => serviceIDs.Contains(x.ID))
                .ToListAsync();
        }
    }
}
