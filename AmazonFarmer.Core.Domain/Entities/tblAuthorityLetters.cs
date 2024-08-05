using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class TblAuthorityLetters
    {
        [Key]
        public int AuthorityLetterID { get; set; }
        public string AuthorityLetterNo { get; set; }
        public string DealerCode { get; set; }
        public int OrderID { get; set; }
        public string BearerName { get; set; }
        public string BearerNIC { get; set; }
        public string FilePath { get; set; }
        public bool IsOCRAutomated { get; set; }
        public string INVNumber { get; set; }
        public string FieldWHIncharge { get; set; }
        public string Dated { get; set; } //2022-03-17
        public bool Status { get; set; } = false;
        public EActivityStatus Active { get; set; } = EActivityStatus.Active;
        public string CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;


        [ForeignKey("OrderID")]
        public virtual TblOrders Order { get; set; }
        [ForeignKey("CreatedByID")]
        public virtual TblUser CreatedBy { get; set; }
        public virtual List<TblAuthorityLetterDetails> AuthorityLetterDetails { get; set; } = null!;
    }
}
