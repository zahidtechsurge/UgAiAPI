using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblAttachmentTypes
    {
        [Key]
        public int ID { get; set; }
        public EAttachmentType AttachmentType { get; set; }
        public string TypeName { get; set; }
        public virtual List<tblAttachment> Attachment { get; set; } = null!;
    }
}
