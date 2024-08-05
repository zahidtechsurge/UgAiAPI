using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class TblOrderProducts
    {
        [Key]
        public int OrderProductID { get; set; }
        public Int64 OrderID { get; set; }
        public int ProductID { get; set; }
        public int? PlanProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitTax { get; set; }
        public decimal UnitTotalAmount { get; set; }
        public decimal Amount { get; set; }
        public int QTY { get; set; }
        public int ClosingQTY { get; set; }//Shipped QTY

        [ForeignKey("ProductID")]
        public virtual TblProduct Product { get; set; }
        [ForeignKey("OrderID")]
        public virtual TblOrders Order { get; set; }

        [ForeignKey("PlanProductID")]
        public virtual tblPlanProduct PlanProduct { get; set; }
    }
    public class TblOrderService
    {
        [Key]
        public int OrderServiceID { get; set; }
        public int? PlanID { get; set; }
        public int? CropID { get; set; }
        public int? CropGroupID { get; set; }
        public int ServiceID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitTax { get; set; }
        public decimal UnitTotalAmount { get; set; }
        public decimal Amount { get; set; }
        public int QTY { get; set; }
        public int ClosingQTY { get; set; }//Shipped QTY
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;

        //For Services
        public DateTime LastHarvestDate { get; set; }
        //For Services
        public DateTime LandPreparationDate { get; set; }
        //For Services
        public DateTime SewingDate { get; set; }

        public EServiceVendorStatus? VendorStatus { get; set; }
        public EServiceRequestStatus? RequestStatus { get; set; }
        public DateTime? ScehduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("PlanID")]
        public virtual tblPlan Plan { get; set; }

        [ForeignKey("ServiceID")]
        public virtual tblService Service { get; set; }
    }
}
