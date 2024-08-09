using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblIntroLanguages
    {
        [Key]
        public int ID { get; set; }
        public int IntroID { get; set; }
        public required string LanguageCode { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        //public EActivityStatus Status { get; set; } = EActivityStatus.Active;


        [ForeignKey("IntroID")]
        public virtual tblIntro Intro { get; set; } = null!;
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; } = null!;
    }

}
