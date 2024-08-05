using AmazonFarmer.Core.Application.DTOs;
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
    public class PaymentAcknowledgmentFileRepo : IPaymentAcknowledgmentFileRepo
    {
        private readonly AmazonFarmerContext _context;

        public PaymentAcknowledgmentFileRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public PagedResult<PaymentAcknowledgmentFile> GetPaginatedFiles(PaymentAcknowledgmentFileRequest req)
        {
            var query = _context.PaymentAcknowledgmentFiles.Include(x=>x.PaymentAcknowledgments).AsQueryable();

            // Sort so that Processed files are at the end
            query = query.OrderBy(f => f.Status == EPaymentAcknowledgmentFileStatus.Processed)
                         .ThenBy(f => f.DateReceived);

            if (req.Status != -1)
            {
                query = query.Where(f => f.Status == (EPaymentAcknowledgmentFileStatus)req.Status);
            }
            if (!string.IsNullOrEmpty(req.StartDate))
            {
                query = query.Where(f => f.DateReceived >= DateTime.Parse(req.StartDate));
            }
            if (!string.IsNullOrEmpty(req.EndDate))
            {
                query = query.Where(f => f.DateReceived <= DateTime.Parse(req.EndDate));
            }

            var totalCount = query.Count();

            var items = query.Skip(req.PageNumber * req.PageSize)
                             .Take(req.PageSize)
                             .ToList();

            return new PagedResult<PaymentAcknowledgmentFile>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public PaymentAcknowledgmentFile GetFileById(int id)
        {
            return _context.PaymentAcknowledgmentFiles
                .Include(f => f.PaymentAcknowledgments)
                .FirstOrDefault(f => f.Id == id);
        }

        public async Task<PaymentAcknowledgmentFile> GetPaymentAcknowledgmentFileByPaymentAcknowledgementID(int PKey)
        {
            return await _context.PaymentAcknowledgmentFiles
                .Include(x => x.PaymentAcknowledgments)
                .Where(x => x.PaymentAcknowledgments.Any(p => p.Id == PKey))
                .FirstOrDefaultAsync();
        }

        public void UpdatepaymentAcknowledgmentFile(PaymentAcknowledgmentFile req)
        {
            _context.PaymentAcknowledgmentFiles.Update(req);
        }
    }
}
