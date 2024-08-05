using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblFarmAttachments
    {
        [Key]
        public int ID { get; set; }
        public int FarmID { get; set; }
        public int AttachmentID { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;

        [ForeignKey("FarmID")]
        public virtual tblfarm Farm { get; set; }

        [ForeignKey("AttachmentID")]
        public virtual tblAttachment Attachment { get; set; }
    }
}
