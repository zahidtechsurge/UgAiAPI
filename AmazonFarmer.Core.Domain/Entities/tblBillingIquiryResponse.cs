using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblBillingIquiryResponse
    {
        [Key]
        public int Id { get; set; }
        public int BillingInquiryRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ConsumerNumber{ get; set; }
        public string ConsumerDetail { get; set; }
        public string BillStatus { get; set; }
        public DateTime? DueDate { get; set; }
        public string AmountWithInDueDate { get; set; }
        public string AmountAfterDueDate { get; set; }
        public string BillingMonth { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? TimePaid { get; set; }
        public string AmountPaid { get; set; }
        public string Tran_auth_ID { get; set; }
        public string Reserved { get; set; }
        public DateTime ResponseTime { get; set; }

        [ForeignKey("BillingInquiryRequestID")]
        public tblBillingIquiryRequest BillingInquiryRequest { get; set; }
    }
}
