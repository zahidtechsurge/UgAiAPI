using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblCrop
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblCropTimings> CropTimings { get; set; } = null!;
        public virtual List<tblCropTranslation> CropTranslations { get; set; } = null!;
        public virtual List<tblPlanCrops> PlanCrops { get; set; } = null;
        public virtual List<tblProductConsumptionMetrics> ProductConsumptionMetrics { get; set; } = null!;
    }

}
