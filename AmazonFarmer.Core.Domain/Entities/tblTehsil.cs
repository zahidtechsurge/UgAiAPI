using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblTehsil
    {
        [Key]
        public int ID { get; set; }
        public string TehsilCode { get; set; }
        public string Name { get; set; }
        public int CityID { get; set; }
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblFarmChangeRequest> FarmChangeRequests { get; set; } = null!;
        public virtual List<tblFarmerProfile> FarmerProfiles { get; set; } = null!;
        public virtual List<tblfarm> farms { get; set; } = null!;
        public virtual List<tblProfileChangeRequest> ProfileChangeRequest { get; set; } = null!;
        public virtual List<tblTehsilLanguages> TehsilLanguagess { get; set; } = null!;
        [ForeignKey("CityID")]
        public virtual tblCity City { get; set; } 
    }

}
