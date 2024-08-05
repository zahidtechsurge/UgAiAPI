using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblBillingPaymentResponse
    {
        [Key]
        public int Id { get; set; }
        public int BillingPaymentRequestID { get; set; }
        public string Response_Code { get; set; }
        public string Identification_parameter { get; set; }
        public string Reserved { get; set; }
        public DateTime ResponseTime { get; set; }

        [ForeignKey("BillingPaymentRequestID")]
        public tblBillingPaymentRequest BillingPaymentRequest { get; set; }
    }
}
