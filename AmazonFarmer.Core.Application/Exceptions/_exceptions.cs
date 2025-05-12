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

        public const string daterangeIsNotValid = "Please enter a valid date range";

        public const string regionNotFound = "Region does not exist";
        public const string regionAlreadyExist = "Region with the same name or code already exists";

        public const string districtNotFound = "District does not exist";
        public const string districtAlreadyExist = "District with the same name or code already exists";
        public const string districtLanguageAlreadyExist = "District with the same language code already exists";

        public const string cityNotFound = "City does not exist";
        public const string cityAlreadyExist = "City with the same name or code already exists";
        public const string cityLanguageAlreadyExist = "City with the same language code already exists";

        public const string cropAlreadyExist = "Crop with the same name already exists";
        public const string cropNotFound = "Crop does not exist";

        public const string serviceAlreadyExist = "Service with the same name or code already exists";
        public const string serviceTranslationAlreadyExist = "Service translation with the same name already exists";
        public const string serviceNotFound = "Service does not exist";
        public const string serviceTranslationNotFound = "Service translation does not exist";

        public const string productNotFound = "Product does not exist";
        public const string productAlreadyExist = "Product with the same name or code already exists";

        public const string tehsilNotFound = "Tehsil does not exist";
        public const string tehsilAlreadyExist = "Tehsil with the same name or code already exists";

        public const string seasonAlreadyExist = "Season with the same name already exists";

        public const string emailRegexExpressionFails = "Please enter a valid email address";
        public const string errorOccuredWhileAddingUser = "Error adding user details. Please try again";
        public const string userAlreadyExist = "User already exists. Please login";
        public const string invalidAppVersion = "Please update your app to the latest version";
        public const string invalidMethod = "App has encountered an unexpected error. Please contact technical support";
        public const string invalidEmail = "Please enter a valid username and password";
        public const string onlyAdmin = "App has encountered an unexpected error. Please contact technical support";
        public const string invalidPassword = "Please enter a valid username and password";
        public const string verifyPasswordInvalid = "Invalid Password";
        public const string deactiveUser = "Your account is inactive. Please contact technical support";
        public const string nullUsernameOrPassword = "Username and password fields can not be empty";
        public const string firstnameRequired = "First name is required";
        public const string phoneRequired = "Mobile number is required";
        public const string phoneAlreadyTaken = "Mobile number already exists";
        public const string emailRequired = "Email address is required";
        public const string emailIsTaken = "Email address already exists. Please try another email address";
        public const string passwordRequired = "Password is required";
        public const string passwordMinLength = "Your password must contain 8 or more characters";
        public const string specialCharacterRequired = "Your password must contain 1 special character";
        public const string passwordOneUpperCaseRequired = "Your password must contain atleast 1 uppercase letter";
        public const string passwordOneLowerCaseRequired = "Your password must contain atleast 1 lowercase letter";
        public const string passwordOneDigitRequired = "Your password must contain atleast a number";
        public const string orderIDRequired = "Order ID is required";
        public const string orderNotFound = "Order does not exist";
        public const string orderNotPaid = "Order has not been paid";
        public const string orderExpectedDeliveryDateNotFound = "Expected delivery date does not exist";
        public const string orderExpired = "Order has been expired";
        public const string alreadyPaid = " Order has been paid already";
        public const string advancePaymentNotDone = "Advance payment has not been made";
        public const string advanceOrderNotFound = "Advance order does not exist";
        public const string orderPaymentPendingLastOrder = "Payment is in processing state";
        public const string productIDRequired = "Product ID is required";
        public const string qtyRequired = "Please add atleast one product in the cart to proceed";
        public const string authorityletterBagQuantityReached = "You can not add more products in the authority letter";
        public const string palindromeFound = "Your password contains a palindrome. Please try using another password";
        public const string consecutivityFound = "Your password can not contain 3 consecutive letters. Please try using another password";
        public const string ristrictedCharactersFound = "Your password contains restricted characters. Please try using another password";
        public const string platformRequired = "App has encountered an unexpected error. Please contact technical support";
        public const string confirmPasswordRequired = "Confirm password is required";
        public const string confirmPasswordNotMatch = "Password fields do not match";
        public const string languageAlreadyExist = "Language already exists";
        public const string languageNotFound = "Language does not exist";
        public const string languageCodeRequired = "Language code is required";
        public const string dateOfBirthRequired = "Date of birth is required";
        public const string cellNumberRequired = "Mobile number is required ";
        public const string ntnNumberRequired = "NTN number is required ";
        public const string ntnNumberIsTaken = "NTN number has been taken already. Please try another NTN number";
        public const string attachmentRequired = "Please attach atleast 1 document to continue";
        public const string attachmentNotValid = "Please enter a valid file type";
        public const string attachmentNotFound = "Attachment doen not exist";
        public const string attachmentDoesnotBelongs = "App has encountered an unexpected error. Please contact technical support";
        public const string attachmentConsumedAlready = "App has encountered an unexpected error. Please contact technical support";
        public const string authorityLetterAttachmentRequired = "Please attach atleast 1 document to continue";
        public const string authorityLetterQtyReached = "Authority letter has reached its maximum quantity";
        public const string authorityLetterNotFound = "Authority letter does not exist";
        public const string fatherNameRequired = "Father name is required";
        public const string cnicNumberRequired = "CNIC is required";
        public const string cnicNumberNotValid = "Please enter a valid CNIC number";
        public const string strnNumberRequired = "STRN number is required";
        public const string invalidOrderProduct = "Please enter a valid order product";
        public const string addressRequired = "Address is required";
        public const string requiredFieldsMissing = "Please enter values in all mandatory fields";
        public const string farmerCommentRequired = "Farmer comments are required";
        public const string isPlanEditable = "Plan change is allowed";
        public const string planNotInValidState = "Plan change is not allowed";
        public const string cnicNumberDoesNotMatach = "CNIC number does not match";
        public const string authorityLetterUpdatedAlready = "Authority letter has been updated already";
        public const string textNotFoundInImage = "Please enter a valid image";
        public const string imageDoesNotSeemsLikeCNIC = "Please attach a valid image";
        public const string invalidAttachmentType = "Please attach a valid file type";
        public const string internalServerError = "App has encountered an unexpected error. Please contact technical support";
        public const string truckerRequired = "Trucker number is required";
        public const string biltyNumberRequired = "Bilty number is required";
        public const string reasonRequired = "Reason is required";
        public const string reasonIDNotFound = "Please select a reason to proceed";
        public const string reasonNotFound = "Reason does not exist";
        public const string cityRequired = "City is required";
        public const string tehsilRequired = "Tehsil is required";
        public const string districtRequired = "District is required";
        public const string usernameRequired = "Username is required";
        public const string leasedLandExteeds = "Lease land must be less than or equal to owned land";
        public const string leasedLandMinMaxLengthExteeds = "Leased land cannot be greater than 100,000";
        public const string ownedLandMinMaxLengthExteeds = "Owned land cannot be greater than 100,000";
        public const string leasedAndOwnedLandCannotBeZero = "Both leased and owned lands cannot be 0";
        public const string isTermsAcceptRequired = "Please accept the terms & conditions to proceed";
        public const string usernameIsTaken = "Username already exists. Please try another username";
        public const string userIDNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string userIDorLanguageCodeNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string cnicIsTaken = "CNIC number already exists. Please try another CNIC number";
        public const string cnicNotValid = "Please enter a valid CNIC number";
        public const string ntnIsTaken = "NTN number already exists. Please try another NTN number";
        public const string strnIsTaken = "STRN already exists. Please try STRN number";
        public const string cellNumberIsTaken = "Mobile number already exists. Please try another number";
        public const string lessThen18 = "App has encountered an unexpected error. Please contact technical support";
        public const string invalidOTP = "OTP is invalid";
        public const string expiredOTP = "OTP is invalid or expired. Please press Resend OTP button to get the new OTP";
        public const string otpRequired = "OTP is required";
        public const string canUseDataRequired = "Please accept the terms & conditions to proceed";
        public const string isAllInformationCorrect = "Please accept the terms & conditions to proceed";
        public const string atleastOneFarm = "Please select atleast one farm to procced";
        public const string OnePrimaryFarmRequired = "Please select a primary farm to proceed";
        public const string invalidUsername = "Please enter a valid username";
        public const string sapFarmerCodeNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string sapOrderIDNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string invalidUCNIC = "Please enter a valid CNIC number";
        public const string userNotFound = "User does not exist";
        public const string userProfileNotFound = "User profile does not exist";
        public const string nullApplicationID = "App has encountered an unexpected error. Please contact technical support";
        public const string cropsRequired = "Please select atleast one crop to proceed";
        public const string productsRequired = "Please add atleast one product in the cart";
        public const string seasonRequired = "Please select a season";
        public const string seasonNotFound = "Season doen not exist";
        public const string seasonRangeOverlaping = "Season starting & ending months are overlaping";
        public const string usernameTaken = "Username already exists. Please try another username";
        public const string statusIDRequired = "App has encountered an unexpected error. Please contact technical support";
        public const string farmNotFound = "Farm does not exist";
        public const string warehouseIDNotFound = "Warehouse ID does not exist";
        public const string warehouseNotFound = "Warehouse does not exist";
        public const string farmIDRequired = "Farm ID is required";
        public const string farmNameRequired = "Farm name is required";
        public const string approvalIDRequired = "App has encountered an unexpected error. Please contact technical support";
        public const string farmNotAuthorized = "App has encountered an unexpected error. Please contact technical support";
        public const string farmInValidState = "App has encountered an unexpected error. Please contact technical support";
        public const string userNotAuthorized = "User does not have sufficient rights";
        public const string profileApproved = "App has encountered an unexpected error. Please contact technical support";
        public const string APINotAuthorized = "App has encountered an unexpected error. Please contact technical support";
        public const string applicationIDRequired = "App has encountered an unexpected error. Please contact technical support";
        public const string planIDRequired = "App has encountered an unexpected error. Please contact technical support";
        public const string planNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string planEPlanApprovalRejectionTypeNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string authorityLetterIDNotFound = "App has encountered an unexpected error. Please contact technical support";
        public const string authorityLetterNotInValidState = "App has encountered an unexpected error. Please contact technical support";
        public const string invoiceNotFoundInSAP = "Invoice does not exist in SAP";
        public const string seemsLikeFarmApproved = "Congratulations, your farm has been approved!";
        public const string designationIDRequired = "App has encountered an unexpected error. Please contact technical support";
        public const string userAddedRoleNotAssigned = "App has encountered an unexpected error. Please contact technical support";
        public const string notFound = "This resource does not exist";

        //SMS Request
        public const string smsResponseNot200 = "Unable to send SMS";
        public const string smsHasNotSent = "Unable to send SMS";

        public const string createCustomerFailure = "App has encountered an unexpected error. Please contact technical support";
        public const string pricingNotMaintained = "Unable to fetch pricing data. Please contact technical support";
        public const string rejectionNotAllowed = "App has encountered an unexpected error. Please contact technical support";
        public const string orderApprovalFailure = "App has encountered an unexpected error. Please contact technical support";

        //SAP Validation
        //Create Customer
        public const string nameLengthExteed_SAPValidation = "Name should be less than 40 characters";
        public const string emailAddressLengthExteed_SAPValidation = "Email address should be less than 60 characters";
        public const string address1LengthExteed_SAPValidation = "Address 1 should be less than 60 characters";
        public const string address2LengthExteed_SAPValidation = "Address 2 should be less than 60 characters";

        public const string noImpactOnApprovalError = "App has encountered an unexpected error. Please contact technical support";
        public const string noImpactOnRejectionError = "App has encountered an unexpected error. Please contact technical support";

        //Agrilift.io
        //Login
        public const string agriliftRetrieveFailure = "There is no data maintained for your user yet. Please check back later or contact technical support";
        public const string agriliftDeserializationError = "There is no data maintained for your user yet. Please check back later or contact technical support";
        public const string agrilift116 = "There is no data maintained for your user yet. Please check back later or contact technical support";

        //Farmdar.ai
        //Login
        public const string farmdarLoginFailure = "There is no data maintained for your user yet. Please check back later or contact technical support";


        public const string productCategoryNotFound = "App has encountered an unexpected error. Please contact technical support";

        //File exception not value
        public const string fileExtensionNotValid = "Please attach a valid file type";

        //Order
        public const string orderAlreadyBlocked = "Order is already blocked or removed";
    }
}
