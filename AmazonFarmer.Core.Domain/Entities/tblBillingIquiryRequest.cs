using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblBillingIquiryRequest
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Int64? OrderID { get; set; }
        public string BankMnemonic { get; set; }
        public string Reserved { get; set; }
        public string Prefix { get; set; }
        public string ConsumerCode { get; set; }
        public DateTime RequestTime { get; set; }
        [ForeignKey("OrderID")]
        public TblOrders Order { get; set; }

        public virtual List<tblBillingIquiryResponse> BillingIquiryResponses { get; set; }
    }
}
