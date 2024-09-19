using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class PaymentAcknowledgmentFileDto
    {
        public string FileName { get; set; }
        public DateTime DateReceived { get; set; }
        public string Status { get; set; }
        public int RowsCount { get; set; }
        public int AcknowledgedRowsCount { get; set; }
        public int CompletedRowsCount { get; set; }
        public string? Comment { get; set; }
        public int FileId { get; set; }
    }
    public class PaymentAcknowledgmentFileRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Status { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class PaymentAcknowledgmentDto
    {
        public int paymentAcknowledgementID { get; set; }
        public string CompanyName { get; set; }
        public string ConsumerNumber { get; set; }
        public string Amount { get; set; }
        public string DatePaid { get; set; }
        public string TimePaid { get; set; }
        public string SettlementDate { get; set; }
        public string TransAuthId { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
        public bool CanReprocess { get; set; }
    }


    public class PaymentAcknowledgmentDetailRequest
    {
        public int Id { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Status { get; set; } 
    }
}
