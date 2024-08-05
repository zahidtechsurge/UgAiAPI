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
        public string LanguageCode { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }


        [ForeignKey("IntroID")]
        public virtual tblIntro Intro { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Languages { get; set; }
    }

}
