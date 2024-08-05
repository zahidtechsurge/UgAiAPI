using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblAttachment
    {
        [Key]
        public int ID { get; set; }
        public int tblAttachmentTypeID { get; set; }
        public Guid Guid { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
        [ForeignKey("tblAttachmentTypeID")]
        public virtual tblAttachmentTypes AttachmentType { get; set; }
        public virtual List<tblFarmAttachments> FarmAttachments { get; set; } = null!;
        public virtual List<tblUserAttachments> UserAttachments { get; set; } = null!;
    }
}
