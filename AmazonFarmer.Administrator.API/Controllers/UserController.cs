using AmazonFarmer.Administrator.API.Extensions;
using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmer.NotificationServices.Services;
using AmazonFarmer.WSDL.Helpers;
using AmazonFarmer.WSDL;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ChangeCustomer;
using Microsoft.Extensions.Options;

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
        private WsdlConfig _wsdlConfig;
        public UserController(IRepositoryWrapper repoWrapper,
            UserManager<TblUser> userManager, RoleManager<TblRole> roleManager,
            SignInManager<TblUser> signInManager, NotificationService notificationService,
            IOptions<WsdlConfig> wsdlConfig)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
            _wsdlConfig = wsdlConfig.Value;
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
                if (employee.Email == req.emailAddress)
                    throw new AmazonFarmerException(_exceptions.emailIsTaken);
            }
            #endregion

            else
            {
                string randPassword = OTPExtension.GenerateOTP();
                employee = new TblUser()
                {
                    CNICNumber = req.cnicNumber,
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
                    isAccountLocked = req.isLocked,
                    EmployeeDistricts = (EDesignation)req.designationID != EDesignation.Territory_Sales_Officer ? new List<TblEmployeeDistrictAssignment>() : assignDistrict(req.districtIDs),
                    EmployeeRegions = (EDesignation)req.designationID != EDesignation.Regional_Sales_Manager ? new List<TblEmployeeRegionAssignment>() : assignRegion(req.regionIDs)
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

                    #region Adding Districts / Regions 
                    if (employee.Designation == EDesignation.Territory_Sales_Officer)
                    {
                        employee.EmployeeDistricts.ForEach(x => x.Status = EActivityStatus.DeActive);
                        employee.EmployeeDistricts.AddRange(assignDistrict(req.districtIDs));
                    }
                    else if (employee.Designation == EDesignation.Regional_Sales_Manager)
                    {
                        employee.EmployeeRegions.ForEach(x => x.Status = EActivityStatus.DeActive);
                        employee.EmployeeRegions.AddRange(assignRegion(req.regionIDs));
                    }

                    await _repoWrapper.UserRepo.updateUser(employee);
                    await _repoWrapper.SaveAsync();
                    #endregion

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
                TblUser user = await _repoWrapper.UserRepo.getUserDetailByUserID(req.userID);
                if (user == null)
                    throw new AmazonFarmerException(_exceptions.userNotFound);
                else
                {
                    user.CNICNumber = req.cnicNumber;
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

                    if (user.Designation == EDesignation.Territory_Sales_Officer)
                    {
                        user.EmployeeDistricts.ForEach(x => x.Status = EActivityStatus.DeActive);
                        user.EmployeeDistricts.AddRange(assignDistrict(req.districtIDs));
                    }
                    else if (user.Designation == EDesignation.Regional_Sales_Manager)
                    {
                        user.EmployeeRegions.ForEach(x => x.Status = EActivityStatus.DeActive);
                        user.EmployeeRegions.AddRange(assignRegion(req.regionIDs));
                    }

                    await _repoWrapper.UserRepo.updateUser(user);
                    await _repoWrapper.SaveAsync();
                }
            }
            return response;
        }
        [HttpPut("updateFarmer")]
        public async Task<JSONResponse> UpdateFarmer(updateFarmerDTO req)
        {
            JSONResponse resp = new JSONResponse();
            TblUser user = await _repoWrapper.UserRepo.getUserDetailByUserID(req.userID);
            user.PhoneNumber = req.phoneNumber;
            user.FarmerProfile.FirstOrDefault().CellNumber = req.phoneNumber;
            user.Email = req.emailAddress;
            user.NormalizedEmail = req.emailAddress.ToUpper();
            user.isAccountLocked = req.isLocked;
            user.Active = req.status ? EActivityStatus.Active : EActivityStatus.DeActive;
            await _repoWrapper.UserRepo.updateUser(user);
            if (!string.IsNullOrEmpty(user.FarmerProfile.FirstOrDefault().SAPFarmerCode))
            {
                var wsdlResponse = await CallUpdateCustomerWSDL(user.FarmerProfile.FirstOrDefault(), user);
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
            return resp;
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
            //else if (req.designationID == 0)
            //    throw new AmazonFarmerException(_exceptions.designationIDRequired);
            else if (!Regex.IsMatch(req.emailAddress, pattern))
                throw new AmazonFarmerException(_exceptions.emailRegexExpressionFails);
        }

        [AllowAnonymous]
        [HttpPost("getUsers")]
        public async Task<APIResponse> GetUsers(ReportPagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<TblUser> users = _repoWrapper.UserRepo.getUsers();

            //removing farmers from list
            users = users.Where(x => x.Designation != null);

            //filtering deleted users
            if (req.rootID == 0)
            {
                users = users.Where(x => x.Active == EActivityStatus.Deleted);
            }
            //filtering active or inactive users
            else if (req.rootID == 1)
            {
                users = users.Where(x => x.Active != EActivityStatus.Deleted);
            }
            //sorting 
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("userID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.Id);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.Id);
                    }
                }
                else if (req.sortColumn.Contains("firstName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.FirstName);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.FirstName);
                    }
                }
                else if (req.sortColumn.Contains("lastName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.LastName);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.LastName);
                    }
                }
                else if (req.sortColumn.Contains("phoneNumber"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.PhoneNumber);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.PhoneNumber);
                    }
                }
                else if (req.sortColumn.Contains("email"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.Email);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.Email);
                    }
                }
                else if (req.sortColumn.Contains("cnic"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.CNICNumber);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.CNICNumber);
                    }
                }
                else if (req.sortColumn.Contains("userName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.UserName);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.UserName);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.Active);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.Active);
                    }
                }
                else if (req.sortColumn.Contains("lockedOutEnabled"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.isAccountLocked);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.isAccountLocked);
                    }
                }

            }
            else
            {
                // on default users will be sorted by userID desc
                users = users.OrderByDescending(x => x.Id);
            }
            //search Term filteration
            if (!string.IsNullOrEmpty(req.search))
            {
                users = users.Where(x =>
                x.FirstName.ToLower().Contains(req.search.ToLower()) ||
                (x.LastName != null && x.LastName.ToLower().Contains(req.search.ToLower())) ||
                (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(req.search.ToLower())) ||
                (x.Email != null && x.Email.ToLower().Contains(req.search.ToLower())) ||
                (x.CNICNumber != null && x.CNICNumber.ToLower().Contains(req.search.ToLower())) ||
                (x.UserName != null && x.UserName.ToLower().Contains(req.search.ToLower()))
                );
            }
            //designation wise filteration
            if (!string.IsNullOrEmpty(req.search1))
            {
                users = users.Where(x => x.Designation != null && x.Designation == (EDesignation)(Convert.ToInt32(req.search1)));
            }
            InResp.totalRecord = users.Count();
            users = users.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = users.Count();
            var t = users.ToList();
            InResp.list = await users.Select(x => new
            {
                userID = x.Id,
                firstName = x.FirstName,
                lastName = x.LastName ?? string.Empty,
                phoneNumber = x.PhoneNumber ?? string.Empty,
                email = x.Email ?? string.Empty,
                cnic = x.CNICNumber ?? string.Empty,
                userName = x.UserName ?? string.Empty,
                status = (int)x.Active,
                lockedOutEnabled = x.isAccountLocked,
                designation = x.Designation != null ? string.IsNullOrEmpty(ConfigExntension.GetEnumDescription(x.Designation)) ? "Farmer" : ConfigExntension.GetEnumDescription(x.Designation) : "Farmer"
            }).ToListAsync();
            response.response = InResp;
            return response;
        }

        [AllowAnonymous]
        [HttpPost("getFarmers")]
        public async Task<APIResponse> GetFarmers(ReportPagination_Req req)
        {
            APIResponse response = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();
            IQueryable<TblUser> users = _repoWrapper.UserRepo.getUsers();

            //removing users from list
            users = users.Where(x => x.Designation == null);

            //filtering deleted users
            if (req.rootID == 0)
            {
                users = users.Where(x => x.Active == EActivityStatus.Deleted);
            }
            //filtering active or inactive users
            else if (req.rootID == 1)
            {
                users = users.Where(x => x.Active != EActivityStatus.Deleted);
            }
            //sorting 
            if (!string.IsNullOrEmpty(req.sortColumn))
            {
                if (req.sortColumn.Contains("userID"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.Id);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.Id);
                    }
                }
                else if (req.sortColumn.Contains("firstName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.FirstName);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.FirstName);
                    }
                }
                else if (req.sortColumn.Contains("lastName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.LastName);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.LastName);
                    }
                }
                else if (req.sortColumn.Contains("phoneNumber"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.PhoneNumber);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.PhoneNumber);
                    }
                }
                else if (req.sortColumn.Contains("email"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.Email);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.Email);
                    }
                }
                else if (req.sortColumn.Contains("cnic"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.CNICNumber);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.CNICNumber);
                    }
                }
                else if (req.sortColumn.Contains("userName"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.UserName);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.UserName);
                    }
                }
                else if (req.sortColumn.Contains("status"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.Active);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.Active);
                    }
                }
                else if (req.sortColumn.Contains("lockedOutEnabled"))
                {
                    if (req.sortOrder.Contains("ASC"))
                    {
                        users = users.OrderBy(x => x.isAccountLocked);
                    }
                    else
                    {
                        users = users.OrderByDescending(x => x.isAccountLocked);
                    }
                }

            }
            else
            {
                // on default users will be sorted by userID desc
                users = users.OrderByDescending(x => x.Id);
            }
            //search Term filteration
            if (!string.IsNullOrEmpty(req.search))
            {
                users = users.Where(x =>
                x.FirstName.ToLower().Contains(req.search.ToLower()) ||
                (x.LastName != null && x.LastName.ToLower().Contains(req.search.ToLower())) ||
                (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(req.search.ToLower())) ||
                (x.Email != null && x.Email.ToLower().Contains(req.search.ToLower())) ||
                (x.CNICNumber != null && x.CNICNumber.ToLower().Contains(req.search.ToLower())) ||
                (x.UserName != null && x.UserName.ToLower().Contains(req.search.ToLower()))
                );
            }
            //designation wise filteration
            if (!string.IsNullOrEmpty(req.search1))
            {
                users = users.Where(x => x.Designation != null && x.Designation == (EDesignation)(Convert.ToInt32(req.search1)));
            }
            InResp.totalRecord = users.Count();
            users = users.Skip(req.pageNumber * req.pageSize)
                         .Take(req.pageSize);
            InResp.filteredRecord = users.Count();
            var t = users.ToList();
            InResp.list = await users.Select(x => new
            {
                userID = x.Id,
                firstName = x.FirstName,
                lastName = x.LastName ?? string.Empty,
                phoneNumber = x.PhoneNumber ?? string.Empty,
                email = x.Email ?? string.Empty,
                cnic = x.CNICNumber ?? string.Empty,
                userName = x.UserName ?? string.Empty,
                status = (int)x.Active,
                lockedOutEnabled = x.isAccountLocked,
                designation = x.Designation != null ? string.IsNullOrEmpty(ConfigExntension.GetEnumDescription(x.Designation)) ? "Farmer" : ConfigExntension.GetEnumDescription(x.Designation) : "Farmer"
            }).ToListAsync();
            response.response = InResp;
            return response;
        }

        [HttpGet("getUserDetails/{userID}")]
        public async Task<APIResponse> GetUserDetails(string userID)
        {
            APIResponse response = new APIResponse();
            TblUser user = await _repoWrapper.UserRepo.getUserDetailByUserID(userID);
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
                designation = user.Designation != null && user.Designation != 0 ? ConfigExntension.GetEnumDescription(user.Designation) : ConfigExntension.GetEnumDescription(ERoles.Farmer),
                role = user.FarmerRoles != null && user.FarmerRoles.Count() > 0 ? user.FarmerRoles.FirstOrDefault().Role.Name : string.Empty,
                email = user.Email ?? string.Empty,
                phoneNumber = user.PhoneNumber ?? string.Empty,
                dateOfBirth = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().DateOfBirth : string.Empty,
                fatherName = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().FatherName : string.Empty,
                strnNumber = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().STRNNumber : string.Empty,
                cnicNumber = user.CNICNumber ?? string.Empty,
                sapFarmerCode = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().SAPFarmerCode : string.Empty,
                cnicAttachment = user.UserAttachments != null ? user.UserAttachments
                .Where(y => y.Attachment.AttachmentTypes.AttachmentType == EAttachmentType.User_CNIC_Document)
                .Select(y => new uploadAttachmentResp
                {
                    id = y.Attachment.ID,
                    type = y.Attachment.FileType,
                    name = y.Attachment.Name,
                    guid = y.Attachment.Guid.ToString()
                }).ToList() : new List<uploadAttachmentResp>(),
                ntnNumber = user.FarmerProfile != null && user.FarmerProfile.Count() > 0 ? user.FarmerProfile.FirstOrDefault().NTNNumber : string.Empty,
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
                lockedOutEnabled = user.isAccountLocked,
                regions = user.EmployeeRegions == null || user.EmployeeRegions.Count() <= 0 ? [] : user.EmployeeRegions.Where(er => er.Status == EActivityStatus.Active).Select(er => er.RegionID
                //{
                //    regionID = er.RegionID,
                //    region = er.Region?.Name,
                //}
                ).ToList(),
                districts = user.EmployeeDistricts == null || user.EmployeeDistricts.Count() <= 0 ? [] : user.EmployeeDistricts.Where(er => er.Status == EActivityStatus.Active).Select(er => er.DitrictID
                //{
                //    districtID = er.DitrictID,
                //    district = er.District?.Name
                //}
                ).ToList()
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

        [HttpPut("changePassword")]
        public async Task<JSONResponse> ChangePassword(changePassword_Req req)
        {
            JSONResponse resp = new JSONResponse();
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

            matchPasswordCriteria(req.password);

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
            return resp;
        }
        [AllowAnonymous]
        [HttpPost("getFarmerProfiles")]
        public async Task<dynamic> GetFarmerProfiles(ReportPagination_Req req)
        {

            APIResponse resp = new APIResponse();
            pagination_Resp InResp = new pagination_Resp();

            List<SP_FarmerDetailsResult> report = await _repoWrapper.PlanRepo.GetSP_FarmerDetailsResult(req.pageNumber, req.pageSize, req.sortColumn, req.sortOrder, req.search, 0);
            if (false)
            {
                return await GetFarmerProfileLink(report);
            }
            else
            {
                if (report != null && report.Count() > 0)
                {
                    InResp.totalRecord = report.Count > 0 ? report.First().TotalRows : 0;
                    InResp.filteredRecord = report.Count();
                    InResp.list = report.Select(x => new FarmerDetailsResponse
                    {
                        applicationStatus = x.ApplicationStatus ?? string.Empty,
                        applicationSubmitDateTime = x.ApplicationSubmitDateTime,
                        farmAcres = x.FarmAcres,
                        farmID = x.FarmID ?? string.Empty,
                        farmCity = x.FarmCity ?? string.Empty,
                        farmerCity = x.FarmerCity ?? string.Empty,
                        farmerCNIC = x.FarmerCNIC ?? string.Empty,
                        farmerName = x.FarmerName ?? string.Empty,
                        farmerCode = x.FarmerCode ?? string.Empty,
                        farmerEmail = x.FarmerEmail ?? string.Empty,
                        farmerUsername = x.FarmerUsername ?? string.Empty,
                        farmerRegion = x.FarmerRegion ?? string.Empty,
                        farmerTehsil = x.FarmerTehsil ?? string.Empty,
                        farmerTerritory = x.FarmerTerritory ?? string.Empty,
                        farmName = x.FarmName ?? string.Empty,
                        farmRegion = x.FarmRegion ?? string.Empty,
                        farmTehsil = x.FarmTehsil ?? string.Empty,
                        farmTerritory = x.FarmTerritory ?? string.Empty,
                        leasedLand = x.LeasedLand ?? string.Empty,
                        noofFarmsAdded = x.NoofFarmsAdded,
                        ownedLand = x.OwnedLand ?? string.Empty,
                        rsm = x.RSM ?? string.Empty,
                        rsmApprovalDateTime = x.RSMApprovalDateTime,
                        totalLand = x.TotalLand,
                        tso = x.TSO ?? string.Empty,
                        tsoApprovalDateTime = x.TSOApprovalDateTime,
                        Address1 = x.Address1,
                        Address2 = x.Address2,
                        latitude = x.latitude,
                        longitude = x.longitude,
                        PhoneNumber = x.PhoneNumber
                    }).ToList();
                }
                else
                {
                    InResp.list = new List<SP_FarmerDetailsResult>();
                }
                resp.response = InResp;
            }
            return resp;
        }
        [AllowAnonymous]
        [HttpGet("downloadFarmerProfile")]
        public async Task<dynamic> DownloadFarmerProfile()
        {
            List<SP_FarmerDetailsResult> report = await _repoWrapper.PlanRepo.GetSP_FarmerDetailsResult(0, 10, "", "", "", 1);
            return await GetFarmerProfileLink(report);
        }
        private async Task<dynamic> GetFarmerProfileLink(List<SP_FarmerDetailsResult> lst)
        {
            List<SP_FarmerDetailsDownload> lst1 = lst.Select(x => new SP_FarmerDetailsDownload
            {
                Address1 = x.Address1,
                Address2 = x.Address2,
                ApplicationStatus = x.ApplicationStatus,
                ApplicationSubmitDateTime = x.ApplicationSubmitDateTime,
                FarmAcres = x.FarmAcres,
                FarmCity = x.FarmCity,
                FarmerCity = x.FarmCity,
                FarmerCNIC = x.FarmerCNIC,
                FarmerName = x.FarmName,
                FarmerCode = x.FarmerCode,
                FarmerEmail = x.FarmerEmail,
                FarmerRegion = x.FarmerRegion,
                FarmerTerritory = x.FarmerTerritory,
                FarmerUsername = x.FarmerUsername,
                FarmID = x.FarmID,
                FarmName = x.FarmName,
                FarmRegion = x.FarmRegion,
                FarmTehsil = x.FarmTehsil,
                FarmTerritory = x.FarmTerritory,
                latitude = x.latitude,
                LeasedLand = x.LeasedLand,
                longitude = x.longitude,
                NoofFarmsAdded = x.NoofFarmsAdded,
                OwnedLand = x.OwnedLand,
                PhoneNumber = x.PhoneNumber,
                RSM = x.RSM,
                RSMApprovalDateTime = x.RSMApprovalDateTime,
                TotalLand = x.TotalLand,
                TSO = x.TSO,
                TSOApprovalDateTime = x.TSOApprovalDateTime
            }).ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new OfficeOpenXml.ExcelPackage();
            //ExcelExtension excelExt = new ExcelExtension();
            package = ExcelExtension.generateTable(lst1.Cast<dynamic>().ToList(), package, ConfigExntension.GetEnumDescription(EDocumentName.FarmerProfile));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=Report.xlsx");
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Concat(ConfigExntension.GetEnumDescription(EDocumentName.FarmerProfile), "-", DateTime.Now.ToString(), ".xlsx"));
            return "";
        }

        [HttpDelete("deleteUser/{userID}")]
        public async Task<JSONResponse> DeleteUser(string userID)
        {
            JSONResponse resp = new JSONResponse();
            TblUser user = await _repoWrapper.UserRepo.getUserByUserID(userID);
            if (user == null)
                throw new AmazonFarmerException(_exceptions.userNotFound);
            else
            {
                string Del = string.Concat("_Deleted_", DateTime.UtcNow.ToString("ddMMyyyyhhmmss"));
                user.UserName = string.Concat(user.UserName, Del);
                user.NormalizedUserName = string.Concat(user.NormalizedUserName, Del);
                user.CNICNumber = string.Concat(user.CNICNumber, Del);
                user.PhoneNumber = string.Concat(user.PhoneNumber, Del);
                user.Email = string.Concat(user.Email, Del);
                user.NormalizedEmail = string.Concat(user.NormalizedEmail, Del);
                user.Active = EActivityStatus.Deleted;

                await _repoWrapper.UserRepo.updateUser(user);
                await _repoWrapper.SaveAsync();
                resp.message = string.Concat(user.FirstName, " has been removed");
            }

            return resp;
        }
        private List<TblEmployeeDistrictAssignment> assignDistrict(int[]? districtID)
        {
            List<TblEmployeeDistrictAssignment> list = new List<TblEmployeeDistrictAssignment>();
            if (districtID != null)
            {
                foreach (var item in districtID)
                {
                    TblEmployeeDistrictAssignment row = new TblEmployeeDistrictAssignment()
                    {
                        DitrictID = item,
                        Status = EActivityStatus.Active
                    };
                    list.Add(row);
                }
            }
            return list;
        }
        private List<TblEmployeeRegionAssignment> assignRegion(int[]? regionID)
        {
            List<TblEmployeeRegionAssignment> list = new List<TblEmployeeRegionAssignment>();
            if (regionID != null)
            {
                foreach (var item in regionID)
                {
                    TblEmployeeRegionAssignment row = new TblEmployeeRegionAssignment()
                    {
                        RegionID = item,
                        Status = EActivityStatus.Active
                    };
                    list.Add(row);
                }
            }
            return list;
        }
        //Function to check password criteria
        private void matchPasswordCriteria(string password)
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

    }
}
