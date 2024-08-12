using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IUserRepo // Defining the interface for User repository
    {
        Task addUser(addUserDTO req); // Method signature for adding a user
        Task<TblRole> getRoleByRoleID(string roleID); // Method signature for getting a role by its ID
        Task<UserDTO> getUserByEmailndPassword(loginReq req); // Method signature for getting a user by email and password
        Task<string> getDeviceTokenByUserID(string userID); // Method signature for getting the device token by UserID
        Task<UserDTO> isUsernameValid(forgetPassword_Req req); // Method signature for checking if a username is valid for password recovery
        Task setOTP(string UserID, string OTPCode); // Method signature for setting OTP for a user
        Task<bool> verifyOTP(TblUser user); // Method signature for verifying OTP for a user
        Task emptyPasswordAttempts(TblUser user);
        Task lockUserAccount(TblUser user);
        Task unlockUserAccount(TblUser user);
        Task approverFarmerProfile(tblFarmerProfile user);
        Task<tblFarmerProfile> forgetPasswordOTP(string cnicNumber, string OTPCode); // Method signature for sending OTP for password recovery
        Task<TblUser> forgetUserPasswordOTP(string cnicNumber, string OTPCode); // Method signature for sending OTP for password recovery
        Task<UserDTO> getUserProfileByUserID(string UserID); // Method signature for getting user profile by user ID
        Task<bool> isUsernameExist(string username); // Method signature for checking if a username exists
        Task<UserDTO> getUserInfoByUsername(string username); // Method signature for getting user info by username
        Task<List<userotp_list>> getAllUsernameWithOTPs(); // Method signature for getting all usernames with OTPs
        Task newPassword(newPassword_Req req); // Method signature for setting a new password
        Task<UserDTO?> getUserInfoByCNIC(string CNIC); // Method signature for getting user info by CNIC
        Task approveFarmAccountByUserID(tblFarmerProfile profile, string ApproverID); // Method signature for approving farm account by user ID
        Task<tblFarmerProfile?> getFarmerProfileByUserID(string UserID); // Method signature for retrieving the Farmer profile by user ID
        Task<TblUser> getUserProfile(string UserID, string languageCode); // Method signature for getting user profile by user ID
        Task<TblUser?> getUserByUserName(string UserName); // Method signature for getting farmerProfile by user ID
        Task<TblUser> getUserByUserID(string userID); // Method signature for getting user by UserID
        Task<tblFarmerProfile?> GetUserProfileByValidation(string cnicNumber, string cellNumber);
        //Task<tblFarmerProfile?> GetUserProfileByValidation(string cnicNumber, string cellNumber, string ntnNumber);
        Task<tblFarmerProfile?> GetUserProfileByValidation(string cnicNumber, string cellNumber,
            string? ntnNumber = null, string? strnNumber = null); // Method signature for getting user by UserID
        Task<tblFarmerProfile> generateUserProfile(farmerSignUp_Req req, TblUser user);
        Task wrongPasswordCounter(TblUser user);
        Task updateDeviceToken(TblUser user, string deviceToken);
        Task<TblUser?> getFarmerProfileByUserCNIC(string cnicNumber);
        Task<getFarmerInfoResp> getFarmerInfoByFarmerIDAndLanguageCode(string userID, string languageCode);
        Task updateUser(TblUser user); // Method signature for updating by user Entity
        Task updateFarmerProfile(tblFarmerProfile userProfile);
        Task updateSelectedLanguage(tblFarmerProfile profile, string languageCode);
        Task<TblUser> getFarmerByFarmApplicationID(int applicationID);

        /// <summary>
        /// This function gets the district IDs for TSO
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetDistrictIDsForTSO(string userId);

        /// <summary>
        /// This function gets the district IDs for RSM
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetRegionIDsForRSM(string userId);


        Task<TblUser> getFarmerInfoByFarmerIDAndLanguageCodeUdpated(string userID, string languageCode);


        /// <summary>
        /// Get the TSO list by district IDs
        /// </summary>
        /// <param name="districtIds"></param>
        /// <returns></returns>
        Task<List<TblUser>> getTSOsByDistrictIDs(List<int> districtIds);
        /// <summary>
        /// get farmer by planID
        /// <param name="planID"></param>
        /// <returns></returns>
        /// </summary>
        Task<TblUser> getUserByPlanID(int planID);
        /// <summary>
        /// get list of Regional Sales Managers by farmID
        /// </summary>
        /// <param name="farmID"></param>
        /// <returns></returns>
        Task<List<TblUser>> getRSMsByFarmID(int farmID); // Method signature for getting list of Regional Sales Managers
        /// <summary>
        /// get list of Territory Sales Executive by farmID
        /// </summary>
        /// <param name="farmID"></param>
        /// <returns></returns>
        Task<List<TblUser>> getTSOsByFarmID(int farmID); // Method signature for getting list of Regional Sales Managers
        /// <summary>
        /// get list of National Sales Managers by farmID
        /// </summary>
        /// <param name="farmID"></param>
        /// <returns></returns>
        Task<List<TblUser>> getNSMsByFarmID(int farmID); // Method signature for getting list of National Sales Managers

        void RemoveActiveToken(ActiveToken token);
        void AddActiveToken(ActiveToken token);
        Task<List<ActiveToken>> GetActiveTokensForUser(string userId);
        Task<TblUser> getUserByUsername(string username, string phoneNumber);
        Task<TblUser> getUserByUsername(string username, string phoneNumber, string emailAddress);
        IQueryable<TblUser> getUsers();
        Task<TblUser> getUserDetailByUserID(string userID);
    }
}
