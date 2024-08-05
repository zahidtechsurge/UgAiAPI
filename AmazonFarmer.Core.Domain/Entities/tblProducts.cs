using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class TblProduct
    {
        [Key]
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public string ProductCode { get; set; }
        public string SalesOrg { get; set; }
        public int UOMID { get; set; }
        public string? Division { get; set; }
        public string? CreatedByID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public EActivityStatus Active { get; set; } = EActivityStatus.Active;



        [ForeignKey("CategoryID")]
        public virtual tblProductCategory Category { get; set; }
        [ForeignKey("UOMID")]
        public virtual tblUnitOfMeasure UOM { get; set; }
        [ForeignKey("CreatedByID")]
        public virtual TblUser? Users { get; set; }
        public virtual List<TblAuthorityLetterDetails> AuthorityLetterDetails { get; set; } = null!;
        public virtual List<tblProductTranslation> ProductTranslations { get; set; } = null!;
        public virtual List<tblPlanProduct> PlanProducts { get; set; } = null!;
        public virtual List<tblProductConsumptionMetrics> ProductConsumptionMetrics { get; set; } = null!;
    }
}
