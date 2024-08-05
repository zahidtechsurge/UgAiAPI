using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblService
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? CreatedByID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public EActivityStatus Active { get; set; } = EActivityStatus.Active;


        [ForeignKey("CreatedByID")]
        public virtual TblUser Users { get; set; }
        public virtual List<TblAuthorityLetterDetails> AuthorityLetterDetails { get; set; } = null!;
        public virtual List<tblServiceTranslation> ServiceTranslations { get; set; } = null!;
        public virtual List<tblPlanService> PlannedServices { get; set; } = null!;
    }
}
