using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace AmazonFarmer.Core.Application.Exceptions
{
    public class AmazonFarmerException : Exception
    {
        public AmazonFarmerException(string message) : base(message)

        {
        }
    }
    public class _exceptions
    {
        public static string getExceptionByLanguageCode(string languageCode, string exceptionKey)
        {
            string resourceFileName = languageCode.ToUpper() == "en" ? "EnglishResources.resx" : "UrduResources.resx";
            ResourceManager resourceManager = new ResourceManager($"AmazonFarmer.Core.Application.Exceptions.{resourceFileName}", typeof(_exceptions).Assembly);
            return resourceManager.GetString(exceptionKey) ?? "Error message not found";
        }

        public const string daterangeIsNotValid = "Date range is not valid";

        public const string regionNotFound = "Region Not found.";
        public const string regionAlreadyExist = "Region with the same Name or code already exist.";

        public const string districtNotFound = "District Not found.";
        public const string districtAlreadyExist = "District with the same Name or code already exist.";
        public const string districtLanguageAlreadyExist = "District with the same language code already exist.";

        public const string cityNotFound = "City not found.";
        public const string cityAlreadyExist = "City with the same name or code already exist..";
        public const string cityLanguageAlreadyExist = "City with the same language code already exist.";

        public const string cropAlreadyExist = "Crop with the same name already exist.";
        public const string cropNotFound = "Crop not found.";

        public const string serviceAlreadyExist = "Service with the same name or code already exist.";
        public const string serviceTranslationAlreadyExist = "Service Translation with the name already exist.";
        public const string serviceNotFound = "Service not found.";
        public const string serviceTranslationNotFound = "Service Translation not found.";

        public const string productNotFound = "Product not found.";
        public const string productAlreadyExist = "Product with the same name or code already exist.";

        public const string tehsilNotFound = "Tehsil not found.";
        public const string tehsilAlreadyExist = "Tehsil with the same name or code already exist.";

        public const string seasonAlreadyExist = "Season with the same name already exist.";

        public const string emailRegexExpressionFails = "Invalid email format. Please try again.";
        public const string errorOccuredWhileAddingUser = "Error adding user details. Please retry.";
        public const string userAlreadyExist = "User already exists. Please login.";
        public const string invalidAppVersion = "Update your app to the latest version.";
        public const string invalidMethod = "Invalid request. Please try again.";
        public const string invalidEmail = "Username or Password isn't valid";
        public const string onlyAdmin = "An error occurred. Please try again.";
        public const string invalidPassword = "Invalid Username or Password";
        public const string verifyPasswordInvalid = "Invalid Password";
        public const string deactiveUser = "An error occurred. Please try again.";
        public const string nullUsernameOrPassword = "Username or Password can not be empty.";
        public const string firstnameRequired = "First Name is Required";
        public const string phoneRequired = "Mobile Number is Required ";
        public const string phoneAlreadyTaken = "Mobile Number is already taken";
        public const string emailRequired = "Email Address is Required";
        public const string emailIsTaken = "Email Address is Already Taken. Try Another.";
        public const string passwordRequired = "Password is Required";
        public const string passwordMinLength = "Your Password must have 8 or more characters";
        public const string specialCharacterRequired = "Your Password must have 1 special characters";
        public const string passwordOneUpperCaseRequired = "Your Password must have atleast 1 uppercase letter";
        public const string passwordOneLowerCaseRequired = "Your Password must have atleast 1 lowercase letter";
        public const string passwordOneDigitRequired = "Your Password must have atleast one number";
        public const string orderIDRequired = "Order ID is required";
        public const string orderNotFound = "An error occurred. Please try again.";
        public const string orderNotPaid = "Order is not paid";
        public const string orderExpectedDeliveryDateNotFound = "An error occurred. Please try again.";
        public const string orderExpired = "Order is expired";
        public const string alreadyPaid = " Order is already paid";
        public const string advancePaymentNotDone = "Advance Payment is not done";
        public const string advanceOrderNotFound = "An error occurred. Please try again.";
        public const string orderPaymentPendingLastOrder = "Payment in Processing";
        public const string productIDRequired = "An error occurred. Please try again.";
        public const string qtyRequired = "Add atleast 1 product in the cart to proceed";
        public const string authorityletterBagQuantityReached = "you can not add more products in the authority letter";
        public const string palindromeFound = "An error occurred. Please try again.";
        public const string consecutivityFound = "An error occurred. Please try again.";
        public const string ristrictedCharactersFound = "An error occurred. Please try again.";
        public const string platformRequired = "An error occurred. Please try again.";
        public const string confirmPasswordRequired = " Confirm your password.";
        public const string confirmPasswordNotMatch = "The passwords you entered do not match";
        public const string languageAlreadyExist = "An error occurred. Please try again.";
        public const string languageNotFound = "An error occurred. Please try again.";
        public const string languageCodeRequired = "An error occurred. Please try again.";
        public const string dateOfBirthRequired = "Date of Birth is required ";
        public const string cellNumberRequired = "Mobile Number is Required ";
        public const string ntnNumberRequired = "NTN Number is Required ";
        public const string ntnNumberIsTaken = "NTN Number is already taken";
        public const string attachmentRequired = "Attach Atleast 1 document";
        public const string attachmentNotValid = "An error occurred. Please try again.";
        public const string attachmentNotFound = "Attachment not found";
        public const string attachmentDoesnotBelongs = "An error occurred. Please try again.";
        public const string attachmentConsumedAlready = "An error occurred. Please try again.";
        public const string authorityLetterAttachmentRequired = "Attachment Required";
        public const string authorityLetterQtyReached = "Max Quantity Reached";
        public const string authorityLetterNotFound = "Authority Letter not found";
        public const string fatherNameRequired = "Father Name Required";
        public const string cnicNumberRequired = "CNIC Required ";
        public const string cnicNumberNotValid = "CNIC Number is invalid";
        public const string strnNumberRequired = "STRN Number Required ";
        public const string invalidOrderProduct = "Invalid Order Product";
        public const string addressRequired = "Address Required ";
        public const string requiredFieldsMissing = "Field Missing";
        public const string farmerCommentRequired = "Farmer's Comment Required ";
        public const string isPlanEditable = "Plan Change is allowed";
        public const string planNotInValidState = "Plan Change is not allowed";
        public const string cnicNumberDoesNotMatach = "CNIC number does not match";
        public const string authorityLetterUpdatedAlready = "Authority Letter already updated";
        public const string textNotFoundInImage = "Invalid Image";
        public const string imageDoesNotSeemsLikeCNIC = "Invalid Image";
        public const string invalidAttachmentType = "An error occurred. Please try again.";
        public const string internalServerError = "An error occurred. Please try again.";
        public const string truckerRequired = "Trucker No. required";
        public const string biltyNumberRequired = "Bilty No. Required";
        public const string reasonRequired = "Please Select Reeason for Reverting the Request";
        public const string reasonIDNotFound = "Please select a Reason to proceed";
        public const string reasonNotFound = "Invalid reason ID";
        public const string cityRequired = "City Required";
        public const string tehsilRequired = "Tehsil Required";
        public const string districtRequired = "Distriect Required ";
        public const string usernameRequired = "Username Required ";
        public const string leasedLandExteeds = "Leased Land Should be less than or equal to Owned Land ";
        public const string leasedLandMinMaxLengthExteeds = "Leased Land cannot be more than 100000";
        public const string ownedLandMinMaxLengthExteeds = "Owned Land cannot be more than 100000";
        public const string leasedAndOwnedLandCannotBeZero = "Leased and Owned Land both cannot be 0";
        public const string isTermsAcceptRequired = "Please Accept the Terms & Conditions to Proceed";
        public const string usernameIsTaken = "Username already taken. Please try again.";
        public const string userIDNotFound = "An error occurred. Please try again.";
        public const string userIDorLanguageCodeNotFound = "An error occurred. Please try again.";
        public const string cnicIsTaken = "CNIC already taken. Please try again.";
        public const string cnicNotValid = "Invalid CNIC";
        public const string ntnIsTaken = "NTN already taken. Please try again";
        public const string strnIsTaken = "STRN already taken. Please try again";
        public const string cellNumberIsTaken = "Mobile Number is taken. Please try again";
        public const string lessThen18 = "An error occurred. Please try again.";
        public const string invalidOTP = "Invalid OTP";
        public const string expiredOTP = "OTP is invalid or expired, please press resent OTP for the new code";
        public const string otpRequired = "OTP Required";
        public const string canUseDataRequired = "Please Accept the Terms & Conditions to Proceed";
        public const string isAllInformationCorrect = "Please Accept the Terms & Conditions to Proceed";
        public const string atleastOneFarm = "Please select atleast one farm to procced";
        public const string OnePrimaryFarmRequired = "Please select one farm as primary farm";
        public const string invalidUsername = "Invalid Username ";
        public const string sapFarmerCodeNotFound = "An error occurred. Please try again.";
        public const string sapOrderIDNotFound = "An error occurred. Please try again.";
        public const string invalidUCNIC = "Invalid CNIC";
        public const string userNotFound = "User Not Found";
        public const string userProfileNotFound = "User Profile Not Found";
        public const string nullApplicationID = "An error occurred. Please try again.";
        public const string cropsRequired = "Please select atleast one Crop to proceed";
        public const string productsRequired = "Please add atleast one product in the cart";
        public const string seasonRequired = "Please select a season";
        public const string seasonNotFound = "Season not found";
        public const string seasonRangeOverlaping = "Seems like season starting & ending month overlaping";
        public const string usernameTaken = "Username already taken. Please try again.";
        public const string statusIDRequired = "An error occurred. Please try again.";
        public const string farmNotFound = "An error occurred. Please try again.";
        public const string warehouseIDNotFound = "An error occurred. Please try again.";
        public const string warehouseNotFound = "An error occurred. Please try again.";
        public const string farmIDRequired = "An error occurred. Please try again.";
        public const string farmNameRequired = "An error occurred. Please try again.";
        public const string approvalIDRequired = "An error occurred. Please try again.";
        public const string farmNotAuthorized = "An error occurred. Please try again.";
        public const string farmInValidState = "An error occurred. Please try again.";
        public const string userNotAuthorized = "An error occurred. Please try again.";
        public const string profileApproved = "An error occurred. Please try again.";
        public const string APINotAuthorized = "An error occurred. Please try again.";
        public const string applicationIDRequired = "An error occurred. Please try again.";
        public const string planIDRequired = "An error occurred. Please try again.";
        public const string planNotFound = "An error occurred. Please try again.";
        public const string planEPlanApprovalRejectionTypeNotFound = "An error occurred. Please try again.";
        public const string authorityLetterIDNotFound = "An error occurred. Please try again.";
        public const string authorityLetterNotInValidState = "An error occurred. Please try again.";
        public const string invoiceNotFoundInSAP = "An error occurred. Please try again.";
        public const string seemsLikeFarmApproved = "Congratulations, your farm is approved !";
        public const string designationIDRequired = "An error occurred. Please try again.";
        public const string userAddedRoleNotAssigned = "An error occurred. Please try again.";
        public const string notFound = "Not Found.";

        //SMS Request
        public const string smsResponseNot200 = "SMS in't success status code";
        public const string smsHasNotSent = "SMS not end";

        public const string createCustomerFailure = "An error occurred. Please try again.";
        public const string pricingNotMaintained = "An error occurred";
        public const string rejectionNotAllowed = "An error occurred. Please try again.";
        public const string orderApprovalFailure = "An error occurred. Please try again.";

        //SAP Validation
        //Create Customer
        public const string nameLengthExteed_SAPValidation = "Name Should be less than 40 characters";
        public const string emailAddressLengthExteed_SAPValidation = "email address should be less than 60 charcters";
        public const string address1LengthExteed_SAPValidation = "Address 1 Should be less than 60 characters ";
        public const string address2LengthExteed_SAPValidation = "Address 2 should be less than 60 characters ";

        public const string noImpactOnApprovalError = "An error occurred. Please try again.";
        public const string noImpactOnRejectionError = "An error occurred. Please try again.";

        //Agrilift.io
        //Login
        public const string agriliftRetrieveFailure = "An error occurred. Please try again.";
        public const string agriliftDeserializationError = "An error occurred. Please try again.";
        public const string agrilift116 = "An error occurred. Please try again.";

        //Farmdar.ai
        //Login
        public const string farmdarLoginFailure = "An error occurred. Please try again.";


        public const string productCategoryNotFound = "An error occurred. Please try again.";

        //File exception not value
        public const string fileExtensionNotValid = "Invalid file format.";


    }
}
