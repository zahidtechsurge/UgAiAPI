using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblDistrict
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string DistrictCode { get; set; }
        public int? RegionId { get; set; } 
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;

        [ForeignKey("RegionId")]
        public virtual tblRegion Region { get; set; }
        public virtual List<tblCropTimings> CropTimings { get; set; } = null!;
        public virtual List<tblDistrictLanguages> DistrictLanguages { get; set; } = null!;
        public virtual List<tblFarmChangeRequest> tblFarmChangeRequest { get; set; } = null!;
        public virtual List<tblFarmerProfile> FarmerProfiles { get; set; } = null!;
        public virtual List<tblfarm> farms { get; set; } = null!;
        public virtual List<tblProfileChangeRequest> ProfileChangeRequest { get; set; } = null!;
        public virtual List<tblCity> Cities { get; set; } = null!; 
        public virtual List<tblwarehouse> Warehouses{ get; set; } = null!;
        public virtual List<TblEmployeeRegionAssignment> EmployeeDistricts { get; set; } = null;
    }

}
