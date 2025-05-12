/*
   This class implements the IUserRepo interface and provides methods for user-related operations such as user registration, authentication, and password management.
*/

// Consider providing XML documentation comments for the class and its members
// to improve code readability and maintainability.
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly SignInManager<TblUser> _signInManager;
        private readonly RoleManager<TblRole> _roleManager;
        private readonly UserManager<TblUser> _userManager;
        private AmazonFarmerContext _context;

        // Constructor to initialize the UserRepo with an instance of the AmazonFarmerContext,
        // SignInManager, RoleManager, and UserManager
        public UserRepo(AmazonFarmerContext context, SignInManager<TblUser> signInManager, RoleManager<TblRole> roleManager, UserManager<TblUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Method to add a new user
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
                    throw new AmazonFarmerException(_exceptions.userAlreadyExist);
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
                    TblRole newRole = await getRoleByRoleID(req.RoleID);

                    //await _userManager.AddToRoleAsync(_req, newRole.Name);
                    await _userManager.AddToRoleAsync(temp, newRole.Name);
                }
                else
                {
                    if (updateResult.Errors != null && updateResult.Errors.Count() > 0)
                    {
                        throw new AmazonFarmerException(updateResult.Errors.FirstOrDefault().Description);
                    }
                    else
                    {
                        throw new AmazonFarmerException(_exceptions.errorOccuredWhileAddingUser);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Method to retrieve a role by role ID
        public async Task<TblRole> getRoleByRoleID(string roleID)
        {
            return await _roleManager.Roles.Where(x => x.Id == roleID).FirstOrDefaultAsync();
        }


        // Method to authenticate user by email and password
        public async Task<UserDTO> getUserByEmailndPassword(loginReq req)
        {
            try
            {
                TblUser _req = await _context.Users.Where(x => x.Email == req.Email).FirstOrDefaultAsync();
                if (_req == null)
                {
                    throw new AmazonFarmerException(_exceptions.invalidEmail);
                }
                var result = await _signInManager.PasswordSignInAsync(_req, req.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    throw new AmazonFarmerException(_exceptions.invalidPassword);
                }
                if (_req.Active != EActivityStatus.Active)
                {
                    throw new AmazonFarmerException(_exceptions.deactiveUser);
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

        // Method to retrieve device token by user ID
        public async Task<string> getDeviceTokenByUserID(string userID)
        {
            string deviceToken = string.Empty;
            if (!string.IsNullOrEmpty(userID))
            {
                deviceToken = await _context.Users.Where(x => x.Id == userID).Select(x => x.DeviceToken).FirstOrDefaultAsync();
            }
            return deviceToken;
        }

        // Method to check if username is valid for password recovery
        public async Task<UserDTO?> isUsernameValid(forgetPassword_Req req)
        {
            return await _context.Users.Include(x => x.FarmerProfile)
                    .Where(x => x.CNICNumber == req.cnicNumber
                    && x.FarmerProfile.FirstOrDefault().isApproved != EFarmerProfileStatus.Blocked)
                    .Select(x => new UserDTO
                    {
                        userID = x.Id,
                        firstName = x.FirstName,
                        lastName = x.LastName,
                        designationID = (int)x.Designation,
                        email = x.Email,
                        phone = x.PhoneNumber,
                        isOTPVerified = x.isOTPApproved.Value
                    }).FirstOrDefaultAsync();
        }

        // Method to set OTP for password recovery
        public async Task setOTP(string UserID, string OTPCode)
        {
            var user = await _context.Users.Where(x => x.Id == UserID).FirstOrDefaultAsync();
            if (user != null)
            {
                user.OTP = OTPCode;
                user.OTPExpiredOn = DateTime.UtcNow.AddMinutes(3);
                _context.Users.Update(user);
            }
        }

        // Method to verify OTP for password recovery
        public async Task<bool> verifyOTP(TblUser user)
        {
            user.isOTPApproved = true;
            user.OTP = string.Empty;
            _context.Users.Update(user);
            return true;
        }

        // Method to check OTP for forget password
        public async Task<tblFarmerProfile> forgetPasswordOTP(string cnicNumber, string OTPCode)
        {
            return await _context.FarmerProfile.Include(x => x.User).Where(x => x.User.CNICNumber == cnicNumber
               && x.User.OTP == OTPCode).FirstOrDefaultAsync();
        }
        // Method to check OTP for forget password
        public async Task<TblUser> forgetUserPasswordOTP(string cnicNumber, string OTPCode)
        {
            return await _context.Users.Include(x => x.FarmerProfile).Where(x => x.CNICNumber == cnicNumber
               && x.OTP == OTPCode).FirstOrDefaultAsync();
        }

        // Method to retrieve user profile by user ID
        public async Task<UserDTO> getUserProfileByUserID(string UserID)
        {
            var farm = await _context.Farms.Where(x => x.UserID == UserID && x.Status != EFarmStatus.Deleted).ToListAsync();
            var user = await _context.Users.Include(x => x.FarmerRoles).Where(x => x.Id == UserID).FirstOrDefaultAsync();
            int isApprovalStatus = 0;
            bool approvedByTSO = false;
            bool approvedByRSM = false;
            bool approvedByPatwari = false;
            if (farm != null && farm.Any())
            {
                var firstApplicationID = farm.OrderBy(x => x.FarmID).First().ApplicationID;
                var relevantFarms = farm.Where(x => x.ApplicationID == firstApplicationID).OrderBy(x => x.FarmID);
                if (relevantFarms.Any(x => x.Status == EFarmStatus.PendingforRSM))
                {
                    approvedByTSO = true;
                }
                if (relevantFarms.Any(x => x.Status == EFarmStatus.PendingForPatwari))
                {
                    approvedByTSO = true;
                    approvedByRSM = true;
                }

                if (relevantFarms.Any(x => x.Status == EFarmStatus.Approved))
                {
                    approvedByTSO = true;
                    approvedByRSM = true;
                    approvedByPatwari = true;
                    isApprovalStatus = (int)EFarmStatus.Approved;
                }
                else if (relevantFarms.Any(x => x.Status == EFarmStatus.Rejected))
                {
                    isApprovalStatus = (int)EFarmStatus.Rejected;
                }
                else if (relevantFarms.Any(x => x.Status == EFarmStatus.Deleted))
                {
                    isApprovalStatus = (int)EFarmStatus.Deleted;
                }
                else if (relevantFarms.Any(x => x.Status == EFarmStatus.SendBack))
                {
                    isApprovalStatus = (int)EFarmStatus.SendBack;
                }
                else if (relevantFarms.Any())
                {
                    isApprovalStatus = (int)relevantFarms.First().Status;
                }


            }
            else
            {
                isApprovalStatus = 0;
            }
            UserDTO resp = new UserDTO()
            {
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                userID = user.Id,
                username = user.UserName,
                designationID = user.Designation == null ? 0 : (int)user.Designation,
                isOTPVerified = user.isOTPApproved.Value,
                hasFarms = (farm != null && farm.Count(x => x.Status != EFarmStatus.Draft || x.Status != EFarmStatus.Deleted) > 0 ? true : false),
                isFarmApprovalAcknowledged = farm != null && farm.Count(x => x.Status == EFarmStatus.Approved) > 0 ? farm.Where(x => x.Status == EFarmStatus.Approved).OrderBy(x => x.UpdatedOn).FirstOrDefault().isFarmApprovalAcknowledged : false,
                //enableEdit = ((farm != null && farm.Count(x => x.Status == EFarmStatus.Draft) == farm.Count()) ? true : false),
                applicationID = farm != null && farm.Any() ? farm.First().ApplicationID.ToString().PadLeft(10, '0') : "0",
                isApprovalStatus = isApprovalStatus,
                approvedByTSO = approvedByTSO,
                approvedByRSM = approvedByRSM,
                approvedByPatwari = approvedByPatwari,
            };
            var userprofile = await _context.FarmerProfile.Where(x => x.UserID == UserID).FirstOrDefaultAsync();
            if (userprofile != null)
            {
                resp.cnic = userprofile.CNICNumber;
                //resp.isApproved = (userprofile.isApproved == EFarmerProfileStatus.Approved ? true : false);
                resp.languageCode = userprofile.SelectedLangCode;
                resp.applicationMessage = "Changes required";
            }
            else
            {
                resp.languageCode = "EN";
            }
            return resp;
        }

        // Method to set new password after password recovery
        public async Task newPassword(newPassword_Req req)
        {


        }

        // Method to check if username exists
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

        // Method to get user info by username
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

        // Method to upload signup documents
        public async Task uploadSignupDocuments()
        {

        }

        // Method to get all usernames with OTPs
        public async Task<List<userotp_list>> getAllUsernameWithOTPs()
        {
            return await _context.Users.Where(x => x.OTP != "").Select(x => new userotp_list
            {
                otp = x.OTP,
                username = x.UserName
            }).ToListAsync();
        }

        // Method to get user info by CNIC
        public async Task<UserDTO?> getUserInfoByCNIC(string CNIC)
        {
            return await _context.Users.Include(x => x.FarmerProfile)
                .Where(x => x.CNICNumber == CNIC
                && x.FarmerProfile.FirstOrDefault().isApproved != EFarmerProfileStatus.Blocked).Select(x => new UserDTO
                {
                    email = x.Email,
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    designationID = (int)x.Designation,
                    isOTPVerified = x.isOTPApproved.Value,
                    userID = x.Id,
                    languageCode = x.FarmerProfile != null && x.FarmerProfile.Count() >= 1 ? x.FarmerProfile.First().SelectedLangCode : "EN",
                    phone = x.PhoneNumber
                }).FirstOrDefaultAsync();
        }

        // Method to approve farm account by user ID
        public async Task approveFarmAccountByUserID(tblFarmerProfile userInfo, string ApproverID)
        {
            if (userInfo != null)
            {
                userInfo.isApproved = EFarmerProfileStatus.Pending;
                userInfo.ApprovedDate = DateTime.UtcNow;
                userInfo.ApprovedByID = ApproverID;
                _context.FarmerProfile.Update(userInfo);
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.userNotFound);
            }
        }

        public async Task<tblFarmerProfile?> getFarmerProfileByUserID(string UserID)
        {
            return await _context.FarmerProfile.Include(x => x.User).Where(x => x.UserID == UserID
            && x.isApproved != EFarmerProfileStatus.Blocked).FirstOrDefaultAsync();
        }

        // Method to get user profile
        public async Task<TblUser> getUserProfile(string UserID, string languageCode)
        {
            getUserProfile_Resp resp = new getUserProfile_Resp();
            var query = _context.Users
                .Include(x => x.FarmerProfile).ThenInclude(x => x.City).ThenInclude(x => x.CityLanguages)
                .Include(x => x.UserAttachments)
                    .ThenInclude(x => x.Attachment)
                        .ThenInclude(x => x.AttachmentTypes)
                .Include(x => x.FarmerProfile).ThenInclude(x => x.District).ThenInclude(x => x.DistrictLanguages)

                .Where(x => x.Id == UserID);


            var userProfileData = await query.FirstOrDefaultAsync();

            return userProfileData;
        }
        // Method to generate user profile
        public async Task<tblFarmerProfile> generateUserProfile(farmerSignUp_Req req, TblUser user)
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
                    isApproved = EFarmerProfileStatus.Pending,
                    SelectedLangCode = req.selectedLangCode,
                };

                await _context.FarmerProfile.AddAsync(_req);
                return _req;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to update device token
        public async Task updateDeviceToken(TblUser user, string deviceToken)
        {
            try
            {
                user.DeviceToken = deviceToken;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to empty password attempts
        public async Task emptyPasswordAttempts(TblUser user)
        {
            try
            {
                user.AccessFailedCount = 0;
                _context.Users.Update(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to count wrong password attempts
        public async Task wrongPasswordCounter(TblUser user)
        {
            try
            {
                user.AccessFailedCount++;
                _context.Users.Update(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to lock user account
        public async Task lockUserAccount(TblUser user)
        {
            user.isAccountLocked = true;
            _context.Users.Update(user);
        }

        // Method to unlock user account
        public async Task unlockUserAccount(TblUser user)
        {
            user.isAccountLocked = false;
            _context.Users.Update(user);
        }
        public async Task approverFarmerProfile(tblFarmerProfile user)
        {
            _context.FarmerProfile.Update(user);
        }
        public async Task<TblUser?> getUserByUserName(string UserName)
        {
            return await _context.Users.Include(x => x.FarmerRoles).ThenInclude(x => x.Role).Where(x => x.UserName == UserName).FirstOrDefaultAsync();
        }
        public async Task<TblUser> getUserByUserID(string userID)
        {
            return await _context.Users
                .Include(x => x.FarmerProfile)
                    .ThenInclude(p => p.City)
                .Include(x => x.UserAttachments.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(ua => ua.Attachment)
                        .ThenInclude(a => a.AttachmentTypes)
                .Where(x => x.Id == userID).FirstOrDefaultAsync();
        }

        public async Task<tblFarmerProfile?> GetUserProfileByValidation(string cnicNumber, string cellNumber)
        {
            return await _context.FarmerProfile.Where(x =>
                                        (x.CNICNumber == cnicNumber ||
                                        x.CellNumber == cellNumber) &&
                                        x.isApproved != EFarmerProfileStatus.Blocked
                                        )
                                        .FirstOrDefaultAsync();
        }

        public async Task<tblFarmerProfile?> GetUserProfileByValidation(string cnicNumber, string cellNumber,
            string ntnNumber)
        {
            return await _context.FarmerProfile.Where(x =>
                                        (x.CNICNumber == cnicNumber ||
                                        x.NTNNumber == ntnNumber ||
                                        x.CellNumber == cellNumber) &&
                                        x.isApproved != EFarmerProfileStatus.Blocked
                                        )
                                        .FirstOrDefaultAsync();
        }

        public async Task<tblFarmerProfile?> GetUserProfileByValidation(string cnicNumber, string cellNumber,
            string? ntnNumber = null,
            string? strnNumber = null)
        {
            return await _context.FarmerProfile.Where(x =>
                                    (x.CNICNumber == cnicNumber ||
                                    x.CellNumber == cellNumber ||
                                    (ntnNumber != null && x.NTNNumber == ntnNumber) ||
                                    (strnNumber != null && x.STRNNumber == strnNumber)) &&
                                    x.isApproved != EFarmerProfileStatus.Blocked
                                    )
                                    .FirstOrDefaultAsync();
        }
        public async Task<TblUser?> getFarmerProfileByUserCNIC(string cnicNumber)
        {
            return await _context.Users.Include(x => x.FarmerProfile).Where(x =>
                                        x.CNICNumber == cnicNumber &&
                                        x.FarmerProfile.FirstOrDefault().isApproved != EFarmerProfileStatus.Blocked
                                        )
                                        .FirstOrDefaultAsync();
        }
        public async Task<getFarmerInfoResp> getFarmerInfoByFarmerIDAndLanguageCode(string userID, string languageCode)
        {
            return await _context.Users
                .Include(x => x.FarmerProfile)
                .Include(x => x.UserAttachments)
                    .ThenInclude(x => x.Attachment)
                        .ThenInclude(x => x.AttachmentTypes)
                .Include(x => x.farms).ThenInclude(f => f.FarmAttachments)
                .Where(x => x.Id == userID)
                .Select(user => new getFarmerInfoResp
                {
                    cityID = user.FarmerProfile.FirstOrDefault().CityID,
                    city = user.FarmerProfile.FirstOrDefault().City.CityLanguages.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Translation,
                    districtID = user.FarmerProfile.FirstOrDefault().DistrictID,
                    district = user.FarmerProfile.FirstOrDefault().District.DistrictLanguages.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Translation,
                    firstName = user.FirstName,
                    status = (int)user.FarmerProfile.First().isApproved,
                    address1 = user.FarmerProfile.First().Address1,
                    address2 = user.FarmerProfile.First().Address2,
                    lastName = user.LastName ?? "", // Handle nullable last name
                    dateOfBirth = user.FarmerProfile.Any() ? user.FarmerProfile.First().DateOfBirth : "", // Check if profile exists
                    fatherName = user.FarmerProfile.Any() ? user.FarmerProfile.First().FatherName : "", // Check if profile exists
                    cnicNumber = user.FarmerProfile.Any() ? user.FarmerProfile.First().CNICNumber : "", // Check if profile exists
                    ntnNumber = user.FarmerProfile.Any() ? user.FarmerProfile.First().NTNNumber : "", // Check if profile exists
                    strnNumber = user.FarmerProfile.Any() ? user.FarmerProfile.First().STRNNumber : "", // Check if profile exists
                    cellNumber = user.PhoneNumber,
                    emailAddress = user.Email,
                    ownedLandAcreage = user.FarmerProfile.Any() ? user.FarmerProfile.First().OwnedLand : "", // Check if profile exists
                    leasedLandAcreage = user.FarmerProfile.Any() ? user.FarmerProfile.First().LeasedLand : "", // Check if profile exists
                    cnic = user.UserAttachments.Any() ? user.UserAttachments.Where(x =>
                    x.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document
                    ).Select(x => new uploadAttachmentResp
                    {
                        id = x.tblAttachmentID,
                        name = x.Attachment.Name,
                        guid = x.Attachment.Guid.ToString(),
                        type = x.Attachment.FileType
                    }).ToList() : null, // Check if profile exists
                    ntn = user.UserAttachments.Any() ? user.UserAttachments.Where(x =>
                    x.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document
                    ).Select(x => new uploadAttachmentResp
                    {
                        id = x.tblAttachmentID,
                        name = x.Attachment.Name,
                        guid = x.Attachment.Guid.ToString(),
                        type = x.Attachment.FileType
                    }).ToList() : null, // Check if profile exists
                    messages = user.farms.Where(x => x.RevertedReason != null && x.Status != EFarmStatus.Deleted).Select(x => x.RevertedReason).ToList(),
                    farms = user.farms.Where(x => x.Status != EFarmStatus.Deleted).Select(farm => new farmDetail_Resp()
                    {
                        cityID = farm.CityID,
                        districtID = farm.DistrictID,
                        tehsilID = farm.TehsilID,
                        farmID = farm.FarmID,
                        farmName = farm.FarmName,
                        acreage = farm.Acreage,
                        address1 = farm.Address1,
                        address2 = farm.Address2,
                        latitude = farm.latitude,
                        longitude = farm.longitude,
                        city = farm.City.CityLanguages.Where(x => x.LanguageCode == languageCode).First().Translation,
                        district = farm.District.DistrictLanguages.Where(x => x.LanguageCode == languageCode).First().Translation,
                        tehsil = farm.Tehsil.TehsilLanguagess.Where(x => x.LanguageCode == languageCode).First().Translation,
                        isApproved = farm.isApproved,
                        isLeased = farm.isLeased,
                        isPrimary = farm.isPrimary,
                        revertedReason = farm.RevertedReason,
                        sapFarmID = farm.SAPFarmCode.PadLeft(10, '0'),
                        status = (int)farm.Status,
                        attachmentGUID = farm.FarmAttachments.Where(fa => fa.Status == EActivityStatus.Active).Select(x => new uploadAttachmentResp
                        {
                            id = x.Attachment.ID,
                            type = x.Attachment.FileType,
                            name = x.Attachment.Name,
                            guid = x.Attachment.Guid.ToString()
                        }).ToList(),
                        farmerProfile = new farmerProfileDTO
                        {
                            firstName = farm.Users.FirstName,
                            LastName = farm.Users.LastName,
                            email = farm.Users.Email,
                            phone = farm.Users.PhoneNumber,
                            dateOfBirth = farm.Users.FarmerProfile.FirstOrDefault().DateOfBirth,
                            fatherName = farm.Users.FarmerProfile.FirstOrDefault().FatherName,
                            strnNumber = farm.Users.FarmerProfile.FirstOrDefault().STRNNumber,
                            cnicNumber = farm.Users.FarmerProfile.FirstOrDefault().CNICNumber,
                            cnicAttachment = farm.Users.UserAttachments
                                    .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document)
                                    .Select(y => new uploadAttachmentResp
                                    {
                                        id = y.Attachment.ID,
                                        type = y.Attachment.FileType,
                                        name = y.Attachment.Name,
                                        guid = y.Attachment.Guid.ToString()
                                    }).ToList(),
                            ntnNumber = farm.Users.FarmerProfile.FirstOrDefault().NTNNumber,
                            ntnAttachment = farm.Users.UserAttachments
                                    .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document)
                                    .Select(y => new uploadAttachmentResp
                                    {
                                        id = y.Attachment.ID,
                                        type = y.Attachment.FileType,
                                        name = y.Attachment.Name,
                                        guid = y.Attachment.Guid.ToString()
                                    }).ToList(),
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
        public async Task<TblUser> getFarmerInfoByFarmerIDAndLanguageCodeUdpated(string userID, string languageCode)
        {

            return await _context.Users
                .Include(x => x.FarmerProfile).ThenInclude(fp => fp.City).ThenInclude(c => c.CityLanguages.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.FarmerProfile).ThenInclude(fp => fp.District).ThenInclude(c => c.DistrictLanguages.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.UserAttachments.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(x => x.Attachment)
                        .ThenInclude(x => x.AttachmentTypes)
                .Include(x => x.farms.Where(x => x.Status != EFarmStatus.Deleted)).ThenInclude(f => f.FarmAttachments.Where(x => x.Status == EActivityStatus.Active)).ThenInclude(fa => fa.Attachment)
                .Include(x => x.farms.Where(x => x.Status != EFarmStatus.Deleted)).ThenInclude(f => f.District).ThenInclude(d => d.DistrictLanguages.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.farms.Where(x => x.Status != EFarmStatus.Deleted)).ThenInclude(f => f.City).ThenInclude(d => d.CityLanguages.Where(x => x.LanguageCode == languageCode))
                .Include(x => x.farms.Where(x => x.Status != EFarmStatus.Deleted)).ThenInclude(f => f.Tehsil).ThenInclude(d => d.TehsilLanguagess.Where(x => x.LanguageCode == languageCode))
                .Where(x => x.Id == userID).FirstOrDefaultAsync();
        }
        public async Task updateUser(TblUser user)
        {
            _context.Users.Update(user);
        }
        public async Task updateFarmerProfile(tblFarmerProfile userProfile)
        {
            _context.FarmerProfile.Update(userProfile);
        }
        public async Task updateSelectedLanguage(tblFarmerProfile profile, string languageCode)
        {
            profile.SelectedLangCode = languageCode;
            _context.FarmerProfile.Update(profile);
            await _context.SaveChangesAsync();
        }
        public async Task<TblUser> getFarmerByFarmApplicationID(int applicationID)
        {
            return await _context.Users.Include(x => x.FarmerProfile).Include(x => x.farms).Where(x => x.farms.Any(x => x.ApplicationID == applicationID)).FirstOrDefaultAsync();
        }


        public async Task<List<int>> GetDistrictIDsForTSO(string userId)
        {
            return await _context.EmployeeDistrictAssignments
                .Where(e => e.UserID == userId && e.Status == EActivityStatus.Active)
                 .Select(a => a.DitrictID).ToListAsync();
        }

        public async Task<List<TblUser>> getTSOsByDistrictIDs(List<int> districtIds)
        {
            return await _context.EmployeeDistrictAssignments
                .Include(x => x.User)
                .Where(d => districtIds.Contains(d.DitrictID))
                .Select(d => d.User).ToListAsync();
        }
        public async Task<List<TblUser>> getTSOsByDistrictIDsForHelp(List<int> districtIds)
        {
            return await _context.Users
                .Include(x => x.EmployeeDistricts.Where(d=>districtIds.Contains(d.DitrictID) && d.Status == EActivityStatus.Active))
                .ThenInclude(x=>x.District)
                .Where(u => u.EmployeeDistricts.Count() > 0).ToListAsync();
        }

        public async Task<List<int>> GetRegionIDsForRSM(string userId)
        {
            List<int> regionIds = await _context.EmployeeRegionAssignments
                .Where(e => e.UserID == userId && e.Status == EActivityStatus.Active)
                 .Select(a => a.RegionID).ToListAsync();

            return await _context.District.Where(d => regionIds.Contains(d.RegionId.Value)).Select(d => d.ID).ToListAsync();
        }

        public async Task<TblUser> getUserByPlanID(int planID)
        {
            return await _context.Users
                .Include(x=>x.FarmerProfile)
                .Include(x => x.plans)
                .Include(x=>x.farms)
                .Where(a => a.plans.Where(c => c.ID == planID).Count() > 0)
                .FirstOrDefaultAsync();
        }
        public async Task<List<TblUser>> getRSMsByFarmID(int farmID)
        {
            return await _context.Users
                .Include(x => x.EmployeeRegions).ThenInclude(x => x.Region).ThenInclude(x => x.Districts).ThenInclude(x => x.farms)
                .Where(u => u.EmployeeRegions
                        .Where(er => er.Region.Districts.Where(d => d.farms
                            .Where(f => f.FarmID == farmID).Count() > 0)
                        .Count() > 0)
                .Count() > 0)
                .ToListAsync();
        }
        public async Task<List<TblUser>> getTSOsByFarmID(int farmID)
        {
            return await _context.Users
                .Include(x => x.EmployeeDistricts)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.farms)
                .Where(x => x.EmployeeDistricts
                    .Where(b => b.District.farms.Where(
                        x => x.FarmID == farmID)
                    .Count() > 0)
                .Count() > 0)
                .ToListAsync();
        }
        public async Task<List<TblUser>> getNSMsByFarmID(int farmID)
        {
            return await _context.Users.Where(x => x.Designation == EDesignation.National_Sales_Manager).ToListAsync();
        }

        public void RemoveActiveToken(ActiveToken token)
        {
            _context.ActiveTokens.Remove(token);
        }

        public async Task<List<ActiveToken>> GetActiveTokensForUser(string userId)
        {
            return await _context.ActiveTokens.Where(a => a.UserId == userId).ToListAsync();
        }

        public void AddActiveToken(ActiveToken token)
        {
            _context.ActiveTokens.Add(token);
        }
        public async Task<TblUser> getUserByUsername(string username, string phoneNumber)
        {
            return await _context.Users.Where(x => x.UserName == username || x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }
        public async Task<TblUser> getUserByUsername(string username, string phoneNumber, string emailAddress)
        {
            return await _context.Users.Where(x => x.UserName == username || x.PhoneNumber == phoneNumber || x.Email == emailAddress).FirstOrDefaultAsync();
        }
        public IQueryable<TblUser> getUsers()
        {
            return _context.Users;
        }

        public async Task<TblUser> getUserDetailByUserID(string userID)
        {
            return await _context.Users
                .Include(x => x.FarmerProfile)
                    .ThenInclude(p => p.City)
                .Include(x => x.UserAttachments.Where(x => x.Status == EActivityStatus.Active))
                    .ThenInclude(ua => ua.Attachment)
                        .ThenInclude(a => a.AttachmentTypes)
                .Include(x=>x.EmployeeDistricts)
                    .ThenInclude(x=>x.District)
                .Include(x=>x.EmployeeRegions)
                    .ThenInclude(x=>x.Region)
                .Where(x => x.Id == userID).FirstOrDefaultAsync();
        }


    }
}
