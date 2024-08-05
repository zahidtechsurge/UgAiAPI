using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblFarmerProfile
    {
        [Key]
        public int ProfileID { get; set; }
        public string UserID { get; set; }
        public string FatherName { get; set; }
        public string CNICNumber { get; set;}
        public string NTNNumber { get; set;}
        public string STRNNumber { get; set;}
        public string CellNumber { get; set;}
        public string OwnedLand { get; set;}
        public string LeasedLand { get; set;}
        public int TotalArea { get; set;}
        public string DateOfBirth { get; set;} //dd-mm-yyyy
        public string Address1 { get; set;}
        public string? Address2 { get; set; } = string.Empty;
        public int CityID { get; set;}
        public int DistrictID { get; set;}
        //public int TehsilID { get; set;}
        public string? SAPFarmerCode { get; set;}
        public EFarmerProfileStatus isApproved { get; set; } = EFarmerProfileStatus.Pending;
        public string? ApprovedByID { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string SelectedLangCode { get; set; }
        [ForeignKey("UserID")]
        public virtual TblUser User { get; set; }
        [ForeignKey("CityID")]
        public virtual tblCity City { get; set; }
        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
        //[ForeignKey("TehsilID")]
        //public virtual tblTehsil Tehsil { get; set; }

    }
    
}
