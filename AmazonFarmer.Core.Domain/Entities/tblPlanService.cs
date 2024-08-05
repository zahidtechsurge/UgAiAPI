using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblPlanService
    {
        [Key]
        public int ID { get; set; }
        public int PlanCropID { get; set; }
        public int ServiceID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("PlanCropID")]
        public virtual tblPlanCrops PlanCrop { get; set; }
        [ForeignKey("ServiceID")]
        public virtual tblService Service { get; set; }
    }
}
