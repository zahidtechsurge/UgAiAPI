using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblReasonTranslation
    {
        [Key]
        public int ID { get; set; }
        public int ReasonID { get; set; }
        public string LanguageCode { get; set; }
        public string Text { get; set; }

        [ForeignKey("ReasonID")]
        public virtual tblReasons Reason { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Languages { get; set; }
    }
}
