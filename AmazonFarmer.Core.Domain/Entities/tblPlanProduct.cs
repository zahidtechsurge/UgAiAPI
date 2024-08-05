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
        public decimal Qty { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("PlanCropID")]
        public virtual tblPlanCrops PlanCrop { get; set; }
        [ForeignKey("ProductID")]
        public virtual TblProduct Product { get; set; }
    }
}
