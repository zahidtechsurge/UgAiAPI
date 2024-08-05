using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblProfileChangeRequest
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string FatherName { get; set; }
        public string CNICNumber { get; set; }
        public string NTNNumber { get; set; }
        public double OwnedLand { get; set; }
        public double LeasedLand { get; set; }
        public double TotalArea { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; } = string.Empty;
        public int CityID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public string SAPFarmerCode { get; set; }
        public bool isApproved { get; set; } = false;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime SubmittedDate { get; set; }
        public EActivityStatus RequestStatus { get; set; } = EActivityStatus.Active;


        [ForeignKey("UserID")]
        public virtual TblUser User { get; set; }
        [ForeignKey("CityID")]
        public virtual tblCity City { get; set; }
        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
        [ForeignKey("TehsilID")]
        public virtual tblTehsil Tehsil { get; set; }
        [ForeignKey("ApprovedBy")]
        public virtual TblUser? Approver { get; set; }
    }
}
