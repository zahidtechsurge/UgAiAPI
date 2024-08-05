using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class PaymentAcknowledgmentFile
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public DateTime DateReceived { get; set; }
        public EPaymentAcknowledgmentFileStatus Status { get; set; }
        public int RowsCount { get; set; }
        public string? Comment { get; set; }
        public List<PaymentAcknowledgment>? PaymentAcknowledgments { get; set; }
    }


    public class PaymentAcknowledgment
    {
        [Key]
        public int Id { get; set; }
        public int FileID { get; set; }
        public DateTime DateReceived { get; set; }
        public string CompanyName { get; set; }
        public string ConsumerNumber { get; set; }
        public string AccountNumber { get; set; }
        public string MaskedConsumerNumber { get; set; }
        public string Amount { get; set; }
        public string DatePaid { get; set; }
        public string TimePaid { get; set; }
        public string SettlementDate { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string Trans_Auth_ID { get; set; }
        public string STAN { get; set; }
        public EPaymentAcknowledgmentStatus Status { get; set; }
        public string? Comment { get; set; }

        [ForeignKey("FileID")]
        public PaymentAcknowledgmentFile PaymentAcknowledgmentFile { get; set; }


    }
}
