using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class TblOrders
    {
        [Key]
        public int OrderID { get; set; }
        public int PlanID { get; set; }
        public EOrderType OrderType { get; set; }
        public EActivityStatus OrderStatus { get; set; } = EActivityStatus.Active;
        public string? SAPOrderID { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal AdvancePercent { get; set; }
        public decimal ApprovalDatePrice { get; set; }
        public DateTime ApprovalDate { get; set; }
        public decimal PaymentDatePrice { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal InvoicedDatePrice { get; set; }
        public DateTime? InvoicedDate { get; set; }
        public int? ParentOrderID { get; set; }
        public string CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;


        [ForeignKey("CreatedByID")]
        public virtual TblUser User { get; set; }
        [ForeignKey("PlanID")]
        public virtual tblPlan Plan { get; set; }
        public virtual List<TblAuthorityLetters> AuthorityLetters { get; set; } = null!;
    }
}
