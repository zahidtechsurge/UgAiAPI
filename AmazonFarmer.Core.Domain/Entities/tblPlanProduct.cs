using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblPlanProduct
    {
        [Key]
        public int ID { get; set; }
        public int PlanCropID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public DateTime Date { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public EOrderPaymentStatus PaymentStatus { get; set; } = EOrderPaymentStatus.NonPaid;
        public EDeliveryStatus DeliveryStatus { get; set; } = EDeliveryStatus.None;
        public int DeliveredQty { get; set; } = 0;

        [ForeignKey("PlanCropID")]
        public virtual tblPlanCrops PlanCrop { get; set; }
        [ForeignKey("ProductID")]
        public virtual TblProduct Product { get; set; }
        public virtual TblOrderProducts OrderProduct { get; set; }
    }
}
