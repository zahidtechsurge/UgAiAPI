using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblBannerLanguages
    {
        [Key]
        public int ID { get; set; }
        public int BannerID { get; set; }
        public string LanguageCode { get; set; }
        public string Image { get; set; }
        //public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        [ForeignKey("BannerID")]
        public virtual tblBanner Banner { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Languages { get; set; }
    }

}
