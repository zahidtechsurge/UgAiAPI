using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class EmployeeDashboardResponse
    {
        public int pendingFarms { get; set; }
        public int completedFarms { get; set; }
        public int blockedOrders { get; set; }
        public int pendingPlans { get; set; }
        public int completedPlans { get; set; }
        public int changeRequestPlans { get; set; }
    }
    public class farmerProfileDTO
    {
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string dateOfBirth { get; set; }
        public string fatherName { get; set; }
        public string cnicNumber { get; set; }
        public List<uploadAttachmentResp> cnicAttachment { get; set; }
        public string ntnNumber { get; set; }
        public string strnNumber { get; set; }
        public List<uploadAttachmentResp> ntnAttachment { get; set; }
    }
    public class addUserDTO
    {
        public int DesignationID { get; set; }
        public string RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
    }
    public class loginReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class userotp_list
    {
        public string username { get; set; }
        public string otp { get; set; }
    }

    public class UserDTO
    {
        public string userID { get; set; }
        public int? designationID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public string cnic { get; set; }
        public bool isOTPVerified { get; set; }
        //public bool isApproved { get; set; } = true;
        public bool hasFarms { get; set; }
        //public bool enableEdit { get; set; }
        public bool isFarmApprovalAcknowledged { get; set; }
        public string languageCode { get; set; }
        public string applicationID { get; set; }
        public string applicationMessage { get; set; }
        public int isApprovalStatus { get; set; }
        public bool approvedByTSO { get; set; } = false;
        public bool approvedByRSM { get; set; } = false;
        public bool approvedByPatwari { get; set; } = false;
    }

    public class farmerSignUp_Req
    {
        public decimal appVersion { get; set; }
        public string platform { get; set; }
        public string deviceToken { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; } = string.Empty;
        public string dateOfBirth { get; set; }
        public string fatherName { get; set; }
        public string cnicNumber { get; set; }
        public List<uploadAttachmentResp> cnicAttachments { get; set; } = new List<uploadAttachmentResp>();
        //public List<farmAttachment> cnicAttachments { get; set; } = new List<farmAttachment>();
        public string ntnNumber { get; set; }
        public List<uploadAttachmentResp> ntnAttachments { get; set; } = new List<uploadAttachmentResp>();
        //public List<farmAttachment> ntnAttachments { get; set; } = new List<farmAttachment>();
        public string strnNumber { get; set; }
        public string cellNumber { get; set; }
        public string emailAddress { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; } = string.Empty;
        public int districtID { get; set; }
        public int cityID { get; set; }
        public decimal ownedLandAcreage { get; set; }
        public decimal leasedLandAcreage { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public bool isTermsAccepted { get; set; } = false;
        public string selectedLangCode { get; set; }
    }
    public class TokenResponse
    {
        public string Token { get; set; } = "";
        public long ExpiresIn { get; set; }
    }
    public class otpVerification_req
    {
        public string cnicNumber { get; set; }
        public string otpCode { get; set; }
    }
    public class forgetPassword_Req
    {
        public string cnicNumber { get; set; }
    }
    public class foretPassword_Resp
    {
        public bool isValid { get; set; }
    }
    public class changePassword_Req
    {
        public string currentPassword { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
    public class newPassword_Req
    {
        public decimal appVersion { get; set; }
        public string cnicNumber { get; set; }
        public string platform { get; set; }
        public string otpCode { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
    public class Login_Req
    {
        public string username { get; set; }
        public string password { get; set; }
        public decimal appVersion { get; set; }
        public string deviceToken { get; set; }
        public string platform { get; set; }
        public string languageCode { get; set; } = "EN";
    }
    public class approveFarmer_Req
    {
        public string userID { get; set; }
    }
    public class getUserProfile_Resp
    {
        public int status { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public string fatherName { get; set; }
        public string cnicNumber { get; set; }
        public List<uploadAttachmentResp> cnic { get; set; }
        public string ntnNumber { get; set; }
        public List<uploadAttachmentResp> ntn { get; set; }
        public string strnNumber { get; set; }
        public string cellNumber { get; set; }
        public string emailAddress { get; set; }
        public string ownedLandAcreage { get; set; }
        public string leasedLandAcreage { get; set; }
        public int cityID { get; set; }
        public string city { get; set; }
        public int districtID { get; set; }
        public string district { get; set; }
        public List<string>? messages { get; set; }
        public bool isGenericMessageEnabled { get; set; } = false;
    }
    public class getFarmerInfoResp : getUserProfile_Resp
    {
        public List<farmDetail_Resp> farms { get; set; }
    }
    public class NotificationRequest
    {
        public ENotificationType Type { get; set; }
        public List<NotificationRequestRecipient> Recipients { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class NotificationRequestRecipient
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class GetDashboard_Resp
    {
        public List<BannerDTO> banners { get; set; } = new List<BannerDTO>();
        public List<getNearestPayableOrders> nextPay { get; set; } = new List<getNearestPayableOrders>();
        public List<getNearestPickupDates> nextPickup { get; set; } = new List<getNearestPickupDates>();
        public WeatherDTO weather { get; set; } = new WeatherDTO();
    }
    public class configurePasswordRequest
    {
        public required string password { get; set; }
    }
    public class configurePassword_Resp
    {
        public string username { get; set; } = string.Empty;
    }
    public class addEmployeeDTO
    {
        public required string firstName { get; set; }
        public required string lastName { get; set; }
        public required string userName { get; set; }
        public required string emailAddress { get; set; }
        public required string phoneNumber { get; set; }
        public required string cnicNumber { get; set; }
        public required int designationID { get; set; }
        public int[]? districtIDs { get; set; } = [];
        public int[]? regionIDs { get; set; } = [];
        public bool status { get; set; }
        public bool isLocked { get; set; }
    }
    public class updateEmployeeDTO : addEmployeeDTO
    {
        public required string userID { get; set; }
    }

}
