/*
   This class implements the ILoggingRepository interface and provides methods for adding and updating log entries in the database.
*/
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces; 
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly AmazonFarmerContext _context;

        // Constructor to initialize the LoggingRepository with an instance of the AmazonFarmerContext
        public LoggingRepository(AmazonFarmerContext context)
        {
            _context = context;
        }

        // Method to add a log entry to the database
        public async Task<WSDLLog> AddLogEntry(WSDLLog logEntry)
        {
            WSDLLog log = new WSDLLog();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    log = _context.WSDLLogs.Add(logEntry).Entity;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return log;
        }

        // Method to update a log entry in the database
        public async Task UpdateLogEntry(WSDLLog logEntry)
        {
            WSDLLog log = new WSDLLog();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.WSDLLogs.Update(logEntry);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // Method to add a notification log entry to the database
        public NotificationLog AddNoticationLog(NotificationLog logEntry)
        {
            return _context.NotificationLog.Add(logEntry).Entity;
        }

        // Method to update a notification log entry in the database
        public void UpdateNoticationLog(NotificationLog logEntry)
        {
            _context.NotificationLog.Update(logEntry);
        }
        public IQueryable<RequestLog> GetLogs()
        {
            return _context.RequestLogs.Include(x=>x.Responses);
        }
        public async Task<List<SP_LogEntryResult>> GetLogs(int pageNumber, int pageSize, string sortColumn, string sortOrder, string? searchTerm)
        {

            var sortOrderParam = new SqlParameter("@SortOrder", sortOrder ?? "DESC");
            var sortColumnParam = new SqlParameter("@SortColumn", sortColumn ?? "requestId");
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var SearchTerm = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);
            var sql = @"
            EXEC sp_GetLogs 
                @PageNumber, 
                @PageSize, 
                @SortColumn, 
                @SortOrder,
                @SearchTerm";
            //return lst;
            return await _context.SP_LogEntryResult.FromSqlRaw(sql, pageNumberParam, pageSizeParam, sortColumnParam, sortOrderParam, SearchTerm).ToListAsync();
        }
        public IQueryable<WSDLLog> GetWSDLLogs()
        {
            return _context.WSDLLogs;
        }
    }
}
