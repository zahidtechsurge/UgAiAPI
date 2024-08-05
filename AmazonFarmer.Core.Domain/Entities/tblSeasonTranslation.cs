using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblSeasonTranslation
    {
        [Key]
        public int ID { get; set; }
        public int SeasonID { get; set; }
        public string LanguageCode { get; set; }
        public string Image { get; set; }
        public string Translation { get; set; }


        [ForeignKey("SeasonID")]
        public virtual tblSeason Season { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; }
    }

}
