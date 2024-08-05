using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblFarmChangeRequest
    {
        [Key]
        public int ID { get; set; }
        public int? FarmID { get; set; }
        public string UserID { get; set; }
        public string FarmName { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public int CityID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int ApplicationID { get; set; }
        public double Acreage { get; set; }
        public bool isLeased { get; set; } = false;
        public bool isPrimary { get; set; } = false;
        public EFarmStatus RequestStatus { get; set; } = EFarmStatus.Draft;
        public bool isApproved { get; set; } = false;
        public bool isFarmApprovalAcknowledged { get; set; } = false;
        public DateTime? isFarmApprovalAcknowledgedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? SAPFarmCode { get; set; }
        public string? FarmerComment { get; set; }

        [ForeignKey("FarmID")]
        public virtual tblfarm? Farm { get; set; } = null;
        [ForeignKey("UserID")]
        public virtual TblUser User { get; set; }
        [ForeignKey("CityID")]
        public virtual tblCity City { get; set; }
        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
        [ForeignKey("TehsilID")]
        public virtual tblTehsil Tehsil { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual TblUser? ApprovedByUser { get; set; } = null;
        [ForeignKey("ApplicationID")]
        public virtual tblFarmApplication? FarmApplication { get; set; } = null;
    }
}
