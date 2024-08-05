using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
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
        public int designationID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string cnic { get; set; }
        public bool isOTPVerified { get; set; }
        public bool isApproved { get; set; }
        public bool hasFarms { get; set; }
        public bool isFarmApprovalAcknowledged { get; set; }
        public string applicationID { get; set; }
        public string languageCode { get; set; }
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
        public List<farmAttachment> cnicAttachments { get; set; } = new List<farmAttachment>();
        public string ntnNumber { get; set; }
        public List<farmAttachment> ntnAttachments { get; set; } = new List<farmAttachment>();
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
    }
    public class approveFarmer_Req
    {
        public string userID { get; set; }
    }
    public class getUserProfile_Resp
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public string fatherName { get; set; }
        public string cnicNumber { get; set; }
        public string ntnNumber { get; set; }
        public string strnNumber { get; set; }
        public string cellNumber { get; set; }
        public string emailAddress { get; set; }
        public string ownedLandAcreage { get; set; }
        public string leasedLandAcreage { get; set; }
    }
}
