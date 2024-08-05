using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblHomeSliderLanguages
    {
        [Key]
        public int ID { get; set; }
        public int HomeSliderID { get; set; }
        public string LanguageCode { get; set; }
        public string Image { get; set; }

        [ForeignKey("HomeSliderID")]
        public virtual tblHomeSlider HomeSlider { get; set; }

        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; }
    }

}
