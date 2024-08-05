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
        public string UserID { get; set; }
        public int FarmID { get; set; }
        public int SeasonID { get; set; }
        public string? Reason { get; set; }
        public ERequestType Status { get; set; } = ERequestType.Draft;
        [ForeignKey("UserID")]
        public virtual TblUser User { get; set; }
        [ForeignKey("FarmID")]
        public virtual tblfarm Farm { get; set; }
        [ForeignKey("SeasonID")]
        public virtual tblSeason Season { get; set; }
        public virtual List<tblPlanCrops>? PlanCrops { get; set; } = null;
        public virtual List<TblOrders>? Orders { get; set; } = null;
    }
}
