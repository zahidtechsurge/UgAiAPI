using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblUserAttachments
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public int tblAttachmentID { get; set; }
        public EActivityStatus Status { get; set; }


        [ForeignKey("UserID")]
        public virtual TblUser User { get; set; }
        [ForeignKey("tblAttachmentID")]
        public virtual tblAttachment Attachment { get; set; }
    }
}
