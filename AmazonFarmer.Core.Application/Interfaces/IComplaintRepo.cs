using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IComplaintRepo
    {
        void AddComplaint(tblComplaint Complaint);
        void UpdateComplaint(tblComplaint Complaint);
        IQueryable<tblComplaint> GetComplaints();
        Task<tblComplaint?> GetComplaintByID(int ComplaintId);
    }
}
