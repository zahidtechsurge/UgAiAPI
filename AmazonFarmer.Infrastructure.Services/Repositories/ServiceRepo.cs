using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class ServiceRepo : IServiceRepo
    {
        private AmazonFarmerContext _context;

        public ServiceRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceDTO>> getServicesByLanguageID(LanguageReq req, int postDeliveryIn)
        {
            return await _context.ServiceTranslation
                .Include(x => x.Service)
                .Where(x => x.LanguageCode == req.languageCode)
                .Select(x => new ServiceDTO
                {
                    serviceID = x.ServiceID,
                    serviceName = x.Text,
                    filePath = x.Image,
                    postDeliveryIn = postDeliveryIn
                })
                .ToListAsync();
        }



    }
}
