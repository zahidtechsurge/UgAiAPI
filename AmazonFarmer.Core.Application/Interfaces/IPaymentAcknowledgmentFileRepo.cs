using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IPaymentAcknowledgmentFileRepo
    {
        PagedResult<PaymentAcknowledgmentFile> GetPaginatedFiles(PaymentAcknowledgmentFileRequest req);
        PaymentAcknowledgmentFile GetFileById(int id);
        Task<PaymentAcknowledgmentFile> GetPaymentAcknowledgmentFileByPaymentAcknowledgementID(int PKey);
        void UpdatepaymentAcknowledgmentFile(PaymentAcknowledgmentFile req);
    }
}
