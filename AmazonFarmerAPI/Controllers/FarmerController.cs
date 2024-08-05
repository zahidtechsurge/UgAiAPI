using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmerAPI.Extensions;
using AmazonFarmerAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
        /// <summary>
        /// Constructor for initializing the FarmerController.
        /// </summary>
        /// <param name="repoWrapper">The repository wrapper instance for accessing data.</param>
        public FarmerController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
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
            try
            {
                // Check if the app version is valid
                if (req.appVersion < Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
                    throw new Exception(_exceptions.invalidAppVersion);
                // Check if the first name is provided
                else if (string.IsNullOrEmpty(req.firstName))
                    throw new Exception(_exceptions.firstnameRequired);
                // Check if the CNIC number is provided and valid
                else if (string.IsNullOrEmpty(req.cnicNumber) || req.cnicNumber.Replace("-", "").Length != 13)
                    throw new Exception(_exceptions.cnicNumberRequired);
                // Check if the platform is provided
                else if (string.IsNullOrEmpty(req.platform))
                    throw new Exception(_exceptions.platformRequired);
                // Check if the date of birth is provided
                else if (string.IsNullOrEmpty(req.dateOfBirth))
                    throw new Exception(_exceptions.dateOfBirthRequired);
                // Check if the cell number is provided
                else if (string.IsNullOrEmpty(req.cellNumber))
                    throw new Exception(_exceptions.cellNumberRequired);
                // Check if the email address is provided
                else if (string.IsNullOrEmpty(req.emailAddress))
                    throw new Exception(_exceptions.emailRequired);
                // Check if the STRN number is provided
                else if (string.IsNullOrEmpty(req.strnNumber))
                    throw new Exception(_exceptions.strnNumberRequired);
                // Check if the NTN number is provided
                else if (string.IsNullOrEmpty(req.ntnNumber))
                    throw new Exception(_exceptions.ntnNumberRequired);
                // Check if CNIC attachments are provided
                else if (req.cnicAttachments == null || req.cnicAttachments.Count() <= 0)
                    throw new Exception(_exceptions.attachmentRequired);
                // Check if NTN attachments are provided
                else if (req.ntnAttachments == null || req.ntnAttachments.Count() <= 0)
                    throw new Exception(_exceptions.attachmentRequired);
                // Check if father's name is provided
                else if (string.IsNullOrEmpty(req.fatherName))
                    throw new Exception(_exceptions.fatherNameRequired);
                // Check if address line 1 is provided
                else if (string.IsNullOrEmpty(req.address1))
                    throw new Exception(_exceptions.addressRequired);
                // Check if city ID is provided
                else if (req.cityID == null || req.cityID == 0)
                    throw new Exception(_exceptions.cityRequired);
                // Check if district ID is provided
                else if (req.districtID == null || req.districtID == 0)
                    throw new Exception(_exceptions.districtRequired);
                // Check if username is provided
                else if (string.IsNullOrEmpty(req.username))
                    throw new Exception(_exceptions.usernameRequired);
                // Check if leased land acreage is within valid range
                else if (req.leasedLandAcreage <= 0 || req.leasedLandAcreage > 100000)
                    throw new Exception(_exceptions.leasedLandMinMaxLengthExteeds);
                // Check if password is provided
                else if (string.IsNullOrEmpty(req.password))
                    throw new Exception(_exceptions.passwordRequired);
                // Check if password confirmation is provided
                else if (string.IsNullOrEmpty(req.confirmPassword))
                    throw new Exception(_exceptions.confirmPasswordRequired);
                // Check if terms are accepted
                else if (!req.isTermsAccepted)
                    throw new Exception(_exceptions.isTermsAcceptRequired);
                // Check if selected language code is provided
                else if (string.IsNullOrEmpty(req.selectedLangCode))
                    throw new Exception(_exceptions.languageCodeRequired);
                // Check if password and confirmation match
                else if (req.password != req.confirmPassword)
                    throw new Exception(_exceptions.confirmPasswordNotMatch);
                // Check if user is older than 18
                else if (!IsOlderThan18(DateTime.ParseExact(req.dateOfBirth, "yyyy-MM-dd", null)))
                    throw new Exception(_exceptions.lessThen18);
                else if (matchPasswordCriteria(req.password))
                {
                    // Signup the farmer user
                    UserDTO user = await _repoWrapper.UserRepo.signupFarmer(req);
                    if (user != null)
                    {
                        try
                        {
                            // Upload CNIC attachments
                            await uploadSignupAttachment(req.cnicAttachments, user.userID, EAttachmentType.User_CNIC_Document);
                            // Upload NTN attachments
                            await uploadSignupAttachment(req.ntnAttachments, user.userID, EAttachmentType.User_NTN_Document);
                        }
                        catch (Exception ex)
                        {
                            // Handle attachment upload exception
                        }

                        // Generate OTP code
                        string OTPCode = OTPExtension.GenerateOTP();

                        // Prepare email details
                        emailDTO email = new emailDTO()
                        {
                            body = "Your OTP Code is:" + OTPCode,
                            isHTML = true,
                            name = req.firstName,
                            toUser = req.emailAddress,
                            fromUser = req.emailAddress,
                            subject = "OTP Verification Code",
                            EmailType = Email_Type.OTPEmail
                        };
                        // Send OTP email
                        EmailExtension.sendEmail(email);

                        // Set OTP for the user
                        await _repoWrapper.UserRepo.setOTP(user.userID, OTPCode);

                        // Generate JWT token
                        TokenResponse token = JWTService.GenerateJwt(
                            user,
                            ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                            ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                            Convert.ToInt32(ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays"))
                        );
                        // Prepare response
                        resp.response = token;
                        resp.message = "OTP Sent";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle signup exception
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
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
            try
            {
                // Get the user ID from claims
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // Verify OTP
                if (!(await _repoWrapper.UserRepo.verifyOTP(userID, req.otpCode)))
                    throw new Exception(_exceptions.invalidOTP);
                else
                {
                    resp.message = "OTP Verification";
                }
            }
            catch (Exception ex)
            {
                // Handle OTP verification exception
                resp.isError = true;
                resp.message = ex.Message;
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
            APIResponse resp = new APIResponse();
            try
            {
                // Get the user ID from claims
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // Retrieve user profile information
                resp.response = await _repoWrapper.UserRepo.getUserProfileByUserID(userID);
            }
            catch (Exception ex)
            {
                // Handle profile retrieval exception
                resp.isError = true;
                resp.message = ex.Message;
            }
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
            try
            {
                // Check if CNIC number is provided
                if (string.IsNullOrEmpty(req.cnicNumber))
                    throw new Exception(_exceptions.cnicNumberRequired);

                // Check if the provided CNIC number is valid and retrieve user information
                UserDTO userInfo = await _repoWrapper.UserRepo.isUsernameValid(req);

                // If user information is not found, throw an exception
                if (userInfo == null)
                    throw new Exception(_exceptions.cnicNumberNotValid);
                else
                {
                    // Generate OTP code
                    string OTPCode = OTPExtension.GenerateOTP();

                    // Create email containing OTP code
                    emailDTO email = new emailDTO()
                    {
                        body = "We've received a forget password request,Here's the OTP Code:" + OTPCode,
                        isHTML = true,
                        name = userInfo.firstName,
                        toUser = userInfo.email,
                        fromUser = userInfo.email,
                        subject = "OTP Verification Code",
                        EmailType = Email_Type.ForgetPasswordEmail
                    };

                    // Send email containing OTP code
                    EmailExtension.sendEmail(email);

                    // Set OTP code for user
                    await _repoWrapper.UserRepo.setOTP(userInfo.userID, OTPCode);

                    // Set success message
                    resp.message = "OTP Sent";
                }
            }
            catch (Exception ex)
            {
                // Handle forget password process exception
                resp.isError = true;
                resp.message = ex.Message;
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
            try
            {
                // Check if the username (CNIC number) already exists
                if (await _repoWrapper.UserRepo.isUsernameExist(req.cnicNumber))
                {
                    // If username is already taken, throw an exception
                    throw new Exception(_exceptions.usernameTaken);
                }
                else
                {
                    // Set message indicating username availability
                    resp.message = "username available";
                }
            }
            catch (Exception ex)
            {
                // Handle username verification exception
                resp.isError = true;
                resp.message = ex.Message;
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
            try
            {
                if (string.IsNullOrEmpty(req.cnicNumber))
                    throw new Exception(_exceptions.cnicNumberRequired);
                else if (string.IsNullOrEmpty(req.otpCode))
                    throw new Exception(_exceptions.otpRequired);

                // Verify the OTP code for the provided CNIC number
                if (await _repoWrapper.UserRepo.forgetPasswordOTP(req.cnicNumber, req.otpCode))
                {
                    // If OTP is verified, set the response message
                    resp.message = "OTP Verified";
                }
            }
            catch (Exception ex)
            {
                // Handle forget password OTP verification exception
                resp.isError = true;
                resp.message = ex.Message;
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
            try
            {
                // Check if the current password is provided
                if (string.IsNullOrEmpty(req.currentPassword))
                    throw new Exception(_exceptions.passwordRequired);

                // Check if the new password is provided
                if (string.IsNullOrEmpty(req.password))
                    throw new Exception(_exceptions.passwordRequired);

                // Check if the confirm password is provided
                else if (string.IsNullOrEmpty(req.confirmPassword))
                    throw new Exception(_exceptions.confirmPasswordRequired);

                // Check if the new password matches the confirm password
                else if (req.password != req.confirmPassword)
                    throw new Exception(_exceptions.confirmPasswordNotMatch);

                else
                {
                    // Change the user password
                    await _repoWrapper.UserRepo.changePassword(req, (User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    resp.message = "Password Changed";
                }
            }
            catch (Exception ex)
            {
                // Handle password change exception
                resp.isError = true;
                resp.message = ex.Message;
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
            try
            {
                // Check if the app version is valid
                if (req.appVersion < Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
                    throw new Exception(_exceptions.invalidAppVersion);

                // Check if username or password is null or empty
                else if (string.IsNullOrEmpty(req.username) || string.IsNullOrEmpty(req.password))
                    throw new Exception(_exceptions.nullUsernameOrPassword);

                // Check if the platform is provided
                else if (string.IsNullOrEmpty(req.platform))
                    throw new Exception(_exceptions.platformRequired);

                else
                {
                    // Get user information by username and password
                    UserDTO DBResp = await _repoWrapper.UserRepo.getUserByUsernameandPassword(req);

                    if (DBResp == null)
                        throw new Exception(_exceptions.userNotFound);

                    // Generate JWT token
                    TokenResponse token = JWTService.GenerateJwt(
                        DBResp,
                        ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                        ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                        Convert.ToInt32(
                            ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays")
                        )
                    );

                    // Set the token in the API response
                    resp.response = token;
                }
            }
            catch (Exception ex)
            {
                // Handle sign-in exception
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
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
            try
            {
                // Check if CNIC number is provided
                if (string.IsNullOrEmpty(req.cnicNumber))
                    throw new Exception(_exceptions.cnicNumberRequired);
                else
                {
                    // Get user information by CNIC number
                    UserDTO DBResp = await _repoWrapper.UserRepo.getUserInfoByCNIC(req.cnicNumber);

                    // If user exists and has a valid user ID
                    if (DBResp != null && !string.IsNullOrEmpty(DBResp.userID))
                    {
                        // Generate OTP code
                        string OTPCode = OTPExtension.GenerateOTP();

                        // Prepare email with OTP code
                        emailDTO email = new emailDTO()
                        {
                            body = "Your OTP Code is: " + OTPCode,
                            isHTML = true,
                            name = DBResp.firstName,
                            toUser = DBResp.email,
                            fromUser = DBResp.email,
                            subject = "OTP Verification Code",
                            EmailType = Email_Type.OTPEmail
                        };

                        // Send the email with OTP code
                        EmailExtension.sendEmail(email);

                        // Set the OTP in the database for the user
                        await _repoWrapper.UserRepo.setOTP(DBResp.userID, OTPCode);
                    }
                    resp.message = "OTP Sent";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.isError = true;
                resp.message = ex.Message;
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
            try
            {
                // Check if the app version is valid
                if (req.appVersion < Convert.ToDecimal(ConfigExntension.GetConfigurationValue("appVersion")))
                    throw new Exception(_exceptions.invalidAppVersion);
                // Check if CNIC number is provided
                else if (string.IsNullOrEmpty(req.cnicNumber))
                    throw new Exception(_exceptions.cnicNumberRequired);
                // Check if platform is provided
                else if (string.IsNullOrEmpty(req.platform))
                    throw new Exception(_exceptions.platformRequired);
                // Check if OTP code is provided
                else if (string.IsNullOrEmpty(req.otpCode))
                    throw new Exception(_exceptions.otpRequired);
                // Check if password is provided
                else if (string.IsNullOrEmpty(req.password))
                    throw new Exception(_exceptions.passwordRequired);
                // Check if confirm password is provided
                else if (string.IsNullOrEmpty(req.confirmPassword))
                    throw new Exception(_exceptions.confirmPasswordRequired);
                // Check if passwords match
                else if (req.password != req.confirmPassword)
                    throw new Exception(_exceptions.confirmPasswordNotMatch);
                else if(matchPasswordCriteria(req.password))
                {
                    // Set new password
                    await _repoWrapper.UserRepo.newPassword(req);

                    // Get user information by CNIC number
                    UserDTO DBResp = await _repoWrapper.UserRepo.getUserInfoByCNIC(req.cnicNumber);

                    if (DBResp == null)
                        throw new Exception(_exceptions.userNotFound);

                    // Generate JWT token for the user
                    TokenResponse token = JWTService.GenerateJwt(
                        DBResp,
                        ConfigExntension.GetConfigurationValue("Jwt:Secret"),
                        ConfigExntension.GetConfigurationValue("Jwt:Issuer"),
                        Convert.ToInt32(
                            ConfigExntension.GetConfigurationValue("Jwt:ExpirationInDays")
                        )
                    );

                    // Set response with token and success message
                    resp.response = token;
                    resp.message = "Password Changed";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.isError = true;
                resp.message = ex.Message;
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
            try
            {
                // Get all usernames with OTPs
                resp.response = await _repoWrapper.UserRepo.getAllUsernameWithOTPs();
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }


        /// <summary>
        /// Endpoint for approving a farmer account.
        /// </summary>
        /// <param name="req">The request containing the user ID of the farmer account to be approved.</param>
        /// <returns>Returns a JSON response indicating the success or failure of the approval process.</returns>
        [HttpPost("approveFarmer")]
        public async Task<JSONResponse> approveFarmer(approveFarmer_Req req)
        {
            JSONResponse resp = new JSONResponse();
            try
            {
                // Get the ID of the approver
                var approverID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Approve the farmer account
                await _repoWrapper.UserRepo.approveFarmAccountByUserID(req.userID, approverID);
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        [HttpGet("getUserProfile")]
        public async Task<APIResponse> getUserProfile()
        {
            APIResponse resp = new APIResponse();
            try
            {
                // Get the user ID from claims
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userID))
                    throw new AmazonFarmerException(_exceptions.userIDNotFound);
                else
                    resp.response = await _repoWrapper.UserRepo.getUserProfile(userID);
            }
            catch (Exception)
            {
                throw;
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
        private async Task uploadSignupAttachment(List<farmAttachment> attachments, string UserID, EAttachmentType AttachmentType)
        {
            foreach (var attachment in attachments)
            {
                if (!string.IsNullOrEmpty(attachment.content))
                {
                    // Decode the base64 string into a byte array
                    byte[] imageBytes = Convert.FromBase64String(attachment.content);
                    // Generate a unique file name
                    string fileName = attachment.name + DateTime.UtcNow.ToString("yyyy.MM.dd") + "-" + Guid.NewGuid().ToString() + ".png";
                    // Specify the directory where you want to save the image
                    string filePath = Path.Combine(ConfigExntension.GetConfigurationValue("Locations:AttachmentURL"), fileName);
                    // Write the image bytes to the file
                    System.IO.File.WriteAllBytes(filePath, imageBytes);
                    // Get the URL of the saved image
                    string imageUrl = $"{ConfigExntension.GetConfigurationValue("Locations:BaseURL")}/Attachments/{fileName}";
                    // Create an AttachmentsDTO object
                    AttachmentsDTO attachmentsDTO = new AttachmentsDTO()
                    {
                        attachmentType = AttachmentType,
                        filePath = imageUrl,
                        fileType = "PNG"
                    };
                    // Upload the attachment
                    tblAttachment attachmentResp = await _repoWrapper.AttachmentRepo.uploadAttachment(attachmentsDTO);
                    // Create an attachAttachment object
                    attachAttachment attachWithUser = new attachAttachment
                    {
                        attachmentID = attachmentResp.ID,
                        userID = UserID
                    };
                    // Attach the uploaded attachment to the user
                    await _repoWrapper.AttachmentRepo.attachUserAttachment(attachWithUser);
                }
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
                throw new Exception(_exceptions.passwordRequired);
            // Minimum length should be at least 8
            else if (password.Length < 8)
                throw new Exception(_exceptions.passwordMinLength);
            // Number of special characters to include 1
            else if (!Regex.IsMatch(password, @"[!@#$%^&*()_+}{:;'?/>.<,|=-]"))
                throw new Exception(_exceptions.specialCharacterRequired);
            // Must contain at least 1 upper case character(s)
            else if (!password.Any(char.IsUpper))
                throw new Exception(_exceptions.passwordOneUpperCaseRequired);

            // Number of numerals to include 1
            else if (!password.Any(char.IsDigit))
                throw new Exception(_exceptions.passwordOneDigitRequired);

            // Must not be a palindrome
            else if (IsPalindrome(password))
                throw new Exception(_exceptions.palindromeFound);

            // Must not contain any character more than twice consecutively
            else if (Regex.IsMatch(password, @"(.)\1{2,}"))
                throw new Exception(_exceptions.consecutivityFound);

            // Must not contain restricted patterns
            else if (ContainsRestrictedPattern(password))
                throw new Exception(_exceptions.ristrictedCharactersFound);

            // Must contain at least 1 lower case character(s)
            else if (!password.Any(char.IsLower))
                throw new Exception(_exceptions.passwordOneLowerCaseRequired);
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
            string[] restrictedPatterns = restrictedWords.Split(","); // Add more patterns as needed
            foreach (string pattern in restrictedPatterns)
            {
                if (password.ToLower().Contains(pattern.ToLower()))
                    return true;
            }
            return false;
        }
    }
}
