﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblPlanCrops
    {
        [Key]
        public int ID { get; set; }
        public int PlanID { get; set; }
        public int? CropID { get; set; }
        public int CropGroupID { get; set; }
        public double Acre { get; set; }
        public EPlanCropEndorse PlanCropEndorse { get; set; } = EPlanCropEndorse.Ok;
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;

        [ForeignKey("PlanID")]
        public virtual tblPlan? Plan { get; set; }
        [ForeignKey("CropID")]
        public virtual tblCrop? Crop { get; set; }
        [ForeignKey("CropGroupID")]
        public virtual tblCropGroup? CropGroup { get; set; }
        public virtual List<tblPlanProduct> PlanProducts { get; set; } = null!;
        public virtual List<tblPlanService> PlanServices { get; set; } = null!;
    }
}
