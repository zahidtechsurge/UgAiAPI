
using System.Resources;

namespace AmazonFarmer.Core.Application.Exceptions
{
    public static class UrduResources
    {
        //private static readonly ResourceManager _resourceManager = new ResourceManager("AmazonFarmer.Core.Application.Exceptions.UrduResources", typeof(UrduResources).Assembly);

        //public static string GetString(string key)
        //{
        //    return _resourceManager.GetString(key);
        //}
        public const string errorOccuredWhileAddingUser = "صارف کو شامل کرتے وقت ایک خرابی پیش آگئی";
        public const string userAlreadyExist = "صارف پہلے سے موجود ہے۔";
        public const string invalidAppVersion = "Kindly update your application!";
        public const string invalidEmail = "Email or Password isn't valid!";
        public const string invalidPassword = "Email or Password isn't valid!";
        public const string deactiveUser = "User is not in valid state!";
        public const string nullUsernameOrPassword = "username or password can not be null or empty";
        public const string firstnameRequired = "First name is required";
        public const string phoneRequired = "Phone number is required";
        public const string emailRequired = "Email is required";
        public const string passwordRequired = "Password is required";
        public const string passwordMinLength = "Minimum length should be at least 8";
        public const string specialCharacterRequired = "Password must contain atleast One special character";
        public const string passwordOneUpperCaseRequired = "Password must contain atleast 1 upper case";
        public const string passwordOneLowerCaseRequired = "Must contain at least 1 lower case character(s)";
        public const string passwordOneDigitRequired = "Password must contain atleast 1 digit";
        public const string palindromeFound = "Must not be a palindrome";
        public const string consecutivityFound = "Must not contain any character more than twice consecutively";
        public const string ristrictedCharactersFound = "Ristricted Characters Found";
        public const string platformRequired = "Platform is required";
        public const string confirmPasswordRequired = "Confirm password is required";
        public const string confirmPasswordNotMatch = "make sure that the password & confirm password matches";
        public const string languageCodeRequired = "زبان کا کوڈ درکار ہے۔";
        public const string dateOfBirthRequired = "Date of Birth is Required";
        public const string cellNumberRequired = "Cell Number is Required";
        public const string ntnNumberRequired = "NTN Number is Required";
        public const string ntnNumberIsTaken = "NTN Number is taken";
        public const string attachmentRequired = "Atleast 1 attachment is Required";
        public const string fatherNameRequired = "Father Name is Required";
        public const string cnicNumberRequired = "CNIC Number is Required";
        public const string cnicNumberNotValid = "CNIC Number is not valid";
        public const string strnNumberRequired = "STRN Number is Required";
        public const string addressRequired = "Address is Required";
        public const string cityRequired = "City is Required";
        public const string tehsilRequired = "Tehsil is Required";
        public const string districtRequired = "District is Required";
        public const string usernameRequired = "Username is Required";
        public const string leasedLandExteeds = "Leased land can't be greater than owned land!";
        public const string leasedLandMinMaxLengthExteeds = "Leased land can't be less than zero or greater than 1 Lac!";
        public const string isTermsAcceptRequired = "make sure to select the Terms & Conditions is Required";
        public const string usernameIsTaken = "Username is taken already, Try another one";
        public const string userIDNotFound = "Internal Server Error! userID no found";
        public const string userIDorLanguageCodeNotFound = "Internal Server Error! userID or languageCode no found";
        public const string cnicIsTaken = "CNIC is taken already, Try another one";
        public const string ntnIsTaken = "NTN is taken already, Try another one";
        public const string strnIsTaken = "STRN is taken already, Try another one";
        public const string cellNumberIsTaken = "Phone Number is taken already, Try another one";
        public const string lessThen18 = "You are not applicable";
        public const string invalidOTP = "OTP Code isn't valid";
        public const string expiredOTP = "OTP Code isn't valid or Expired, Kindly resend the OTP!";
        public const string otpRequired = "OTP Code Required";
        public const string canUseDataRequired = "It is required to agree to us to using the data";
        public const string isAllInformationCorrect = "It is required to agree that all the information you shared is Correct";
        public const string atleastOneFarm = "Make sure to select atleast one farm";
        public const string OnePrimaryFarmRequired = "Make sure to select one farm as primary";
        public const string invalidUsername = "Username is not valid";
        public const string invalidUCNIC = "CNIC is not valid";
        public const string userNotFound = "User not found";
        public const string nullApplicationID = "Internal Server Error, Unable to get the application ID";
        public const string cropsRequired = "Kindly Select Crops";
        public const string productsRequired = "Kindly add atleast one product in cart";
        public const string seasonRequired = "Kindly Select Season";
        public const string usernameTaken = "Username is already taken!";
        public const string statusIDRequired = "Status ID not found!";
        public const string farmNotFound = "Farm not found!";
        public const string warehouseNotFound = "Warehouse not found!";
        public const string farmIDRequired = "Farm ID not found!";
        public const string approvalIDRequired = "Approval ID not found!";
        public const string farmNotAuthorized = "You're not authorized to perform any action on this request!";
        public const string userNotAuthorized = "You're not authorized to perform any action on this request!";
        public const string applicationIDRequired = "Application ID is required!";
        public const string planIDRequired = "Plan ID is required!";
        public const string planNotFound = "Plan is not found!";
    }
}
