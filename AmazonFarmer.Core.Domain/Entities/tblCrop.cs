using System; 
using System.ComponentModel.DataAnnotations; 

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblCrop
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblCropTimings> CropTimings { get; set; } = null!;
        public virtual List<tblCropTranslation> CropTranslations { get; set; } = null!;
        public virtual List<tblPlanCrops> PlanCrops { get; set; } = null!;
        public virtual List<tblProductConsumptionMetrics> ProductConsumptionMetrics { get; set; } = null!;
        public virtual List<TblOrders> Orders { get; set; } = null!;
        public virtual List<tblCropGroupCrops> CropGroupCrops { get; set; } = null!;
    }

}
