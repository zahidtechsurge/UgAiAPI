using Microsoft.EntityFrameworkCore;
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
        public Int64 OrderID { get; set; }
        public int PlanID { get; set; }
        public int? CropID { get; set; }
        public EOrderType OrderType { get; set; }
        public bool isLocked { get; set; } = false;
        public EOrderStatus OrderStatus { get; set; } = EOrderStatus.Active;
        public EOrderPaymentStatus PaymentStatus { get; set; } = EOrderPaymentStatus.NonPaid;
        public EDeliveryStatus DeliveryStatus { get; set; } = EDeliveryStatus.None;
        public bool IsConsumed { get; set; } = false;
        public string? OrderName { get; set; }
        public string? SAPOrderID { get; set; }
        public string? OneLinkTransactionID { get; set; }
        public string? SAPTransactionID { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal AdvancePercent { get; set; }
        public decimal ApprovalDatePrice { get; set; }
        public DateTime ApprovalDate { get; set; }
        public decimal? PaymentDatePrice { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? InvoicedDatePrice { get; set; }
        public DateTime? InvoicedDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public Int64? ParentOrderID { get; set; }
        public string CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime DuePaymentDate { get; set; } = DateTime.Now;
        public int OrderRandomTransactionID { get; set; }
        public int WarehouseID { get; set; }
        public string FiscalYear { get; set; } = "";
        public string CompanyCode { get; set; } = "";
        public decimal ReconciliationAmount { get; set; }

        [ForeignKey("WarehouseID")]
        public virtual tblwarehouse Warehouse { get; set; }


        [ForeignKey("CreatedByID")]
        public virtual TblUser User { get; set; }

        [ForeignKey("PlanID")]
        public virtual tblPlan Plan { get; set; }

        [ForeignKey("CropID")]
        public virtual tblCropGroup Crop { get; set; }

        [ForeignKey("ParentOrderID")]
        public virtual TblOrders ParentOrder { get; set; }

        public virtual List<TblOrders> ChildOrders { get; set; } = new List<TblOrders>();
        public virtual List<TblAuthorityLetters> AuthorityLetters { get; set; } = null!;
        public virtual List<TblOrderProducts>? Products { get; set; }
        public virtual List<tblTransaction> Transactions { get; set; }
    }


    public class TblOrderLog
    {

        [Key]
        public int OrderLogId { get; set; }
        public Int64 OrderID { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public EOrderType OrderType { get; set; }
        public EOrderStatus OrderStatus { get; set; }
        public EOrderPaymentStatus PaymentStatus { get; set; } = EOrderPaymentStatus.NonPaid;
        public EDeliveryStatus DeliveryStatus { get; set; } = EDeliveryStatus.None;

        [ForeignKey("OrderID")]
        public TblOrders Order { get; set; }

    }
}
