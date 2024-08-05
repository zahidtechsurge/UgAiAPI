using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblBillingPaymentRequest
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Int64? OrderID { get; set; }
        public string consumer_number { get; set; }
        public string prefix { get; set; }
        public string Amount { get; set; }
        public string BankMemonic { get; set; }
        public string Tran_Auth_ID { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string Reserved { get; set; }


        public DateTime RequestTime{ get; set; }
        [ForeignKey("OrderID")]
        public TblOrders Order { get; set; }

        public virtual List<tblBillingPaymentResponse> BillingPaymentResponses { get; set; }
        public virtual List<tblTransaction> Transactions { get; set; }
    }
}
