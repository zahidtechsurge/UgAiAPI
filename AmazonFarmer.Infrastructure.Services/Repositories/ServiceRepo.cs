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
                .Where(x => x.LanguageCode == req.languageCode && x.Service.Active == EActivityStatus.Active)
                .Select(x => new ServiceDTO
                {
                    serviceID = x.ServiceID,
                    serviceName = x.Text,
                    filePath = string.Concat(req.basePath, x.Image.TrimStart('/').Replace("/", "%2F").Replace(" ", "%20").Replace("\\", "%2F")),
                    postDeliveryIn = postDeliveryIn
                })
                .ToListAsync();
        }

        public async Task<List<tblService>> getServicesByIDs(List<int> serviceIDs, string languageCode)
        {
            return await _context.Service
                .Include(x => x.ServiceTranslations.Where(x => x.LanguageCode == languageCode))
                .Where(x => serviceIDs.Contains(x.ID) && x.Active == EActivityStatus.Active)
                .ToListAsync();
        }
        public IQueryable<tblService> GetService()
        {
            return _context.Service;
        }
        public async Task<tblService?> GetServiceByID(int id)
        {
            return await _context.Service.Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public async Task<tblService?> GetServiceByID(string name, string code)
        {
            return await _context.Service.Where(x => x.Name == name || x.Code == code).FirstOrDefaultAsync();
        }
        public void AddService(tblService service)
        {
            _context.Service.Add(service);
        }
        public void UpdateService(tblService service)
        {
            _context.Service.Update(service);
        }
        public async Task<List<tblServiceTranslation>> GetServiceTranslationByServiceID(int ID)
        {
            return await _context.ServiceTranslation.Include(x => x.Language).Where(x => x.ServiceID == ID).ToListAsync();
        }
        public async Task<tblServiceTranslation?> GetServiceTranslationByID(int ID)
        {
            return await _context.ServiceTranslation.Include(x => x.Language).Where(x => x.ID == ID).FirstOrDefaultAsync();
        }
        public async Task<tblServiceTranslation?> GetServiceTranslationByID(int ServiceID, string LanguageCode)
        {
            return await _context.ServiceTranslation.Include(x => x.Language).Where(x => x.ServiceID == ServiceID && x.LanguageCode == LanguageCode).FirstOrDefaultAsync();
        }
        public async Task<tblServiceTranslation?> GetServiceTranslationByID(string name, string code)
        {
            return await _context.ServiceTranslation.Where(x => x.Text == name || x.LanguageCode == code).FirstOrDefaultAsync();
        }
        public void AddServiceTranslation(tblServiceTranslation serviceTranslation)
        {
            _context.ServiceTranslation.Add(serviceTranslation);
        }
        public void UpdateServiceTranslation(tblServiceTranslation serviceTranslation)
        {
            _context.ServiceTranslation.Update(serviceTranslation);
        }
    }
}
