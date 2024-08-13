using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using ChangeCustomer;
using FirebaseAdmin.Auth;
using Google.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Google.Cloud.Vision.V1.ProductSearchResults.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AmazonFarmerException = AmazonFarmer.Core.Application.Exceptions.AmazonFarmerException;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing farmer-related operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly NotificationService _notificationService;
        private readonly accuWeatherExtension _accuWeatherService;
        private readonly UserManager<TblUser> _userManager;
        private readonly RoleManager<TblRole> _roleManager;
        private readonly SignInManager<TblUser> _signInManager;
        private readonly AmazonFarmerContext _dbContext;
        private WsdlConfig _wsdlConfig;
        /// <summary>
        /// Constructor for initializing the FarmerController.
        /// </summary>
        /// <param name="repoWrapper">The repository wrapper instance for accessing data.</param>
        public FarmerController(IRepositoryWrapper repoWrapper,
            NotificationService notificationService, accuWeatherExtension accuWeatherService,
            UserManager<TblUser> userManager, RoleManager<TblRole> roleManager,
            SignInManager<TblUser> signInManager,
            AmazonFarmerContext dbContext,
            IOptions<WsdlConfig> wsdlConfig
            )
        {
            _repoWrapper = repoWrapper;
            _notificationService = notificationService;
            _accuWeatherService = accuWeatherService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _wsdlConfig = wsdlConfig.Value;
        }

        /// <summary>
        /// Endpoint for user signup.
        /// </summary>
        /// <param name="req">Request object containing user signup details.</param>
        /// <returns>Returns an API response containing the result of the signup operation.</returns>
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<APIResponse> signup(farmerSignUp_Req req)
        {
            APIResponse resp = new APIResponse();
            VerifySignUpRequest(req);
            TblUser userReq = new TblUser();
            if (matchPasswordCriteria(req.password))
            {
                TblUser? VerifyUser = await _repoWrapper.UserRepo.getUserByUserName(req.username);
                UserDTO user = new UserDTO();

                if (VerifyUser != null)
                    throw new AmazonFarmerException(_exceptions.usernameIsTaken);

                tblFarmerProfile? profiles = null;
                if (!string.IsNullOrEmpty(req.ntnNumber) && string.IsNullOrEmpty(req.strnNumber))
                {
                    profiles = await _repoWrapper.UserRepo.GetUserProfileByValidation(
                    req.cnicNumber, req.cellNumber, ntnNumber: req.ntnNumber);
                }
                else if (!string.IsNullOrEmpty(req.strnNumber) && string.IsNullOrEmpty(req.ntnNumber))
                {
                    profiles = await _repoWrapper.UserRepo.GetUserProfileByValidation(
                    req.cnicNumber, req.cellNumber, strnNumber: req.strnNumber);
                }
                else if (!string.IsNullOrEmpty(req.strnNumber) && !string.IsNullOrEmpty(req.ntnNumber))
                {
                    profiles = await _repoWrapper.UserRepo.GetUserProfileByValidation(
                    req.cnicNumber, req.cellNumber, ntnNumber: req.ntnNumber, strnNumber: req.strnNumber);
                }
                else
                {
                    profiles = await _repoWrapper.UserRepo.GetUserProfileByValidation(
                    req.cnicNumber, req.cellNumber);
                }


                if (profiles != null)
                {
                    if (profiles.CNICNumber == req.cnicNumber)
                        throw new AmazonFarmerException(_exceptions.cnicIsTaken);
                    if (!string.IsNullOrEmpty(req.ntnNumber))
                    {
                        if (profiles.NTNNumber == req.ntnNumber)
                            throw new AmazonFarmerException(_exceptions.ntnNumberIsTaken);
                    }
                    if (!string.IsNullOrEmpty(req.strnNumber))
                    {
                        if (profiles.STRNNumber == req.strnNumber)
                            throw new AmazonFarmerException(_exceptions.strnIsTaken);
                    }
                    if (profiles.CellNumber == req.cellNumber)
                        throw new AmazonFarmerException(_exceptions.cellNumberIsTaken);
                }
                //else
                //{
                IdentityResult updateResult = new IdentityResult();
                userReq = new TblUser
                {
                    FirstName = req.firstName,
                    LastName = req.lastName,
                    CNICNumber = req.cnicNumber,
                    PhoneNumber = req.cellNumber,
                    Email = req.emailAddress,
                    UserName = req.username,
                    NormalizedUserName = req.username.ToUpper(),
                    isOTPApproved = false,
                    Active = EActivityStatus.Active,
                    SignupAgreementDateTime = DateTime.UtcNow
                };
                updateResult = await _userManager.CreateAsync(userReq, req.password);

                if (updateResult.Succeeded)
                {
                    TblRole? newRole = await getRoleByRoleName(ERoles.Farmer.ToString());

                    await _userManager.AddToRoleAsync(userReq, newRole.Name);

                    tblFarmerProfile profile = await _repoWrapper.UserRepo.generateUserProfile(req, userReq);
                    //await _repoWrapper.SaveAsync();
                    user = new UserDTO
                    {
                        email = userReq.Email,
                        firstName = userReq.FirstName,
                        lastName = userReq.LastName,
                        //designationID = userReq.Designation == null ? 0 : (int)userReq.Designation,
                        isOTPVerified = userReq.isOTPApproved.Value,
                        //isApproved = profile.isApproved == EFarmerProfileStatus.Approved ? true : false,
                        userID = userReq.Id
                    };
                    user.languageCode = profile.SelectedLangCode;

                    await _repoWrapper.SaveAsync();
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
                //}

                if (user != null)
                {
                    userReq = await _repoWrapper.UserRepo.getUserByUserID(user.userID);
                    // Upload CNIC attachments
                    await uploadSignupAttachment(req.cnicAttachments, user.userID, EAttachmentType.User_CNIC_Document, userReq);

                    // Upload NTN attachments
                    await uploadSignupAttachment(req.ntnAttachments, user.userID, EAttachmentType.User_NTN_Document, userReq);

                    //await _repoWrapper.SaveAsync();
                    // Generate OTP code
                    string OTPCode = OTPExtension.GenerateOTP();

                    NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OTP, "EN");
                    if (notificationDTO != null)
                    {
                        List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = req.emailAddress, Name = req.firstName } },
                                    Subject =  notificationDTO.title,
                                    Message= notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = req.cellNumber, Name = req.firstName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                }
                            };

                        NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                        replacementDTO.OTP = OTPCode;
                        replacementDTO.UserName = userReq.FirstName;

                        await _notificationService.SendNotifications(notifications, replacementDTO);
                    }

                    // Set OTP for the user
                    await _repoWrapper.UserRepo.setOTP(user.userID, OTPCode);
                    await _repoWrapper.SaveAsync();

                    TblUser? dbUser = await _repoWrapper.UserRepo.getUserByUserName(req.username);

                    var userRoles = await _userManager.GetRolesAsync(dbUser);
                    var authClaims = new List<Claim>();

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role.ToString(), userRole));
                    }
                    List<ActiveToken> existingTokens = await _repoWrapper.UserRepo.GetActiveTokensForUser(user.userID);
                    if (existingTokens != null && existingTokens.Count() > 0)
                    {
                        foreach (ActiveToken existingToken in existingTokens)
                        {
                            _repoWrapper.UserRepo.RemoveActiveToken(existingToken);
                        }
                    }
                    int TokenExpiration = Convert.ToInt32(ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays"));
                    // Generate JWT token
                    TokenResponse token = JWTService.GenerateJwt(
                        user,
                        ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                        ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                        TokenExpiration,
                        authClaims
                    );
                    ActiveToken newToken = new()
                    {
                        CreatedAt = DateTime.UtcNow,
                        Expiration = DateTime.UtcNow.AddDays(TokenExpiration),
                        Token = token.Token,
                        UserId = user.userID,
                    };
                    _repoWrapper.UserRepo.AddActiveToken(newToken);
                    await _repoWrapper.SaveAsync();
                    // Prepare response
                    resp.response = token;
                    resp.message = "OTP Sent";
                }


            }

            return resp;
        }

        [HttpGet("getHelp")]
        public async Task<APIResponse> getHelp()
        {
            APIResponse resp = new APIResponse();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            tblFarmerProfile farmer = await _repoWrapper.UserRepo.getFarmerProfileByUserID(userID);
            List<TblUser> employees = await _repoWrapper.UserRepo.getTSOsByDistrictIDs([farmer.DistrictID]);
            resp.response = employees.Select(e => new HelpDTO_Resp
            {
                name = string.Concat(e.FirstName, ' ', e.LastName),
                email = e.Email,
                phone = e.PhoneNumber,
                designation = ConfigExntension.GetEnumDescription(e.Designation.Value)
            }).ToList();
            return resp;
        }

        // Method to retrieve a role by role name
        private async Task<TblRole?> getRoleByRoleName(string roleName)
        {
            return await _roleManager.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync();
        }
        private void VerifySignUpRequest(farmerSignUp_Req req)
        {
            // Check if the app version is valid
            if (req.appVersion != Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
                throw new AmazonFarmerException(_exceptions.invalidAppVersion);
            // Check if the first name is provided
            else if (string.IsNullOrEmpty(req.firstName))
                throw new AmazonFarmerException(_exceptions.firstnameRequired);
            // Check if the CNIC number is provided and valid
            else if (string.IsNullOrEmpty(req.cnicNumber) || req.cnicNumber.Replace("-", "").Length != 13)
                throw new AmazonFarmerException(_exceptions.cnicNumberRequired);
            // Check if the platform is provided
            else if (string.IsNullOrEmpty(req.platform))
                throw new AmazonFarmerException(_exceptions.platformRequired);
            // Check if the date of birth is provided
            else if (string.IsNullOrEmpty(req.dateOfBirth))
                throw new AmazonFarmerException(_exceptions.dateOfBirthRequired);
            // Check if the cell number is provided
            else if (string.IsNullOrEmpty(req.cellNumber))
                throw new AmazonFarmerException(_exceptions.cellNumberRequired);
            // Check if the email address is provided
            else if (string.IsNullOrEmpty(req.emailAddress))
                throw new AmazonFarmerException(_exceptions.emailRequired);
            // Check if the STRN number is provided
            //else if (string.IsNullOrEmpty(req.strnNumber))
            //throw new AmazonFarmerException(_exceptions.strnNumberRequired);
            // Check if the NTN number is provided
            //else if (string.IsNullOrEmpty(req.ntnNumber))
            //throw new AmazonFarmerException(_exceptions.ntnNumberRequired);
            // Check if CNIC attachments are provided
            else if (req.cnicAttachments == null || req.cnicAttachments.Count() <= 0)
                throw new AmazonFarmerException(_exceptions.attachmentRequired);
            // Check if NTN attachments are provided
            //else if (req.ntnAttachments == null || req.ntnAttachments.Count() <= 0)
            //throw new AmazonFarmerException(_exceptions.attachmentRequired);
            // Check if father's name is provided
            else if (string.IsNullOrEmpty(req.fatherName))
                throw new AmazonFarmerException(_exceptions.fatherNameRequired);
            // Check if address line 1 is provided
            else if (string.IsNullOrEmpty(req.address1))
                throw new AmazonFarmerException(_exceptions.addressRequired);
            // Check if city ID is provided
            else if (req.cityID == null || req.cityID == 0)
                throw new AmazonFarmerException(_exceptions.cityRequired);
            // Check if district ID is provided
            else if (req.districtID == null || req.districtID == 0)
                throw new AmazonFarmerException(_exceptions.districtRequired);
            // Check if username is provided
            else if (string.IsNullOrEmpty(req.username))
                throw new AmazonFarmerException(_exceptions.usernameRequired);
            // Check if leased land acreage is within valid range
            else if (req.leasedLandAcreage > 100000)
                throw new AmazonFarmerException(_exceptions.leasedLandMinMaxLengthExteeds);
            // Check if leased land acreage is within valid range
            else if (req.ownedLandAcreage > 100000)
                throw new AmazonFarmerException(_exceptions.ownedLandMinMaxLengthExteeds);
            else if (req.leasedLandAcreage <= 0 && req.ownedLandAcreage <= 0)
                throw new AmazonFarmerException(_exceptions.leasedAndOwnedLandCannotBeZero);
            // Check if password is provided
            else if (string.IsNullOrEmpty(req.password))
                throw new AmazonFarmerException(_exceptions.passwordRequired);
            // Check if password confirmation is provided
            else if (string.IsNullOrEmpty(req.confirmPassword))
                throw new AmazonFarmerException(_exceptions.confirmPasswordRequired);
            // Check if terms are accepted
            else if (!req.isTermsAccepted)
                throw new AmazonFarmerException(_exceptions.isTermsAcceptRequired);
            // Check if selected language code is provided
            else if (string.IsNullOrEmpty(req.selectedLangCode))
                throw new AmazonFarmerException(_exceptions.languageCodeRequired);
            // Check if password and confirmation match
            else if (req.password != req.confirmPassword)
                throw new AmazonFarmerException(_exceptions.confirmPasswordNotMatch);
            // Check if user is older than 18
            else if (!IsOlderThan18(DateTime.ParseExact(req.dateOfBirth, "yyyy-MM-dd", null)))
                throw new AmazonFarmerException(_exceptions.lessThen18);
            CreateCustomer_SAPValidation(req);
        }

        ///<summary>
        ///SAP Validations for create customer
        /// </summary>
        private void CreateCustomer_SAPValidation(farmerSignUp_Req req)
        {
            if (string.Concat(req.firstName, " ", req.lastName).Length >= 41)
                throw new AmazonFarmerException(_exceptions.nameLengthExteed_SAPValidation);
            else if (req.emailAddress.Length >= 242)
                throw new AmazonFarmerException(_exceptions.emailAddressLengthExteed_SAPValidation);
            else if (req.address1.Length >= 61)
                throw new AmazonFarmerException(_exceptions.address1LengthExteed_SAPValidation);
            else if (req.address2.Length >= 61)
                throw new AmazonFarmerException(_exceptions.address2LengthExteed_SAPValidation);
        }

        /// <summary>
        /// Endpoint for verifying OTP (One Time Password).
        /// </summary>
        /// <param name="req">Request object containing OTP verification details.</param>
        /// <returns>Returns an API response containing the result of the OTP verification operation.</returns>
        [HttpPost("verifyOTP")]
        public async Task<APIResponse> verifyOTP(otpVerification_req req)
        {
            APIResponse resp = new APIResponse();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verify OTP
            bool VerifyOTPResp = false;

            TblUser user = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (user != null)
            {
                if (user.OTPExpiredOn < DateTime.UtcNow)

                {
                    throw new AmazonFarmerException(_exceptions.expiredOTP);
                }
                if (user.OTP == req.otpCode)
                {
                    await _repoWrapper.UserRepo.verifyOTP(user);
                    await _repoWrapper.SaveAsync();
                    VerifyOTPResp = true;
                }
            }
            if (!VerifyOTPResp)
                throw new AmazonFarmerException(_exceptions.invalidOTP);
            else
            {
                resp.message = "OTP Verification";
            }

            return resp;
        }


        /// <summary>
        /// Endpoint for retrieving user profile information.
        /// </summary>
        /// <returns>Returns an API response containing the user profile information.</returns>
        [HttpGet("getProfile")]
        public async Task<APIResponse> getProfile()
        {
            APIResponse resp = new APIResponse();// Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Retrieve user profile information
            resp.response = await _repoWrapper.UserRepo.getUserProfileByUserID(userID);
            return resp;
        }


        /// <summary>
        /// Endpoint for initiating the forget password process.
        /// </summary>
        /// <param name="req">Forget password request object containing CNIC number.</param>
        /// <returns>Returns an API response indicating the success or failure of the forget password process.</returns>
        [AllowAnonymous]
        [HttpPost("forgetPassword")]
        public async Task<APIResponse> forgetPassword(forgetPassword_Req req)
        {
            APIResponse resp = new APIResponse();

            // Check if CNIC number is provided
            if (string.IsNullOrEmpty(req.cnicNumber))
                throw new AmazonFarmerException(_exceptions.cnicNumberRequired);

            // Check if the provided CNIC number is valid and retrieve user information
            UserDTO userInfo = await _repoWrapper.UserRepo.isUsernameValid(req);

            // If user information is not found, throw an exception
            if (userInfo == null)
                throw new AmazonFarmerException(_exceptions.cnicNumberNotValid);
            else
            {
                // Generate OTP code
                string OTPCode = OTPExtension.GenerateOTP();

                // Create email containing OTP code
                NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OTP, "EN");
                if (notificationDTO != null)
                {
                    List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                            Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = userInfo.email, Name = userInfo.firstName} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = userInfo.phone, Name = userInfo.firstName} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                },
                            };

                    NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                    replacementDTO.OTP = OTPCode;
                    replacementDTO.UserName = userInfo.firstName;
                    await _notificationService.SendNotifications(notifications, replacementDTO);
                }

                // Set OTP code for user
                await _repoWrapper.UserRepo.setOTP(userInfo.userID, OTPCode);
                await _repoWrapper.SaveAsync();

                // Set success message
                resp.message = "OTP Sent";
            }

            return resp;
        }


        /// <summary>
        /// Endpoint for verifying the availability of a username (CNIC number).
        /// </summary>
        /// <param name="req">Forget password request object containing CNIC number.</param>
        /// <returns>Returns an API response indicating whether the username is available or not.</returns>
        [AllowAnonymous]
        [HttpPost("verifyUsername")]
        public async Task<APIResponse> verifyUsername(forgetPassword_Req req)
        {
            APIResponse resp = new APIResponse();
            // Check if the username (CNIC number) already exists
            UserDTO userInfo = await _repoWrapper.UserRepo.isUsernameValid(req);

            if (userInfo != null)
            {
                // If username is already taken, throw an exception
                throw new AmazonFarmerException(_exceptions.usernameTaken);
            }
            else
            {
                // Set message indicating username availability
                resp.message = "username available";
            }
            return resp;
        }


        /// <summary>
        /// Endpoint for verifying the OTP code sent during the forget password process.
        /// </summary>
        /// <param name="req">Object containing CNIC number and OTP code.</param>
        /// <returns>Returns an API response indicating whether the OTP code is verified or not.</returns>
        [AllowAnonymous]
        [HttpPost("forgetPasswordOTP")]
        public async Task<APIResponse> forgetPasswordOTP(otpVerification_req req)
        {
            APIResponse resp = new APIResponse();
            if (string.IsNullOrEmpty(req.cnicNumber))
                throw new AmazonFarmerException(_exceptions.cnicNumberRequired);
            else if (string.IsNullOrEmpty(req.otpCode))
                throw new AmazonFarmerException(_exceptions.otpRequired);

            // Verify the OTP code for the provided CNIC number
            bool isExist = false;
            //var user = await _repoWrapper.UserRepo.forgetPasswordOTP(req.cnicNumber, req.otpCode);
            var user = await _repoWrapper.UserRepo.forgetUserPasswordOTP(req.cnicNumber, req.otpCode);
            if (user != null)
            {
                if (user.OTPExpiredOn < DateTime.UtcNow)
                {
                    throw new AmazonFarmerException(_exceptions.expiredOTP);
                }
                //user.User.OTP = "";
                user.OTPExpiredOn = DateTime.UtcNow.AddMinutes(15);
                await _repoWrapper.UserRepo.emptyPasswordAttempts(user);
                await _repoWrapper.SaveAsync();
                isExist = true;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.invalidOTP);
            }

            if (isExist)
            {
                // If OTP is verified, set the response message
                resp.message = "Your OTP has been successfully verified";
            }
            return resp;
        }


        /// <summary>
        /// Endpoint for changing the user password.
        /// </summary>
        /// <param name="req">Object containing current password, new password, and confirm password.</param>
        /// <returns>Returns an API response indicating the status of password change operation.</returns>
        [HttpPost("changePassword")]
        public async Task<APIResponse> changePassword(changePassword_Req req)
        {
            APIResponse resp = new APIResponse();
            // Check if the current password is provided
            if (string.IsNullOrEmpty(req.currentPassword))
                throw new AmazonFarmerException(_exceptions.passwordRequired);

            // Check if the new password is provided
            if (string.IsNullOrEmpty(req.password))
                throw new AmazonFarmerException(_exceptions.passwordRequired);

            // Check if the confirm password is provided
            else if (string.IsNullOrEmpty(req.confirmPassword))
                throw new AmazonFarmerException(_exceptions.confirmPasswordRequired);

            // Check if the new password matches the confirm password
            else if (req.password != req.confirmPassword)
                throw new AmazonFarmerException(_exceptions.confirmPasswordNotMatch);

            else
            {
                // Change the user password
                TblUser user = await _repoWrapper.UserRepo.getUserByUserID((User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, req.currentPassword, req.password);
                    if (!result.Succeeded)
                    {
                        throw new AmazonFarmerException(result.Errors.FirstOrDefault().Description.ToString());
                    }
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                }
                resp.message = "Password Changed";
            }
            return resp;
        }


        /// <summary>
        /// Endpoint for user sign-in.
        /// </summary>
        /// <param name="req">Object containing username, password, and app version.</param>
        /// <returns>Returns an API response containing authentication token upon successful sign-in.</returns>
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<APIResponse> signin(Login_Req req)
        {
            APIResponse resp = new APIResponse();
            // Check if the app version is valid
            if (req.appVersion != Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
                throw new AmazonFarmerException(_exceptions.invalidAppVersion);

            // Check if username or password is null or empty
            else if (string.IsNullOrEmpty(req.username) || string.IsNullOrEmpty(req.password))
                throw new AmazonFarmerException(_exceptions.nullUsernameOrPassword);

            // Check if the platform is provided
            else if (string.IsNullOrEmpty(req.platform))
                throw new AmazonFarmerException(_exceptions.platformRequired);

            else
            {
                TblUser? user = await _repoWrapper.UserRepo.getUserByUserName(req.username);
                if (user == null)
                {
                    throw new AmazonFarmerException(_exceptions.invalidEmail);
                }
                if (user.isAccountLocked)
                    throw new AmazonFarmerException(_exceptions.deactiveUser);

                tblFarmerProfile _req = new tblFarmerProfile();

                //if (User.IsInRole("Farmer"))
                if (user.FarmerRoles.Any(x => x.Role.eRole == ERoles.Farmer))
                {
                    _req = await _repoWrapper.UserRepo.getFarmerProfileByUserID(user.Id);
                    if (_req == null)
                    {
                        throw new AmazonFarmerException(_exceptions.userNotAuthorized);
                    }
                    else
                    {
                        _req.SelectedLangCode = req.languageCode;
                    }


                }


                var result = await _signInManager.PasswordSignInAsync(user, req.password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    if ((user.AccessFailedCount + 1) >= 3)
                    {
                        await _repoWrapper.UserRepo.lockUserAccount(user);
                    }
                    else
                    {
                        await _repoWrapper.UserRepo.wrongPasswordCounter(user);
                    }
                    await _repoWrapper.SaveAsync();

                    throw new AmazonFarmerException(_exceptions.invalidPassword);
                }
                if (user.Active != EActivityStatus.Active)
                {
                    throw new AmazonFarmerException(_exceptions.deactiveUser);
                }


                await _repoWrapper.UserRepo.emptyPasswordAttempts(user);

                await _repoWrapper.UserRepo.updateDeviceToken(user, req.deviceToken);
                if (_req != null)
                {
                    await updateSelectedLanguage(_req);
                }
                await _repoWrapper.SaveAsync();

                // Get user information by username and password
                UserDTO DBResp = new UserDTO()
                {
                    email = user.Email,
                    //isApproved = _req.isApproved == EFarmerProfileStatus.Approved ? true : false,
                    isOTPVerified = user.isOTPApproved.Value,
                    designationID = user.Designation == null ? 0 : (int)user.Designation,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    userID = user.Id,
                    languageCode = _req.SelectedLangCode
                };

                if (DBResp == null)
                    throw new AmazonFarmerException(_exceptions.userNotFound);

                NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.signin, _req.SelectedLangCode);
                if (notificationDTO != null)
                {
                    string fcmRecipient = await _repoWrapper.UserRepo.getDeviceTokenByUserID(DBResp.userID);
                    List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.FCM,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = fcmRecipient, Name = ""} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.fcmBody
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = DBResp.email, Name = DBResp.firstName} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = DBResp.phone, Name = DBResp.firstName} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                },
                            };

                    NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();

                    await _notificationService.SendNotifications(notifications, replacementDTO);
                }
                TblUser? dbUser = await _repoWrapper.UserRepo.getUserByUserName(req.username);

                var userRoles = await _userManager.GetRolesAsync(dbUser);
                var authClaims = new List<Claim>();

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role.ToString(), userRole));
                }

                int TokenExpiration = Convert.ToInt32(ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays"));
                // Generate JWT token
                TokenResponse token = JWTService.GenerateJwt(
                    DBResp,
                    ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                    ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                    TokenExpiration, authClaims
                );

                List<ActiveToken> existingTokens = await _repoWrapper.UserRepo.GetActiveTokensForUser(dbUser.Id);
                if (existingTokens != null && existingTokens.Count() > 0)
                {
                    foreach (ActiveToken existingToken in existingTokens)
                    {
                        _repoWrapper.UserRepo.RemoveActiveToken(existingToken);
                    }
                }

                await _repoWrapper.SaveAsync();

                ActiveToken newToken = new()
                {
                    CreatedAt = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(TokenExpiration),
                    Token = token.Token,
                    UserId = dbUser.Id,
                };
                _repoWrapper.UserRepo.AddActiveToken(newToken);
                await _repoWrapper.SaveAsync();

                // Set the token in the API response
                resp.response = token;
            }

            return resp;
        }
        private async Task updateSelectedLanguage(tblFarmerProfile profile)
        {
            if (!string.IsNullOrEmpty(profile.UserID))
            {
                profile.FatherName = profile.FatherName ?? string.Empty;
                profile.CNICNumber = profile.CNICNumber ?? string.Empty;
                profile.NTNNumber = profile.NTNNumber ?? string.Empty;
                profile.STRNNumber = profile.STRNNumber ?? string.Empty;
                profile.CellNumber = profile.CellNumber ?? string.Empty;
                profile.OwnedLand = profile.OwnedLand ?? string.Empty;
                profile.LeasedLand = profile.LeasedLand ?? string.Empty;
                profile.Address1 = profile.Address1 ?? string.Empty;
                profile.DateOfBirth = profile.DateOfBirth ?? string.Empty;
                await _repoWrapper.UserRepo.updateSelectedLanguage(profile, profile.SelectedLangCode);
            }
        }


        /// <summary>
        /// Endpoint for resending OTP (One Time Password) for forgotten password.
        /// </summary>
        /// <param name="req">Object containing CNIC number for identifying the user.</param>
        /// <returns>Returns an API response indicating whether the OTP was successfully sent.</returns>
        [AllowAnonymous]
        [HttpPost("resendOTP")]
        public async Task<APIResponse> resendOTP(forgetPassword_Req req)
        {
            APIResponse resp = new APIResponse();
            // Check if CNIC number is provided
            if (string.IsNullOrEmpty(req.cnicNumber))
                throw new AmazonFarmerException(_exceptions.cnicNumberRequired);
            else
            {
                // Get user information by CNIC number
                UserDTO? DBResp = await _repoWrapper.UserRepo.getUserInfoByCNIC(req.cnicNumber);

                // If user exists and has a valid user ID
                if (DBResp != null && !string.IsNullOrEmpty(DBResp.userID))
                {
                    // Generate OTP code
                    string OTPCode = OTPExtension.GenerateOTP();

                    // Prepare email with OTP code
                    NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OTP, "EN");
                    if (notificationDTO != null)
                    {
                        List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = DBResp.phone, Name = DBResp.firstName} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = DBResp.email, Name = DBResp.firstName} },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                }
                            };


                        NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                        // Send the email with OTP code
                        replacementDTO.OTP = OTPCode;
                        replacementDTO.UserName = DBResp.firstName;
                        await _notificationService.SendNotifications(notifications, replacementDTO);
                    }

                    // Set the OTP in the database for the user
                    await _repoWrapper.UserRepo.setOTP(DBResp.userID, OTPCode);
                    await _repoWrapper.SaveAsync();
                    resp.message = "OTP Sent";
                }
            }
            return resp;
        }


        /// <summary>
        /// Endpoint for setting a new password after forgetting the old one.
        /// </summary>
        /// <param name="req">Object containing CNIC number, OTP code, new password, and confirm password.</param>
        /// <returns>Returns an API response indicating whether the password was successfully changed.</returns>
        [AllowAnonymous]
        [HttpPost("newPassword")]
        public async Task<APIResponse> newPassword(newPassword_Req req)
        {
            APIResponse resp = new APIResponse();

            // Check if the app version is valid
            if (req.appVersion != Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
                throw new AmazonFarmerException(_exceptions.invalidAppVersion);
            // Check if CNIC number is provided
            else if (string.IsNullOrEmpty(req.cnicNumber))
                throw new AmazonFarmerException(_exceptions.cnicNumberRequired);
            // Check if platform is provided
            else if (string.IsNullOrEmpty(req.platform))
                throw new AmazonFarmerException(_exceptions.platformRequired);
            // Check if OTP code is provided
            else if (string.IsNullOrEmpty(req.otpCode))
                throw new AmazonFarmerException(_exceptions.otpRequired);
            // Check if password is provided
            else if (string.IsNullOrEmpty(req.password))
                throw new AmazonFarmerException(_exceptions.passwordRequired);
            // Check if confirm password is provided
            else if (string.IsNullOrEmpty(req.confirmPassword))
                throw new AmazonFarmerException(_exceptions.confirmPasswordRequired);
            // Check if passwords match
            else if (req.password != req.confirmPassword)
                throw new AmazonFarmerException(_exceptions.confirmPasswordNotMatch);
            else if (matchPasswordCriteria(req.password))
            {
                // Set new password
                var userProfile = await _repoWrapper.UserRepo.getFarmerProfileByUserCNIC(req.cnicNumber);

                if (userProfile != null)
                {
                    //Removing this as perZahid bhai that OTP expiry will not be verified when new password call - Kamran
                    //if (userProfile.User.OTPExpiredOn < DateTime.UtcNow)
                    //{
                    //    throw new AmazonFarmerException(_exceptions.expiredOTP);
                    //}
                    if (userProfile.OTP == req.otpCode)
                    {
                        var _token = await _userManager.GeneratePasswordResetTokenAsync(userProfile);

                        var result = await _userManager.ResetPasswordAsync(userProfile, _token, req.password);

                        if (!result.Succeeded)
                        {
                            throw new AmazonFarmerException(result.Errors.FirstOrDefault().Description.ToString());
                        }
                        _repoWrapper.UserRepo.emptyPasswordAttempts(userProfile);
                        await _repoWrapper.UserRepo.unlockUserAccount(userProfile);

                        //await _repoWrapper.SaveAsync();
                    }
                    else
                    {
                        throw new AmazonFarmerException(_exceptions.invalidOTP);
                    }
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                }


                // Get user information by CNIC number
                UserDTO DBResp = new UserDTO
                {
                    email = userProfile.Email,
                    firstName = userProfile.FirstName,
                    lastName = userProfile.LastName,
                    designationID = userProfile.Designation == null ? 0 : (int?)userProfile.Designation,
                    isOTPVerified = userProfile.isOTPApproved.Value,
                    userID = userProfile.Id,
                    languageCode = userProfile.FarmerProfile != null && userProfile.FarmerProfile.Count() >= 1 ? userProfile.FarmerProfile.FirstOrDefault().SelectedLangCode : "EN"
                };

                if (DBResp == null)
                    throw new AmazonFarmerException(_exceptions.userNotFound);

                TblUser? dbUser = await _repoWrapper.UserRepo.getUserByUserID(userProfile.Id);

                var userRoles = await _userManager.GetRolesAsync(dbUser);
                var authClaims = new List<Claim>();

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role.ToString(), userRole));
                }

                // Generate JWT token for the user
                TokenResponse token = JWTService.GenerateJwt(
                    DBResp,
                    ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                    ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                    Convert.ToInt32(
                        ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays")
                    ), authClaims
                );

                List<ActiveToken> existingTokens = await _repoWrapper.UserRepo.GetActiveTokensForUser(userProfile.Id);
                if (existingTokens != null && existingTokens.Count() > 0)
                {
                    foreach (ActiveToken existingToken in existingTokens)
                    {
                        _repoWrapper.UserRepo.RemoveActiveToken(existingToken);
                    }
                }
                await _repoWrapper.SaveAsync();

                ActiveToken newToken = new()
                {
                    CreatedAt = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(Convert.ToInt32(
                        ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays")
                    )),
                    Token = token.Token,
                    UserId = dbUser.Id,
                };
                _repoWrapper.UserRepo.AddActiveToken(newToken);

                await _repoWrapper.SaveAsync();

                // Set response with token and success message
                resp.response = token;
                resp.message = "Password Changed";
            }
            return resp;
        }


        /// <summary>
        /// Endpoint for retrieving usernames along with OTPs. This endpoint is obsolete.
        /// </summary>
        /// <returns>Returns an API response containing usernames along with OTPs.</returns>
        [AllowAnonymous]
        [Obsolete("This endpoint is obsolete.")]
        [HttpPost("getUsernamesWithOTP")]
        public async Task<APIResponse> getUsernamesWithOTP()
        {
            APIResponse resp = new APIResponse();
            // Get all usernames with OTPs
            resp.response = await _repoWrapper.UserRepo.getAllUsernameWithOTPs();
            return resp;
        }


        /// <summary>
        /// Endpoint for approving a farmer account.
        /// </summary>
        /// <param name="req">The request containing the user ID of the farmer account to be approved.</param>
        /// <returns>Returns a JSON response indicating the success or failure of the approval process.</returns>
        /// 
        [Obsolete]
        [HttpPost("approveFarmer")]
        public async Task<JSONResponse> approveFarmer(approveFarmer_Req req)
        {
            JSONResponse resp = new JSONResponse();
            // Get the ID of the approver
            var approverID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Approve the farmer account
            tblFarmerProfile? farmerProfile = await _repoWrapper.UserRepo.getFarmerProfileByUserID(req.userID);
            if (farmerProfile != null)
            {
                await _repoWrapper.UserRepo.approveFarmAccountByUserID(farmerProfile, approverID);
            }
            return resp;
        }

        [HttpGet("getUserProfile")]
        public async Task<APIResponse> getUserProfile()
        {
            APIResponse resp = new APIResponse();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);
            else
            {
                //resp.response = await _repoWrapper.UserRepo.getUserProfile(userID, languageCode);
                TblUser user = await _repoWrapper.UserRepo.getUserProfile(userID, languageCode);
                var profile = user.FarmerProfile.FirstOrDefault();

                getUserProfile_Resp responseObj = new getUserProfile_Resp
                {
                    cityID = profile.CityID,
                    city = profile.City.CityLanguages.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Translation,
                    districtID = profile.DistrictID,
                    district = profile.District.DistrictLanguages.Where(x => x.LanguageCode == languageCode).FirstOrDefault().Translation,
                    firstName = user.FirstName,
                    status = (int)profile.isApproved,
                    address1 = profile.Address1,
                    address2 = profile.Address2,
                    lastName = user.LastName ?? "", // Handle nullable last name
                    dateOfBirth = profile.DateOfBirth ?? "", // Check if profile exists
                    fatherName = profile.FatherName ?? "", // Check if profile exists
                    cnicNumber = profile.CNICNumber ?? "", // Check if profile exists
                    ntnNumber = profile.NTNNumber ?? "", // Check if profile exists
                    strnNumber = profile.STRNNumber ?? "", // Check if profile exists
                    cellNumber = user.PhoneNumber,
                    emailAddress = user.Email,
                    ownedLandAcreage = profile.OwnedLand ?? "", // Check if profile exists
                    leasedLandAcreage = profile.LeasedLand ?? "", // Check if profile exists
                    messages = null //user.farms.Where(x => x.RevertedReason != null && x.Status != EFarmStatus.Deleted).Select(x => x.RevertedReason).ToList()                        
                };

                var userAttachments = new List<uploadAttachmentResp>();
                responseObj.cnic = new List<uploadAttachmentResp>();
                responseObj.ntn = new List<uploadAttachmentResp>();

                foreach (var attachment in user.UserAttachments.Where(ua => ua.Status == EActivityStatus.Active))
                {
                    if (attachment.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document)
                    {
                        responseObj.cnic.Add(MapUserAttachmentToAttachmentResponse(attachment));
                    }
                    else if (attachment.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document)
                    {
                        responseObj.ntn.Add(MapUserAttachmentToAttachmentResponse(attachment));
                    }
                }
                resp.response = responseObj;
            }

            return resp;
        }

        [HttpPut("updateUserProfile")]
        public async Task<JSONResponse> updateUserProfile(getUserProfile_Resp req)
        {
            JSONResponse resp = new JSONResponse();
            // Get the user ID from claims
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);

            TblUser? farmerDB = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (farmerDB == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else if (farmerDB.FarmerProfile == null)
                throw new AmazonFarmerException(_exceptions.userProfileNotFound);
            //else if (farmerDB.FarmerProfile.First().isApproved == EFarmerProfileStatus.Approved)
            //    throw new AmazonFarmerException(_exceptions.profileApproved);
            else
            {
                List<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmsByfarmerID(userID);
                bool isFullyEditable = true;
                if (farms.Count() > 0 && farms.Any(x => x.Status == EFarmStatus.Approved))
                {
                    isFullyEditable = false;
                    //throw new AmazonFarmerException(_exceptions.seemsLikeFarmApproved);
                }

                farmerDB.PhoneNumber = req.cellNumber;
                farmerDB.Email = req.emailAddress;
                farmerDB.NormalizedEmail = req.emailAddress.ToUpper();
                tblFarmerProfile? farmerProfile = farmerDB.FarmerProfile.FirstOrDefault();

                if (isFullyEditable && farmerProfile != null)
                {
                    farmerDB.FirstName = req.firstName;
                    farmerDB.LastName = req.lastName;
                    farmerDB.CNICNumber = req.cnicNumber;
                    farmerDB.NormalizedEmail = req.emailAddress.ToUpper();
                    farmerProfile.CellNumber = req.cellNumber;
                    farmerProfile.CNICNumber = req.cnicNumber;
                    farmerProfile.FatherName = req.fatherName;
                    farmerProfile.NTNNumber = req.ntnNumber;
                    farmerProfile.STRNNumber = req.strnNumber;
                    farmerProfile.OwnedLand = (req.ownedLandAcreage);
                    farmerProfile.LeasedLand = (req.leasedLandAcreage);
                    farmerProfile.TotalArea = Convert.ToInt32(req.ownedLandAcreage) + Convert.ToInt32(req.leasedLandAcreage);
                    farmerProfile.DateOfBirth = req.dateOfBirth;
                    farmerProfile.Address1 = req.address1;
                    farmerProfile.Address2 = req.address2;
                    farmerProfile.CityID = req.cityID;
                    farmerProfile.DistrictID = req.districtID;
                    farmerProfile.isApproved = EFarmerProfileStatus.Editted;

                    await uploadSignupAttachment(req.cnic, userID, EAttachmentType.User_CNIC_Document, farmerDB);
                    await uploadSignupAttachment(req.ntn, userID, EAttachmentType.User_NTN_Document, farmerDB);


                    await _repoWrapper.UserRepo.updateUser(farmerDB);
                    await _repoWrapper.SaveAsync();
                    resp.message = "updated";
                }
                else if (farmerProfile != null)
                {
                    farmerProfile.CellNumber = req.cellNumber;
                    await _repoWrapper.UserRepo.updateUser(farmerDB);
                    var wsdlResponse = await CallUpdateCustomerWSDL(farmerProfile, farmerDB);

                    if (wsdlResponse != null && wsdlResponse.Messages.Count() > 0 && wsdlResponse.Messages.FirstOrDefault().Message.msgTyp.ToUpper() == "S")
                    {
                        resp.message = "updated";
                        await _repoWrapper.SaveAsync();
                    }
                    else
                    {
                        throw new AmazonFarmerException(wsdlResponse.Messages.FirstOrDefault().Message.msg);
                    }
                }
            }
            return resp;

        }

        /// <summary>
        /// Uploads signup attachments for a user.
        /// </summary>
        /// <param name="attachments">List of attachments to upload.</param>
        /// <param name="UserID">ID of the user to whom the attachments belong.</param>
        /// <param name="AttachmentType">Type of the attachment.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        //private async Task uploadSignupAttachment(List<farmAttachment> attachments, string UserID, EAttachmentType AttachmentType)
        //{
        //    foreach (var attachment in attachments)
        //    {
        //        if (!string.IsNullOrEmpty(attachment.content))
        //        {
        //            // Decode the base64 string into a byte array
        //            byte[] imageBytes = Convert.FromBase64String(attachment.content);
        //            // Generate a unique file name
        //            string fileName = attachment.name + DateTime.UtcNow.ToString("yyyy.MM.dd") + "-" + Guid.NewGuid().ToString() + ".png";
        //            // Specify the directory where you want to save the image
        //            string filePath = Path.Combine(ConfigExntension.GetConfigurationValue("Locations:AttachmentURL"), fileName);
        //            // Write the image bytes to the file
        //            System.IO.File.WriteAllBytes(filePath, imageBytes);
        //            // Get the URL of the saved image
        //            string imageUrl = $"{ConfigExntension.GetConfigurationValue("Locations:BaseURL")}/Attachments/{fileName}";
        //            // Create an AttachmentsDTO object
        //            AttachmentsDTO attachmentsDTO = new AttachmentsDTO()
        //            {
        //                attachmentType = AttachmentType,
        //                filePath = imageUrl,
        //                fileType = "PNG",
        //                fileName = attachment.name
        //            };
        //            // Upload the attachment
        //            tblAttachment attachmentResp = await _repoWrapper.AttachmentRepo.uploadAttachment(attachmentsDTO);
        //            // Create an attachAttachment object
        //            attachAttachment attachWithUser = new attachAttachment
        //            {
        //                attachmentID = attachmentResp.ID,
        //                userID = UserID
        //            };
        //            // Attach the uploaded attachment to the user
        //            await _repoWrapper.AttachmentRepo.attachUserAttachment(attachWithUser);
        //        }
        //    }
        //}

        [HttpGet("getDashboard")]
        public async Task<APIResponse> getDashboard()
        {
            APIResponse resp = new APIResponse();
            GetDashboard_Resp _inResp = new GetDashboard_Resp();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            if (!string.IsNullOrEmpty(userID))
            {
                var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
                //WarehouseLocation location = new WarehouseLocation();
                //List<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmsByfarmerID(userID);
                //if (farms != null)
                //{
                //    tblfarm farmPrimary = farms.Where(x => x.isPrimary && x.Status == EFarmStatus.Approved).FirstOrDefault();
                //    tblfarm defaultFarm = farms.Where(x => x.Status == EFarmStatus.Approved).FirstOrDefault();
                //    tblfarm locationFarm = farmPrimary == null ? defaultFarm : farmPrimary;

                //    location = new WarehouseLocation()
                //    {
                //        lat = locationFarm.latitude != null ? locationFarm.latitude.Value : 0,
                //        lng = locationFarm.longitude != null ? locationFarm.longitude.Value : 0,
                //    };
                //}
                WarehouseLocation location = new WarehouseLocation();
                List<tblfarm> farms = await _repoWrapper.FarmRepo.getFarmsByfarmerID(userID);
                if (farms?.Any() == true)
                {
                    tblfarm locationFarm = farms?.FirstOrDefault(x => x.isPrimary && x.Status == EFarmStatus.Approved)
                       ?? farms?.FirstOrDefault(x => x.Status == EFarmStatus.Approved);

                    location = new WarehouseLocation()
                    {
                        lat = locationFarm.latitude != null ? locationFarm.latitude.Value : 0,
                        lng = locationFarm.longitude != null ? locationFarm.longitude.Value : 0,
                        city = locationFarm.City.Name != null ? locationFarm.City.Name : string.Empty,
                    };
                }
                List<tblBanner> banners = await _repoWrapper.BannerRepo.getBanners();
                if (banners != null && banners.Count() > 0)
                {
                    _inResp.banners = banners.Where(x => x.BannerType == EBannerType.homeScreen && x.Status == EActivityStatus.Active)
                        .FirstOrDefault()
                        .BannerLanguages
                        .Where(x => x.LanguageCode == languageCode)
                        .Select(x => new BannerDTO
                        {
                            bannerName = string.Empty,
                            filePath = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL") + x.Image.Replace("/", "%2F").Replace(" ", "%20")
                        }).ToList();
                }
                _inResp.nextPay = await _repoWrapper.OrderRepo.getNearestPayableOrdersByUserID(userID);
                _inResp.nextPickup = await _repoWrapper.OrderRepo.getNearestPickupDatesByUserID(userID);




                #region get weather Information
                try
                {
                    EngroWeatherAPI_Response locationResp = await _accuWeatherService.getWeather(location.city);
                    //getWeatherAPI_Req locationResp = await _accuWeatherService.getLocation(location);
                    //_inResp.weather = await _accuWeatherService.getWeather(locationResp);

                    string baseUrl = ConfigExntension.GetConfigurationValue("Locations:PublicAttachmentURL"); //setting up base url

                    string weatherIconPath = baseUrl + ConfigExntension.GetConfigurationValue("AccuWeather:iconURL").Replace("[icon]", locationResp.weatherIconCode).Replace("/", "%2F").Replace(" ", "%20");

                    _inResp.weather = new WeatherDTO()
                    {
                        weatherText = locationResp.description,
                        weatherValue = Math.Round(locationResp.temp).ToString(),
                        weatherArea = locationResp.name,
                        weatherUnit = "C",
                        weatherIconID = locationResp.weatherIconCode,
                        weatherIconPath = weatherIconPath
                    };
                }
                catch (Exception ex)
                {
                    _inResp.weather = new WeatherDTO()
                    {
                        weatherIconID = "01n",
                        //weatherText = apiResp[0].WeatherText,
                        weatherArea = "N/A",
                        weatherUnit = "C",
                        weatherValue = "N/A"
                    };
                }
                //tblWeatherIcon tblWeather = await _repoWrapper.WeatherRepo.getWeatherByWeatherType(_inResp.weather.weatherIconID);
                //var weatherTranslation = tblWeather == null ? null : tblWeather.WeatherIconTranslations.Where(x => x.LanguageCode == languageCode).FirstOrDefault();
                //_inResp.weather.weatherText = weatherTranslation == null ? "" : weatherTranslation.Text;
                //_inResp.weather.weatherIconPath = weatherTranslation == null ? "" : weatherTranslation.Image;
                #endregion

                resp.response = _inResp;
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            }
            return resp;
        }

        [HttpGet("getFarmerInfo")]
        public async Task<APIResponse> getFarmerInfo()
        {
            APIResponse resp = new APIResponse();


            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            var languageCode = User.FindFirst("languageCode")?.Value; // Extracting language code from claims
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(languageCode))
                throw new AmazonFarmerException(_exceptions.userIDorLanguageCodeNotFound);


            TblUser user = await _repoWrapper.UserRepo.getFarmerInfoByFarmerIDAndLanguageCodeUdpated(userID, languageCode);
            var profile = user.FarmerProfile.FirstOrDefault();

            getFarmerInfoResp responseObj = new getFarmerInfoResp
            {
                isGenericMessageEnabled = user.farms.Any() ? user.farms.Any(x => x.Status == EFarmStatus.SendBack || x.Status == EFarmStatus.Rejected) ? true : false : false,
                cityID = profile.CityID,
                city = profile.City.CityLanguages.FirstOrDefault().Translation,
                districtID = profile.DistrictID,
                district = profile.District.DistrictLanguages.FirstOrDefault().Translation,
                firstName = user.FirstName,
                status = (int)profile.isApproved,
                address1 = profile.Address1,
                address2 = profile.Address2,
                lastName = user.LastName ?? "", // Handle nullable last name
                dateOfBirth = profile.DateOfBirth ?? "", // Check if profile exists
                fatherName = profile.FatherName ?? "", // Check if profile exists
                cnicNumber = profile.CNICNumber ?? "", // Check if profile exists
                ntnNumber = profile.NTNNumber ?? "", // Check if profile exists
                strnNumber = profile.STRNNumber ?? "", // Check if profile exists
                cellNumber = user.PhoneNumber,
                emailAddress = user.Email,
                ownedLandAcreage = profile.OwnedLand ?? "", // Check if profile exists
                leasedLandAcreage = profile.LeasedLand ?? "", // Check if profile exists
                messages = user.farms.Where(x => x.RevertedReason != null && x.Status != EFarmStatus.Deleted).Select(x => x.RevertedReason).ToList(),
                farms = user.farms.Select(farm => new farmDetail_Resp()
                {
                    canSetPrimary = farm.Status == EFarmStatus.Draft || farm.Status == EFarmStatus.SendBack ? true : false,
                    cityID = farm.CityID,
                    districtID = farm.DistrictID,
                    tehsilID = farm.TehsilID,
                    farmID = farm.FarmID,
                    farmName = farm.FarmName,
                    acreage = farm.Acreage,
                    address1 = farm.Address1,
                    address2 = farm.Address2,
                    city = farm.City.CityLanguages.First().Translation,
                    district = farm.District.DistrictLanguages.First().Translation,
                    tehsil = farm.Tehsil.TehsilLanguagess.First().Translation,
                    isApproved = farm.isApproved,
                    isLeased = farm.isLeased,
                    isPrimary = farm.isPrimary,
                    revertedReason = farm.RevertedReason,
                    sapFarmID = farm.SAPFarmCode.PadLeft(10, '0'),
                    status = (int)farm.Status,
                    statusDescription = ConfigExntension.GetEnumDescription(farm.Status),
                    attachmentGUID = farm.FarmAttachments.Select(x => new uploadAttachmentResp
                    {
                        id = x.Attachment.ID,
                        type = x.Attachment.FileType,
                        name = x.Attachment.Name,
                        guid = x.Attachment.Guid.ToString()
                    }).ToList()
                }).ToList()
            };

            var userAttachments = new List<uploadAttachmentResp>();
            responseObj.cnic = new List<uploadAttachmentResp>();
            responseObj.ntn = new List<uploadAttachmentResp>();

            foreach (var attachment in user.UserAttachments)
            {
                if (attachment.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document)
                {
                    responseObj.cnic.Add(MapUserAttachmentToAttachmentResponse(attachment));
                }
                else if (attachment.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document)
                {
                    responseObj.ntn.Add(MapUserAttachmentToAttachmentResponse(attachment));
                }
            }
            resp.response = responseObj;
            return resp;
        }
        [HttpPost("verifyPassword")]
        public async Task<APIResponse> verifyPassword(configurePasswordRequest req)
        {
            APIResponse resp = new APIResponse();
            configurePassword_Resp inResp = new configurePassword_Resp() { username = string.Empty };
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            //if (matchPasswordCriteria(req.password))
            if (true)
            {
                TblUser user = await _repoWrapper.UserRepo.getUserByUserID(userID);
                var signinResult = await _signInManager.PasswordSignInAsync(user, req.password, isPersistent: false, lockoutOnFailure: false);
                if (signinResult.Succeeded)
                {
                    inResp.username = user.UserName ?? string.Empty;
                }
                else
                    throw new AmazonFarmerException(_exceptions.verifyPasswordInvalid);

                resp.response = inResp;
            }

            return resp;
        }
        [HttpDelete("deleteAccount")]
        public async Task<JSONResponse> DeleteAccount()
        {
            JSONResponse resp = new JSONResponse();
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracting user ID from claims
            if (!string.IsNullOrEmpty(userID))
            {
                TblUser farmer = await _repoWrapper.UserRepo.getUserByUserID(userID);
                if (farmer != null && farmer.FarmerProfile.Count() > 0)
                {
                    string Del = string.Concat("_Deleted_", DateTime.UtcNow.ToString("ddMMyyyyhhmmss"));
                    farmer.UserName = string.Concat(farmer.UserName, Del);
                    farmer.NormalizedUserName = string.Concat(farmer.NormalizedUserName, Del);
                    farmer.CNICNumber = string.Concat(farmer.CNICNumber, Del);
                    farmer.PhoneNumber = string.Concat(farmer.PhoneNumber, Del);
                    farmer.FarmerProfile.First().CNICNumber = string.Concat(farmer.FarmerProfile.First().CNICNumber, Del);
                    farmer.FarmerProfile.First().NTNNumber = string.Concat(farmer.FarmerProfile.First().NTNNumber, Del);
                    farmer.FarmerProfile.First().STRNNumber = string.Concat(farmer.FarmerProfile.First().STRNNumber, Del);
                    farmer.FarmerProfile.First().CellNumber = string.Concat(farmer.FarmerProfile.First().CellNumber, Del);
                    farmer.Active = EActivityStatus.Deleted;
                    await _repoWrapper.UserRepo.updateUser(farmer);
                    await _repoWrapper.SaveAsync();
                }
                else
                {
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                }
            }
            else
            {
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            }
            return resp;
        }
        private uploadAttachmentResp MapUserAttachmentToAttachmentResponse(tblUserAttachments attachment)
        {
            return new uploadAttachmentResp
            {
                id = attachment.tblAttachmentID,
                name = attachment.Attachment.Name,
                guid = attachment.Attachment.Guid.ToString(),
                type = attachment.Attachment.FileType
            };
        }
        /// <summary>
        /// Uploads signup attachments for a user.
        /// </summary>
        /// <param name="attachments">List of attachments to upload.</param>
        /// <param name="UserID">ID of the user to whom the attachments belong.</param>
        /// <param name="AttachmentType">Type of the attachment.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task uploadSignupAttachment(List<uploadAttachmentResp> newAttachments, string UserID, EAttachmentType AttachmentType, TblUser user)
        {
            if (user.UserAttachments != null && user.UserAttachments.Count > 0)
            {
                List<tblUserAttachments> userAttachments = user.UserAttachments.ToList();

                foreach (tblUserAttachments userAttachment in userAttachments)
                {
                    if (userAttachment.Attachment != null && userAttachment.Attachment.AttachmentTypes.AttachmentType == AttachmentType)
                    {
                        userAttachment.Status = EActivityStatus.DeActive;
                        await _repoWrapper.AttachmentRepo.updateUserAttachment(userAttachment);
                    }
                }

                foreach (var newAttachment in newAttachments)
                {
                    tblUserAttachments userAttachment = userAttachments.Where(a => a.tblAttachmentID == newAttachment.id).FirstOrDefault();
                    if (userAttachment != null)
                    {
                        userAttachment.Status = EActivityStatus.Active;
                        await _repoWrapper.AttachmentRepo.updateUserAttachment(userAttachment);
                    }
                    else
                    {
                        attachAttachment attachWithUser = new attachAttachment
                        {
                            attachmentID = newAttachment.id,
                            userID = UserID
                        };
                        await _repoWrapper.AttachmentRepo.attachUserAttachment(attachWithUser);
                    }
                }
            }
            else
            {

                foreach (var newAttachment in newAttachments)
                {
                    attachAttachment attachWithUser = new attachAttachment
                    {
                        attachmentID = newAttachment.id,
                        userID = UserID
                    };
                    await _repoWrapper.AttachmentRepo.attachUserAttachment(attachWithUser);
                }
            }
        }
        /// <summary>
        /// Uploads signup attachments for a user.
        /// </summary>
        /// <param name="attachments">List of attachments to upload.</param>
        /// <param name="UserID">ID of the user to whom the attachments belong.</param>
        /// <param name="AttachmentType">Type of the attachment.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task uploadSignupAttachment(List<int> attachments, string UserID, EAttachmentType AttachmentType)
        {
            foreach (var attachment in attachments)
            {
                // Create an attachAttachment object
                attachAttachment attachWithUser = new attachAttachment
                {
                    attachmentID = attachment,
                    userID = UserID
                };
                // Attach the uploaded attachment to the user
                await _repoWrapper.AttachmentRepo.attachUserAttachment(attachWithUser);
            }
        }

        /// <summary>
        /// Checks if a person is older than 18 years based on their date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth of the person.</param>
        /// <returns>True if the person is older than 18 years, otherwise false.</returns>
        private bool IsOlderThan18(DateTime dateOfBirth)
        {
            // Calculate the age by subtracting the date of birth from the current date
            TimeSpan ageDifference = DateTime.Today - dateOfBirth;
            int ageInYears = (int)(ageDifference.TotalDays / 365.25); // Allowing for leap years

            // Check if the age is greater than or equal to 18
            return ageInYears >= 18;
        }


        //Function to check password criteria
        private bool matchPasswordCriteria(string password)
        {
            bool isMatch = false;

            //if Password is null
            if (password == null)
                throw new AmazonFarmerException(_exceptions.passwordRequired);
            // Minimum length should be at least 8
            else if (password.Length < 8)
                throw new AmazonFarmerException(_exceptions.passwordMinLength);
            // Number of special characters to include 1
            else if (!Regex.IsMatch(password, @"[!@#$%^&*()_+}{:;'?/>.<,|=-]"))
                throw new AmazonFarmerException(_exceptions.specialCharacterRequired);
            // Must contain at least 1 upper case character(s)
            else if (!password.Any(char.IsUpper))
                throw new AmazonFarmerException(_exceptions.passwordOneUpperCaseRequired);

            // Number of numerals to include 1
            else if (!password.Any(char.IsDigit))
                throw new AmazonFarmerException(_exceptions.passwordOneDigitRequired);

            // Must not be a palindrome
            else if (IsPalindrome(password))
                throw new AmazonFarmerException(_exceptions.palindromeFound);

            // Must not contain any character more than twice consecutively
            else if (Regex.IsMatch(password, @"(.)\1{2,}"))
                throw new AmazonFarmerException(_exceptions.consecutivityFound);

            // Must not contain restricted patterns
            else if (ContainsRestrictedPattern(password))
                throw new AmazonFarmerException(_exceptions.ristrictedCharactersFound);

            // Must contain at least 1 lower case character(s)
            else if (!password.Any(char.IsLower))
                throw new AmazonFarmerException(_exceptions.passwordOneLowerCaseRequired);
            else
            {
                isMatch = true;
            }

            return isMatch;
        }

        // Function to check if the password is a palindrome
        private bool IsPalindrome(string password)
        {
            int length = password.Length;
            for (int i = 0; i < length / 2; i++)
            {
                if (password[i] != password[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if the password contains restricted patterns
        private bool ContainsRestrictedPattern(string password)
        {
            string restrictedWords = ConfigExntension.GetConfigurationValue("RestrictedWords");
            string[] restrictedPatterns = restrictedWords.Split(","); // Add more patterns as needed.
            foreach (string pattern in restrictedPatterns)
            {
                if (password.ToLower().Contains(pattern.ToLower()))
                    return true;
            }
            return false;
        }


        private async Task<ResponseType?> CallUpdateCustomerWSDL(tblFarmerProfile profile, TblUser user)
        {
            var request = new RequestType
            {
                city = "ZZ",
                condGrp1 = "ZZ",
                condGrp2 = "ZZ",
                condGrp3 = "ZZ",
                condGrp4 = "ZZ",
                district = "ZZ",
                email = user.Email,
                fax = "ZZ",
                mobileNum = profile.CellNumber,
                name = "ZZ",
                phoneNum = user.PhoneNumber,
                postalCode = "ZZ",
                salePoint = "ZZ",
                searchTerm1 = "ZZ",
                searchTerm2 = "ZZ",
                street = "ZZ",
                street2 = "ZZ",
                street4 = "",
                custNum = profile.SAPFarmerCode
            };

            WSDLFunctions wSDLFunctions = new WSDLFunctions(_repoWrapper, _wsdlConfig);

            ResponseType? wsdlResponse = await wSDLFunctions.ChangeCustomerWSDLAsync(request);


            return wsdlResponse;

        }
    }
}
