﻿using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmer.NotificationServices.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AmazonFarmer.Administrator.API.Controllers
{
    /// <summary>
    /// Controller for managing farmer-related operations.
    /// </summary>
    [EnableCors("corsPolicy")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/User")]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly UserManager<TblUser> _userManager;
        private readonly RoleManager<TblRole> _roleManager;
        private readonly SignInManager<TblUser> _signInManager;
        private readonly NotificationService _notificationService;
        public UserController(IRepositoryWrapper repoWrapper,
            UserManager<TblUser> userManager, RoleManager<TblRole> roleManager,
            SignInManager<TblUser> signInManager, NotificationService notificationService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
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
            if (req.appVersion < Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
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


                if (user.Designation != EDesignation.Admin)
                {
                    throw new AmazonFarmerException(_exceptions.onlyAdmin);
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

                //await _repoWrapper.UserRepo.updateDeviceToken(user, req.deviceToken);
                await _repoWrapper.SaveAsync();

                // Get user information by username and password
                UserDTO DBResp = new UserDTO()
                {
                    email = user.Email ?? string.Empty,
                    //isApproved = _req.isApproved == EFarmerProfileStatus.Approved ? true : false,
                    //isOTPVerified = user.isOTPApproved.Value,
                    designationID = user.Designation == null ? 0 : (int)user.Designation,
                    firstName = user.FirstName,
                    lastName = user.LastName ?? string.Empty,
                    userID = user.Id,
                    languageCode = "EN"
                };

                if (DBResp == null)
                    throw new AmazonFarmerException(_exceptions.userNotFound);

                NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.signin);
                if (notificationDTO != null)
                {
                    string fcmRecipient = await _repoWrapper.UserRepo.getDeviceTokenByUserID(DBResp.userID);
                    List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                                    Recipients = new List<NotificationRequestRecipient> ()
                                    {
                                        new NotificationRequestRecipient { Name = DBResp.email, Email = DBResp.firstName }
                                    },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients =new List<NotificationRequestRecipient> {
                                            new NotificationRequestRecipient(){
                                                Email = DBResp.phone,
                                                Name = DBResp.firstName
                                            }
                                            },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody

                                },
                        };
                    NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                    replacementDTO.NotificationBodyTypeID = ENotificationBody.signin;
                    await _notificationService.SendNotifications(notifications, replacementDTO);
                }
                var authClaims = new List<Claim>();

                TblUser? dbUser = await _repoWrapper.UserRepo.getUserByUserName(req.username);
                if (dbUser != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(dbUser);

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role.ToString(), userRole));
                    }
                }


                // Generate JWT token
                TokenResponse token = JWTService.GenerateJwt(
                    DBResp,
                    ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                    ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                    Convert.ToInt32(
                        ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays")
                    ), authClaims
            );

                // Set the token in the API response
                resp.response = token;
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
            if (!string.IsNullOrEmpty(userID))
                resp.response = await _repoWrapper.UserRepo.getUserProfileByUserID(userID);
            else
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            return resp;
        }
        [HttpPost("addUser")]
        public async Task<JSONResponse> AddUser(addEmployeeDTO req)
        {
            JSONResponse response = new JSONResponse();
            validateUserRequest(req);
            TblUser employee = await _repoWrapper.UserRepo.getUserByUsername(req.userName, req.phoneNumber, req.emailAddress);

            #region Throw error if the username / phonenumber / email address is taken
            if (employee != null)
            {
                if (employee.PhoneNumber == req.phoneNumber)
                    throw new AmazonFarmerException(_exceptions.phoneAlreadyTaken);
                if (employee.UserName == req.userName)
                    throw new AmazonFarmerException(_exceptions.usernameIsTaken);
                if (employee.UserName == req.emailAddress)
                    throw new AmazonFarmerException(_exceptions.emailIsTaken);
            }
            #endregion

            else
            {
                string randPassword = OTPExtension.GenerateOTP();
                employee = new TblUser()
                {
                    FirstName = req.firstName,
                    LastName = req.lastName,
                    PhoneNumber = req.phoneNumber,
                    UserName = req.userName,
                    NormalizedUserName = req.userName.ToUpper(),
                    Email = req.emailAddress,
                    NormalizedEmail = req.emailAddress.ToUpper(),
                    Active = req.status ? EActivityStatus.Active : EActivityStatus.DeActive,
                    OTP = randPassword,
                    Designation = (EDesignation)req.designationID,
                    isAccountLocked = req.isLocked
                };
                randPassword = string.Concat("Engro-", randPassword);

                var updateResult = await _userManager.CreateAsync(employee, randPassword);
                if (updateResult.Succeeded)
                {
                    TblRole? newRole = await _repoWrapper.RoleRepo.getRoleByEnum(ERoles.Employee);
                    if (newRole != null && !string.IsNullOrEmpty(newRole.Name))
                        await _userManager.AddToRoleAsync(employee, newRole.Name);
                    else
                        throw new AmazonFarmerException(_exceptions.userAddedRoleNotAssigned);

                    NotificationDTO notificationDTO = await _repoWrapper.NotificationRepo.getNotificationByENotificationBody(ENotificationBody.OTP, "EN");
                    if (notificationDTO != null)
                    {
                        List<NotificationRequest> notifications = new List<NotificationRequest> {
                                new NotificationRequest
                                {
                                    Type= ENotificationType.Email,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = req.emailAddress, Name = req.firstName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.body
                                },
                                new NotificationRequest
                                {
                                    Type= ENotificationType.SMS,
                                    Recipients = new List<NotificationRequestRecipient> { new NotificationRequestRecipient() { Email = req.phoneNumber, Name = req.firstName } },
                                    Subject =  notificationDTO.title,
                                    Message = notificationDTO.smsBody
                                }
                            };

                        NotificationReplacementDTO replacementDTO = new NotificationReplacementDTO();
                        replacementDTO.Project = "Ugai";
                        replacementDTO.NotificationBodyTypeID = ENotificationBody.OTP;
                        replacementDTO.Password = randPassword;

                        await _notificationService.SendNotifications(notifications, replacementDTO);
                    }

                    response.message = "employee added!";
                }
                else
                {
                    if (updateResult.Errors != null && updateResult.Errors.Count() > 0)
                    {
                        throw new AmazonFarmerException(updateResult.Errors.First().Description);
                    }
                    else
                    {
                        throw new AmazonFarmerException(_exceptions.errorOccuredWhileAddingUser);
                    }
                }

            }
            return response;
        }
        [HttpPut("updateUser")]
        public async Task<JSONResponse> UpdateUser(updateEmployeeDTO req)
        {
            JSONResponse response = new JSONResponse();
            validateUserRequest(req);
            if (string.IsNullOrEmpty(req.userID))
                throw new AmazonFarmerException(_exceptions.userIDNotFound);
            else
            {
                TblUser user = await _repoWrapper.UserRepo.getUserByUserID(req.userID);
                if (user == null)
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                else
                {
                    user.FirstName = req.firstName;
                    user.LastName = req.lastName;
                    user.PhoneNumber = req.phoneNumber;
                    user.UserName = req.userName;
                    user.NormalizedUserName = req.userName.ToUpper();
                    user.Email = req.emailAddress;
                    user.NormalizedEmail = req.emailAddress.ToUpper();
                    user.Active = req.status ? EActivityStatus.Active : EActivityStatus.DeActive;
                    user.Designation = (EDesignation)req.designationID;
                    user.isAccountLocked = req.isLocked;
                    await _repoWrapper.UserRepo.updateUser(user);
                    await _repoWrapper.SaveAsync();
                }
            }
            return response;
        }
        private void validateUserRequest(addEmployeeDTO req)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrEmpty(req.firstName))
                throw new AmazonFarmerException(_exceptions.firstnameRequired);
            else if (string.IsNullOrEmpty(req.userName))
                throw new AmazonFarmerException(_exceptions.usernameRequired);
            else if (string.IsNullOrEmpty(req.emailAddress))
                throw new AmazonFarmerException(_exceptions.emailRequired);
            else if (string.IsNullOrEmpty(req.phoneNumber))
                throw new AmazonFarmerException(_exceptions.phoneRequired);
            else if (req.designationID == 0)
                throw new AmazonFarmerException(_exceptions.designationIDRequired);
            else if (!Regex.IsMatch(req.emailAddress, pattern))
                throw new AmazonFarmerException(_exceptions.emailRegexExpressionFails);
        }
        [HttpPost("getUsers")]
        public async Task<APIResponse> GetUsers(pagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<TblUser> users = _repoWrapper.UserRepo.getUsers();
            users = users.Where(x => x.Active != EActivityStatus.Deleted);
            if (!string.IsNullOrEmpty(req.search))
            {
                users = users.Where(x =>
                x.FirstName.ToLower().Contains(req.search.ToLower()) ||
                (x.LastName != null && x.LastName.ToLower().Contains(req.search.ToLower())) ||
                (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(req.search.ToLower())) ||
                (x.Email != null && x.Email.ToLower().Contains(req.search.ToLower())) ||
                (x.UserName != null && x.UserName.ToLower().Contains(req.search.ToLower()))
                );
            }
            InResp.totalRecord = users.Count();
            users = users.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = users.Count();
            InResp.list = await users.Select(x => new
            {
                userID = x.Id,
                firstName = x.FirstName,
                lastName = x.LastName ?? string.Empty,
                phoneNumber = x.PhoneNumber ?? string.Empty,
                email = x.Email ?? string.Empty,
                userName = x.UserName ?? string.Empty,
                status = (int)x.Active,
                lockedOutEnabled = x.isAccountLocked
            }).ToListAsync();
            response.response = InResp;
            return response;
        }

        [HttpGet("getUserDetails/{userID}")]
        public async Task<APIResponse> GetUserDetails(string userID)
        {
            APIResponse response = new APIResponse();
            TblUser user = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (user == null)
            {
                throw new AmazonFarmerException(_exceptions.userNotFound);
            }
            response.response = new
            {
                userID = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName ?? string.Empty,
                userName = user.UserName ?? string.Empty,
                designationID = user.Designation == null ? 0 : (int)user.Designation,
                designation = user.Designation != null ? ConfigExntension.GetEnumDescription(user.Designation) : ConfigExntension.GetEnumDescription(ERoles.Farmer),
                role = user.FarmerRoles != null && user.FarmerRoles.Count() > 0 ? user.FarmerRoles.FirstOrDefault().Role.Name : string.Empty,
                email = user.Email ?? string.Empty,
                phoneNumber = user.PhoneNumber ?? string.Empty,
                dateOfBirth = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().DateOfBirth : string.Empty,
                fatherName = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().FatherName : string.Empty,
                strnNumber = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().STRNNumber : string.Empty,
                cnicNumber = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().CNICNumber : string.Empty,
                cnicAttachment = user.UserAttachments != null ? user.UserAttachments
                .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document)
                .Select(y => new uploadAttachmentResp
                {
                    id = y.Attachment.ID,
                    type = y.Attachment.FileType,
                    name = y.Attachment.Name,
                    guid = y.Attachment.Guid.ToString()
                }).ToList() : new List<uploadAttachmentResp>(),
                ntnNumber = user.FarmerProfile != null && user.FarmerProfile.Count() > 0? user.FarmerProfile.FirstOrDefault().NTNNumber : string.Empty,
                ntnAttachment = user.UserAttachments != null ? user.UserAttachments
                .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_NTN_Document)
                .Select(y => new uploadAttachmentResp
                {
                    id = y.Attachment.ID,
                    type = y.Attachment.FileType,
                    name = y.Attachment.Name,
                    guid = y.Attachment.Guid.ToString()
                }).ToList() : new List<uploadAttachmentResp>(),
                status = (int)user.Active,
                lockedOutEnabled = user.isAccountLocked
            };

            return response;

        }
        [HttpGet("getDesignations")]
        public APIResponse GetDesignations()
        {
            APIResponse resp = new APIResponse();
            resp.response = Enum.GetValues(typeof(EDesignation))
                         .Cast<EDesignation>()
                         .Select(status => new
                         {
                             role = ConfigExntension.GetEnumDescription(status),
                             roleID = (int)status
                         })
                         .ToList();
            return resp;
        }
    }
}