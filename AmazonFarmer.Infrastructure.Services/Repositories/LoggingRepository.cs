using AmazonFarmer.Core.Application.Interfaces;
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
    public class LoggingRepository : ILoggingRepository
    {
        private readonly AmazonFarmerContext _context;

        public LoggingRepository(AmazonFarmerContext context)
        {
            _context = context;
        }

        public WSDLLog AddLogEntry(WSDLLog logEntry)
        {
            return _context.WSDLLogs.Add(logEntry).Entity; 
        }

        public void UpdateLogEntry(WSDLLog logEntry)
        {
            _context.WSDLLogs.Update(logEntry); 
        }
    }
}
