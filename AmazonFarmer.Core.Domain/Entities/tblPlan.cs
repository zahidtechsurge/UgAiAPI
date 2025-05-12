using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblPlan
    {
        [Key]
        public int ID { get; set; }
        public required string UserID { get; set; }
        public required int FarmID { get; set; }
        public required int WarehouseID { get; set; }
        public required int SeasonID { get; set; }
        public string? Reason { get; set; }
        public string? FarmerComment { get; set; }
        public EPlanStatus Status { get; set; } = EPlanStatus.Draft;
        public EPlanChangeRequest PlanChangeStatus { get; set; } = EPlanChangeRequest.Default;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsPlanPaid { get; set; }
        public EModeOfPayment? ModeOfPayment { get; set; } = EModeOfPayment.Partial_Payment;
        [ForeignKey("UserID")]
        public virtual TblUser? User { get; set; }
        [ForeignKey("WarehouseID")]
        public virtual tblwarehouse? Warehouse { get; set; }
        [ForeignKey("FarmID")]
        public virtual tblfarm? Farm { get; set; }
        [ForeignKey("SeasonID")]
        public virtual tblSeason? Season { get; set; }
        public virtual List<tblPlanCrops>? PlanCrops { get; set; } = null;
        public virtual List<TblOrders>? Orders { get; set; } = null;
        public virtual List<TblOrderService>? OrderServices { get; set; }
    }
}
