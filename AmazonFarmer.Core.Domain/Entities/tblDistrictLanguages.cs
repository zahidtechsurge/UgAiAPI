using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblDistrictLanguages
    {
        [Key]
        public int ID { get; set; }
        public int DistrictID { get; set; }
        public string LanguageCode { get; set; }
        public string Translation { get; set; }

        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }

        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Languages { get; set; }
    }

}
