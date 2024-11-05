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
    public class ComplaintRepo : IComplaintRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the ComplaintRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public ComplaintRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public void AddComplaint(tblComplaint Complaint)
        {
            _context.Complaints.Add(Complaint);
        }
        public void UpdateComplaint(tblComplaint Complaint)
        {
            _context.Complaints.Update(Complaint);
        }
        public IQueryable<tblComplaint> GetComplaints()
        {
            return _context.Complaints.Include(x=>x.CreatedBy);
        }
        public async Task<tblComplaint?> GetComplaintByID(int ComplaintId)
        {
            return await _context.Complaints
                .Include(x=>x.CreatedBy)
                .Where(x=>x.ComplaintID == ComplaintId)
                .FirstOrDefaultAsync();
        }
    }
}
