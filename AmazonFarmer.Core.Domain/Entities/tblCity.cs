using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblCity
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string CityCode { get; set; }
        public int DistrictID { get; set; }

        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblCityLanguages> CityLanguages { get; set; } = null!;
        public virtual List<tblFarmChangeRequest> tblFarmChangeRequest { get; set; } = null!;
        public virtual List<tblFarmerProfile> FarmerProfiles { get; set; } = null!;
        public virtual List<tblfarm> farms { get; set; } = null!;
        public virtual List<tblProfileChangeRequest> ProfileChangeRequest { get; set; } = null!;
        public virtual List<tblTehsil> Tehsils { get; set; } = null!;
    }

}
