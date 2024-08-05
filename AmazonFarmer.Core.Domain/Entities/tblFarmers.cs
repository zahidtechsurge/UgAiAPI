using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblfarm
    {
        [Key]
        public int FarmID { get; set; }
        public string UserID { get; set; }
        public string FarmName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public double? latitude { get; set; } = null;
        public double? longitude { get; set; } = null;
        public int? ApplicationID { get; set; }
        public int CityID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int Acreage { get; set; }
        public bool isLeased { get; set; } = false;
        public bool isPrimary { get; set; } = false;
        public EFarmStatus Status { get; set; } = EFarmStatus.Draft;
        public bool isApproved { get; set; } = false;
        public bool isFarmApprovalAcknowledged { get; set; } = false;
        public DateTime? isFarmApprovalAcknowledgedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string SAPFarmCode { get; set; }
        public virtual List<tblFarmAttachments> FarmAttachments { get; set; } = null!;
        public virtual List<tblFarmChangeRequest> FarmChangeRequests { get; set; } = null!;
        public virtual List<tblPlan> plans { get; set; } = null!;
        [ForeignKey("UserID")]
        public virtual TblUser Users { get; set; }
        [ForeignKey("CityID")]
        public virtual tblCity City { get; set; }
        [ForeignKey("DistrictID")]
        public virtual tblDistrict District { get; set; }
        [ForeignKey("TehsilID")]
        public virtual tblTehsil Tehsil { get; set; }
        [ForeignKey("ApplicationID")]
        public virtual tblFarmApplication FarmApplication { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual TblUser? Approver { get; set; }
    }
    public class TblUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; } = string.Empty;
        public EDesignation Designation { get; set; }
        public EActivityStatus Active { get; set; } = EActivityStatus.Active;
        public string? OTP { get; set; } = string.Empty;
        public DateTime? OTPExpiredOn { get; set; }
        public int WrongPasswordAttempt { get; set; }
        public bool isAccountLocked { get; set; } = false;
        public bool? isOTPApproved { get; set; } = false;
        public DateTime? LastPasswordChange { get; set; }
        public DateTime? SignupAgreementDateTime { get; set; }
        public virtual ICollection<TblFarmerClaim> Claims { get; set; }
        public virtual ICollection<TblFarmerLogin> Logins { get; set; }
        public virtual ICollection<TblFarmerToken> Tokens { get; set; }
        public virtual ICollection<TblFarmerRole> FarmerRoles { get; set; }
        public virtual List<TblProduct> Products { get; set; } = null!;
        public virtual List<TblOrders> Orders { get; set; } = null!;
        public virtual List<TblAuthorityLetters> AuthorityLetters { get; set; } = null;
        public virtual List<tblFarmChangeRequest> tblFarmChangeRequest { get; set; } = null!;
        public virtual List<tblFarmerProfile> FarmerProfile { get; set; } = null;
        public virtual List<tblfarm> farms { get; set; } = null;
        public virtual List<tblPlan> plans { get; set; } = null;
        //public virtual List<tblProfileChangeRequest> ProfileChangeRequest { get; set; } = null!;
        public virtual ICollection<tblProfileChangeRequest> ProfileChangeRequest { get; set; } = new List<tblProfileChangeRequest>();
        public virtual List<tblUserAttachments> UserAttachments { get; set; } = null!;
    }
    public class TblRole : IdentityRole
    {
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public TblRole(string RoleName) : base(RoleName)
        {

        }
        public TblRole() : base()
        {
        }

        public virtual ICollection<TblFarmerRole> FarmerRoles { get; set; }
        public virtual ICollection<TblRoleClaim> RoleClaims { get; set; }
    }
    public class TblFarmerRole : IdentityUserRole<string>
    {
        [ForeignKey("UserId")]
        public virtual TblUser User { get; set; }

        [ForeignKey("RoleId")]
        public virtual TblRole Role { get; set; }
    }
    public class TblFarmerLogin : IdentityUserLogin<string>
    {
    }
    public class TblFarmerToken : IdentityUserToken<string>
    {
    }
    public class TblFarmerClaim : IdentityUserClaim<string>
    {
    }

    public class TblRoleClaim : IdentityRoleClaim<string>
    {
        [ForeignKey("ClaimValue")]
        public virtual TblClaim Claim { get; set; }

        [ForeignKey("RoleId")]
        public virtual TblRole Role { get; set; }
    }
    public class TblClaim
    {
        [Key]
        public string ClaimId { get; set; }
        public string ClaimDescription { get; set; }
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual TblPage TblPage { get; set; }
        public virtual List<TblClaimAction> ClaimActions { get; set; }

        public virtual List<TblRoleClaim> RoleClaims { get; set; }

    }
    public class TblClaimAction
    {
        [Key]
        public string ClaimActionId { get; set; }
        public string ClaimId { get; set; }
        public string ClaimDescription { get; set; }

        [ForeignKey("ClaimId")]
        public TblClaim Claim { get; set; }

    }
    public partial class TblPage
    {
        public TblPage()
        {
            this.Claims = new HashSet<TblClaim>();
        }
        [Key]
        public int PageID { get; set; }
        public int ModuleID { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public Nullable<int> PageOrder { get; set; }
        public EActivityStatus IsActive { get; set; }
        public string? PageIcon { get; set; }
        public bool ShowOnMenu { get; set; }
        public string Controller { get; set; }
        public string ActionMethod { get; set; }
        public string ProjectModule { get; set; }

        [ForeignKey("ModuleID")]
        public virtual TblNavigationModule NavigationModule { get; set; }
        public virtual ICollection<TblClaim> Claims { get; set; }
    }
    public class TblNavigationModule
    {
        [Key]
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public EActivityStatus IsActive { get; set; }
        public bool ShowInMenu { get; set; }
        public int ModuleOrder { get; set; }
        public virtual List<TblPage> Pages { get; set; }
    }


}
