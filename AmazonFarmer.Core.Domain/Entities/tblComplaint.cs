using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblComplaint
    {
        [Key]
        public int ComplaintID {  get; set; }
        public string ComplaintTitle { get; set; } = string.Empty;
        public string ComplaintDesc { get; set; } = string.Empty;
        public EComplaintStatus ComplaintStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedByID { get; set; } = string.Empty;
        public DateTime? ResolvedOn { get; set; }
        public string? ResolvedByID { get; set; } = string.Empty;

        [ForeignKey("CreatedByID")]
        public virtual TblUser CreatedBy { get; set; } = null!;
        //[ForeignKey("ResolvedByID")]
        //public virtual TblUser ResolvedBy { get; set; } = null!;
    }
}
