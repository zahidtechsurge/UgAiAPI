using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly SignInManager<TblUser> _signInManager;
        private readonly RoleManager<TblRole> _roleManager;
        private readonly UserManager<TblUser> _userManager;
        private AmazonFarmerContext _context;
        public UserRepo(AmazonFarmerContext context, SignInManager<TblUser> signInManager, RoleManager<TblRole> roleManager, UserManager<TblUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task addUser(addUserDTO req)
        {
            try
            {
                Microsoft.AspNetCore.Identity.IdentityResult updateResult = new Microsoft.AspNetCore.Identity.IdentityResult();

                EDesignation Designation = (EDesignation)Enum.Parse(typeof(EDesignation), req.DesignationID.ToString());

                int isExist = await _context.Users.Where(x => x.Email == req.Email).CountAsync();

                TblUser temp = await _context.Users.Where(x => x.Email == req.Email).FirstOrDefaultAsync();

                if (isExist > 0)
                {
                    throw new Exception(_exceptions.userAlreadyExist);
                }

                TblUser _req = new TblUser()
                {
                    Active = EActivityStatus.Active,
                    Designation = Designation,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    UserName = req.Email,
                    PhoneNumber = req.Phone,
                };

                updateResult = await _userManager.CreateAsync(_req, req.Password);
                if (updateResult.Succeeded)
                {
                    IdentityRole newRole = await getRoleByRoleID(req.RoleID);

                    //await _userManager.AddToRoleAsync(_req, newRole.Name);
                    await _userManager.AddToRoleAsync(temp, newRole.Name);
                }
                else
                {
                    if (updateResult.Errors != null && updateResult.Errors.Count() > 0)
                    {
                        throw new Exception(updateResult.Errors.FirstOrDefault().Description);
                    }
                    else
                    {
                        throw new Exception(_exceptions.errorOccuredWhileAddingUser);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IdentityRole> getRoleByRoleID(string roleID)
        {
            return await _roleManager.Roles.Where(x => x.Id == roleID).FirstOrDefaultAsync();
        }
        public async Task<IdentityRole> getRoleByRoleName(string roleName)
        {
            return await _roleManager.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync();
        }
        public async Task<UserDTO> getUserByEmailndPassword(loginReq req)
        {
            try
            {
                TblUser _req = await _context.Users.Where(x => x.Email == req.Email).FirstOrDefaultAsync();
                if (_req == null)
                {
                    throw new Exception(_exceptions.invalidEmail);
                }
                var result = await _signInManager.PasswordSignInAsync(_req, req.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    throw new Exception(_exceptions.invalidPassword);
                }
                if (_req.Active != EActivityStatus.Active)
                {
                    throw new Exception(_exceptions.deactiveUser);
                }

                return new UserDTO()
                {
                    email = req.Email,
                    designationID = (int)_req.Designation,
                    firstName = _req.FirstName,
                    lastName = _req.LastName
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserDTO> getUserByUsernameandPassword(Login_Req req)
        {
            try
            {
                TblUser user = await _context.Users.Where(x => x.UserName == req.username).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception(_exceptions.invalidEmail);
                }
                if (user.isAccountLocked)
                    throw new Exception(_exceptions.deactiveUser);

                tblFarmerProfile _req = new tblFarmerProfile();
                if (user.Designation == EDesignation.Farmer)
                {
                    _req = await _context.FarmerProfile.Include(x => x.User).Where(x => x.UserID == user.Id).FirstOrDefaultAsync();
                }

                var result = await _signInManager.PasswordSignInAsync(user, req.password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    await wrongPasswordCounter(user);

                    throw new Exception(_exceptions.invalidPassword);
                }
                if (user.Active != EActivityStatus.Active)
                {
                    throw new Exception(_exceptions.deactiveUser);
                }
                await emptyPasswordAttempts(user);

                return new UserDTO()
                {
                    email = req.username,
                    isApproved = _req.isApproved,
                    isOTPVerified = user.isOTPApproved.Value,
                    designationID = (int)user.Designation,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    userID = user.Id,
                    languageCode = _req.SelectedLangCode
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserDTO> isUsernameValid(forgetPassword_Req req)
        {
            return await _context.FarmerProfile.Include(x => x.User).Where(x => x.CNICNumber == req.cnicNumber).Select(x => new UserDTO
            {
                userID = x.User.Id,
                firstName = x.User.FirstName,
                lastName = x.User.LastName,
                designationID = (int)x.User.Designation,
                email = x.User.Email,
                isOTPVerified = x.User.isOTPApproved.Value
            }).FirstOrDefaultAsync();
        }
        public async Task<UserDTO> signupFarmer(farmerSignUp_Req req)
        {
            int isExist = await _context.Users.Where(x => x.UserName == req.username).CountAsync();

            if (isExist != 0)
                throw new Exception(_exceptions.usernameIsTaken);
            isExist = await _context.FarmerProfile.Where(x => x.CNICNumber == req.cnicNumber).CountAsync();
            if (isExist != 0)
                throw new Exception(_exceptions.cnicIsTaken);
            isExist = await _context.FarmerProfile.Where(x => x.NTNNumber == req.ntnNumber).CountAsync();
            if (isExist != 0)
                throw new Exception(_exceptions.ntnNumberIsTaken);
            isExist = await _context.FarmerProfile.Where(x => x.STRNNumber == req.strnNumber).CountAsync();
            if (isExist != 0)
                throw new Exception(_exceptions.strnIsTaken);
            isExist = await _context.FarmerProfile.Where(x => x.CellNumber == req.cellNumber).CountAsync();
            if (isExist != 0)
                throw new Exception(_exceptions.cellNumberIsTaken);
            else
            {
                Microsoft.AspNetCore.Identity.IdentityResult updateResult = new Microsoft.AspNetCore.Identity.IdentityResult();
                TblUser userReq = new TblUser
                {
                    FirstName = req.firstName,
                    LastName = req.lastName,
                    PhoneNumber = req.cellNumber,
                    Email = req.emailAddress,
                    UserName = req.username,
                    NormalizedUserName = req.username.ToUpper(),
                    Designation = EDesignation.Farmer,
                    isOTPApproved = false,
                    Active = EActivityStatus.Active,
                    SignupAgreementDateTime = DateTime.Now
                };
                updateResult = await _userManager.CreateAsync(userReq, req.password);
                if (updateResult.Succeeded)
                {
                    IdentityRole newRole = await getRoleByRoleName(EDesignation.Farmer.ToString());
                    await _userManager.AddToRoleAsync(userReq, newRole.Name);

                    userReq = await _context.Users.Where(x => x.UserName == req.username).FirstOrDefaultAsync();

                    if (userReq != null)
                    {
                        tblFarmerProfile profile = await generateUserProfile(req, userReq);
                        UserDTO resp = new UserDTO
                        {
                            email = userReq.Email,
                            firstName = userReq.FirstName,
                            lastName = userReq.LastName,
                            designationID = (int)userReq.Designation,
                            isOTPVerified = userReq.isOTPApproved.Value,
                            isApproved = profile.isApproved,
                            userID = userReq.Id
                        };
                        resp.languageCode = profile.SelectedLangCode;
                        return resp;
                    }
                    else
                    {
                        throw new Exception(_exceptions.errorOccuredWhileAddingUser);
                    }
                }
                else
                {
                    if (updateResult.Errors != null && updateResult.Errors.Count() > 0)
                    {
                        throw new Exception(updateResult.Errors.FirstOrDefault().Description);
                    }
                    else
                    {
                        throw new Exception(_exceptions.errorOccuredWhileAddingUser);
                    }
                }
            }
        }

        public async Task setOTP(string UserID, string OTPCode)
        {
            var user = await _context.Users.Where(x => x.Id == UserID).FirstOrDefaultAsync();
            if (user != null)
            {
                user.OTP = OTPCode;
                user.OTPExpiredOn = DateTime.Now.AddSeconds(60);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public async Task<bool> verifyOTP(string UserID, string OTPCode)
        {
            var user = await _context.Users.Where(x => x.Id == UserID).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.OTPExpiredOn < DateTime.Now)
                {
                    throw new Exception(_exceptions.expiredOTP);
                }
                if (user.OTP == OTPCode)
                {
                    user.isOTPApproved = true;
                    user.OTP = string.Empty;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> forgetPasswordOTP(string cnicNumber, string OTPCode)
        {
            bool isExist = false;
            var user = await _context.FarmerProfile.Include(x => x.User).Where(x => x.CNICNumber == cnicNumber && x.User.OTP == OTPCode).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.User.OTPExpiredOn < DateTime.Now)
                {
                    throw new Exception(_exceptions.expiredOTP);
                }
                //user.User.OTP = "";
                user.User.OTPExpiredOn = DateTime.Now.AddMinutes(15);
                await emptyPasswordAttempts(user.User);
                isExist = true;
            }
            else
            {
                throw new Exception(_exceptions.invalidOTP);
            }
            return isExist;
        }

        public async Task<UserDTO> getUserProfileByUserID(string UserID)
        {
            var farm = await _context.Farms.Where(x => x.UserID == UserID).OrderBy(x => x.FarmID).FirstOrDefaultAsync();

            var user = await _context.Users.Where(x => x.Id == UserID).FirstOrDefaultAsync();
            UserDTO resp = new UserDTO()
            {
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                userID = user.Id,
                username = user.UserName,
                designationID = (int)user.Designation,
                isOTPVerified = user.isOTPApproved.Value,
                hasFarms = (farm != null ? true : false),
                isFarmApprovalAcknowledged = farm != null ? farm.isFarmApprovalAcknowledged : false,
                applicationID = farm != null ? farm.ApplicationID.ToString().PadLeft(10, '0') : "0"
            };
            var userprofile = await _context.FarmerProfile.Where(x => x.UserID == UserID).FirstOrDefaultAsync();
            if (userprofile != null)
            {
                resp.cnic = userprofile.CNICNumber;
                resp.isApproved = userprofile.isApproved;
                resp.languageCode = userprofile.SelectedLangCode;
            }

            return resp;
        }

        public async Task changePassword(changePassword_Req req, string userID)
        {
            var user = await _context.Users.Where(x => x.Id == userID).FirstOrDefaultAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, req.currentPassword, req.password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description.ToString());
                }
            }
            else
            {
                throw new Exception(_exceptions.userNotFound);
            }
        }
        public async Task newPassword(newPassword_Req req)
        {
            var userProfile = await _context.FarmerProfile.Include(x => x.User).Where(x => x.CNICNumber == req.cnicNumber).FirstOrDefaultAsync();
            if (userProfile != null && userProfile.User != null)
            {

                if (userProfile.User.OTPExpiredOn < DateTime.Now)
                {
                    throw new Exception(_exceptions.expiredOTP);
                }
                if (userProfile.User.OTP == req.otpCode)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(userProfile.User);

                    var result = await _userManager.ResetPasswordAsync(userProfile.User, token, req.password);

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.FirstOrDefault().Description.ToString());
                    }
                    emptyPasswordAttempts(userProfile.User);
                }
                else
                {
                    throw new Exception(_exceptions.invalidOTP);
                }
            }
            else
            {
                throw new Exception(_exceptions.userNotFound);
            }

        }

        public async Task<bool> isUsernameExist(string username)
        {
            int isExist = await _context.Users.Where(x => x.UserName == username).CountAsync();
            if (isExist >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<UserDTO> getUserInfoByUsername(string username)
        {
            return await _context.Users.Where(x => x.UserName == username).Select(x => new UserDTO
            {
                email = x.Email,
                firstName = x.FirstName,
                lastName = x.LastName,
                designationID = (int)x.Designation,
                isOTPVerified = x.isOTPApproved.Value,
                userID = x.Id
            }).FirstOrDefaultAsync();

        }
        public async Task uploadSignupDocuments()
        {

        }
        public async Task<List<userotp_list>> getAllUsernameWithOTPs()
        {
            return await _context.Users.Where(x => x.OTP != "").Select(x => new userotp_list
            {
                otp = x.OTP,
                username = x.UserName
            }).ToListAsync();
        }

        public async Task<UserDTO> getUserInfoByCNIC(string CNIC)
        {
            return await _context.FarmerProfile.Include(x => x.User).Where(x => x.CNICNumber == CNIC).Select(x => new UserDTO
            {
                email = x.User.Email,
                firstName = x.User.FirstName,
                lastName = x.User.LastName,
                designationID = (int)x.User.Designation,
                isOTPVerified = x.User.isOTPApproved.Value,
                userID = x.User.Id,
                languageCode = x.SelectedLangCode
            }).FirstOrDefaultAsync();
        }
        public async Task approveFarmAccountByUserID(string UserID, string ApproverID)
        {
            var userInfo = await _context.FarmerProfile.Where(x => x.UserID == UserID).FirstOrDefaultAsync();
            if (userInfo != null)
            {
                userInfo.isApproved = true;
                userInfo.ApprovedDate = DateTime.Now;
                userInfo.ApprovedByID = ApproverID;
                _context.FarmerProfile.Update(userInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception(_exceptions.userNotFound);
            }
        }
        public async Task<getUserProfile_Resp> getUserProfile(string UserID)
        {
            getUserProfile_Resp resp = new getUserProfile_Resp();

            var query = from user in _context.Users
                        join profile in _context.FarmerProfile
                            on user.Id equals profile.UserID into userProfile
                        from up in userProfile.DefaultIfEmpty()
                        where user.Id == UserID
                        select new getUserProfile_Resp
                        {
                            firstName = user.FirstName,
                            lastName = user.LastName ?? "", // Handle nullable last name
                            dateOfBirth = up != null ? up.DateOfBirth : "", // Check if profile exists
                            fatherName = up != null ? up.FatherName : "", // Check if profile exists
                            cnicNumber = up != null ? up.CNICNumber : "", // Check if profile exists
                            ntnNumber = up != null ? up.NTNNumber : "", // Check if profile exists
                            strnNumber = up != null ? up.STRNNumber : "", // Check if profile exists
                            cellNumber = user.PhoneNumber,
                            emailAddress = user.Email,
                            ownedLandAcreage = up != null ? up.OwnedLand : "", // Check if profile exists
                            leasedLandAcreage = up != null ? up.LeasedLand : "" // Check if profile exists
                        };


            string queryString = query.ToQueryString();
            var userProfileData = await query.FirstOrDefaultAsync();

            return userProfileData;
        }
        private async Task<tblFarmerProfile> generateUserProfile(farmerSignUp_Req req, TblUser user)
        {
            try
            {
                tblFarmerProfile _req = new tblFarmerProfile()
                {
                    UserID = user.Id,
                    FatherName = req.fatherName,
                    CNICNumber = req.cnicNumber,
                    NTNNumber = req.ntnNumber,
                    STRNNumber = req.strnNumber,
                    OwnedLand = req.ownedLandAcreage.ToString(),
                    LeasedLand = req.leasedLandAcreage.ToString(),
                    DateOfBirth = req.dateOfBirth,
                    Address1 = req.address1,
                    Address2 = req.address2,
                    CityID = req.cityID,
                    DistrictID = req.districtID,
                    CellNumber = req.cellNumber,
                    //TehsilID = req.tehsilID,
                    isApproved = false,
                    SelectedLangCode = req.selectedLangCode
                };

                _req = _context.FarmerProfile.AddAsync(_req).Result.Entity;
                _context.SaveChanges();
                return _req;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task wrongPasswordCounter(TblUser user)
        {
            if (user.WrongPasswordAttempt == 2)
            {
                user.isAccountLocked = true;
            }
            user.WrongPasswordAttempt = (user.WrongPasswordAttempt + 1);


            _context.Update(user);
            _context.SaveChanges();

            if (user.isAccountLocked)
                throw new Exception(_exceptions.deactiveUser);
        }
        private async Task emptyPasswordAttempts(TblUser user)
        {
            user.WrongPasswordAttempt = 0;
            user.isAccountLocked = false;
            user.OTP = null;
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
