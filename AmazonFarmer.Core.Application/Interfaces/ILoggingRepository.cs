using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface ILoggingRepository // Defining the interface for logging repository
    {
        Task<WSDLLog> AddLogEntry(WSDLLog logEntry); // Method signature for adding a log entry
        Task UpdateLogEntry(WSDLLog logEntry); // Method signature for updating a log entry

        NotificationLog AddNoticationLog(NotificationLog logEntry);
        void UpdateNoticationLog(NotificationLog logEntry);
        IQueryable<RequestLog> GetLogs();
        Task<List<SP_LogEntryResult>> GetLogs(int pageNumber, int pageSize, string sortColumn, string sortOrder, string? searchTerm);
        IQueryable<WSDLLog> GetWSDLLogs();
    }
}
