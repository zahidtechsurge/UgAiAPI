using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblRegion
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblDistrict> Districts{ get; set; } = null!;
        public virtual List<tblRegionLanguages> RegionLanguages { get; set; } = null!;
        public virtual List<TblEmployeeRegionAssignment> EmployeeRegions { get; set; } = null;

    }

    public class tblRegionLanguages
    {
        [Key]
        public int ID { get; set; }
        public int RegionID { get; set; }
        public string LanguageCode { get; set; }
        public string Translation { get; set; }
        [ForeignKey("RegionID")]
        public virtual tblRegion Region { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; }
    }

}
