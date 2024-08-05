using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class PaymentAcknowledgmentRepo : IPaymentAcknowledgmentRepo
    {
        private readonly AmazonFarmerContext _context;

        public PaymentAcknowledgmentRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public PagedResult<PaymentAcknowledgment> GetAcknowledgmentsByFileId(PaymentAcknowledgmentDetailRequest req)
        {
            var query = _context.PaymentAcknowledgments
                .Where(p => p.FileID == req.Id);

            if (req.Status != -1)
            {
                query = query.Where(f => f.Status == (EPaymentAcknowledgmentStatus)req.Status);
            }
            var totalCount = query.Count();

            var items = query.Skip(req.PageNumber * req.PageSize)
                             .Take(req.PageSize)
                             .ToList();

            return new PagedResult<PaymentAcknowledgment>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
