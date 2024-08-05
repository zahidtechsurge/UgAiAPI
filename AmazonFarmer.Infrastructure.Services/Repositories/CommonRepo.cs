using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class CommonRepo : ICommonRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the AttachmentRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public CommonRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
        public async Task<string> GetConfigurationValueByConfigType(EConfigType type)
        {
            return await _context.ConfigurationTable.Where(c => c.Type == type).Select(c => c.Value).FirstAsync();
        }

        public async Task<List<tblConfig>> GetConfigurationValueByConfigType(List<EConfigType> types)
        {
            return await _context.ConfigurationTable.Where(c => types.Contains(c.Type)).ToListAsync();
        }
    }
}
