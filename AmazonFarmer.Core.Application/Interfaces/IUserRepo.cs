using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces
{
    public interface IUserRepo
    {
        Task addUser(addUserDTO req);
        Task<IdentityRole> getRoleByRoleID(string roleID);
        Task<UserDTO> getUserByEmailndPassword(loginReq req);
        Task<UserDTO> getUserByUsernameandPassword(Login_Req req);
        Task<UserDTO> isUsernameValid(forgetPassword_Req req);
        Task<UserDTO> signupFarmer(farmerSignUp_Req req);
        Task setOTP(string UserID, string OTPCode);
        Task<bool> verifyOTP(string UserID, string OTPCode);
        Task<bool> forgetPasswordOTP(string cnicNumber, string OTPCode);
        Task<UserDTO> getUserProfileByUserID(string UserID);
        Task changePassword(changePassword_Req req, string userID);
        Task<bool> isUsernameExist(string username);
        Task<UserDTO> getUserInfoByUsername(string username);
        Task<List<userotp_list>> getAllUsernameWithOTPs();
        Task newPassword(newPassword_Req req);
        Task<UserDTO> getUserInfoByCNIC(string CNIC);
        Task approveFarmAccountByUserID(string UserID, string ApproverID);
        Task<getUserProfile_Resp> getUserProfile(string UserID);

    }
}
