using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblProductConsumptionMetrics
    {
        [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int CropID { get; set; }
        public int? TerritoryID { get; set; }
        public decimal Usage { get; set; }
        public string UOM { get; set; }
        [ForeignKey("ProductID")]
        public virtual TblProduct Product { get; set; }
        [ForeignKey("CropID")]
        public virtual tblCrop Crop { get; set; }
        //[ForeignKey("UserID")]
        //public virtual TblUser Users { get; set; }

    }
}
