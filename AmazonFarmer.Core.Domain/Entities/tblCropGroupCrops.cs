using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblCropGroup
    {
        [Key]
        public int ID { get; set; }
        public EActivityStatus Status { get; set; }
        public virtual List<tblPlanCrops> PlanCrops { get; set; } = null!;
        public virtual List<tblCropGroupCrops> CropGroupCrops { get; set; } = null!;
        public virtual List<TblOrders> Orders { get; set; } = null!;
    }
    public class tblCropGroupCrops
    {
        [Key]
        public int ID { get; set; }
        public int CropGroupID { get; set; }
        public required int CropID { get; set; }
        [ForeignKey("CropGroupID")]
        public virtual tblCropGroup? CropGroup { get; set; }
        [ForeignKey("CropID")]
        public virtual tblCrop? Crop { get; set; }
        //public virtual List<tblPlanCrops> PlanCrops { get; set; } = null!;
    }
}
