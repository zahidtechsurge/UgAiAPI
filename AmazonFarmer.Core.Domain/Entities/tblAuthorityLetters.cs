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
        public string SAPFarmerCode { get; set; }
        public Int64 OrderID { get; set; }
        public string BearerName { get; set; }
        public string BearerNIC { get; set; }
        public int? AttachmentID { get; set; }
        public string? PDFGUID { get; set; } = string.Empty;
        public bool IsOCRAutomated { get; set; }
        public string? INVNumber { get; set; } = string.Empty;
        public string FieldWHIncharge { get; set; }
        public DateTime Dated { get; set; } //2022-03-17
        public bool Status { get; set; } = false;
        public EAuthorityLetterStatus Active { get; set; } = EAuthorityLetterStatus.Active;
        public string CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public int WareHouseID { get; set; }


        [ForeignKey("OrderID")]
        public virtual TblOrders Order { get; set; }
        [ForeignKey("AttachmentID")]
        public virtual tblAttachment Attachment { get; set; }
        [ForeignKey("CreatedByID")]
        public virtual TblUser CreatedBy { get; set; }
        public virtual List<TblAuthorityLetterDetails> AuthorityLetterDetails { get; set; } = null!;


        [ForeignKey("WareHouseID")]
        public virtual tblwarehouse Warehouse { get; set; }
    }
}
