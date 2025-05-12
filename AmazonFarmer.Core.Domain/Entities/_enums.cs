using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AmazonFarmer.Core.Domain.Entities
{
    public enum EComplaintStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Progress")]
        Progress = 2,
        [Description("Resolved")]
        Resolved = 3
    }
    public enum EDocumentName
    {
        [Description("Farmer Profile")]
        FarmerProfile = 1,
        [Description("Order Report")]
        OrderReport = 2
    }
    public enum EServiceType
    {
        AgriliftReport = 1,
        FarmDarReport = 2,
        AgriliftReportList = 3,
        FarmDarReportList = 4,
    }
    public enum EServices
    {
        [Description("Soil Sampling")]
        Soil_Sampling = 1,
        [Description("Geofencing")]
        Geofencing = 2,
        [Description("Drone Footage")]
        Drone_Footage = 3,
        [Description("Nitrogen Uptake")]
        Nitrogen_Uptake = 4,
        [Description("Productivity Zone")]
        Productivity_Zone = 5
    }
    public enum ESeasons
    {
        Rabi = 1,
        Kharif = 2
    }
    public enum EPlanApprovalRejectionType
    {
        [Description("No Impact")]
        NoImpactAdvance = 1,

        [Description("Forfeit Advance")]
        ForfeitAdvance = 2,

        [Description("Refund Advance")]
        RefundAdvance = 3
    }
    public enum EPlanUpdateStatus
    {
        [Description("Approve")]
        Approve = 1,

        [Description("Endorse")]
        Endorse = 2,

        [Description("Send Back")]
        SendBack = 3,

        [Description("Reject")]
        Reject = 4
    }
    public enum ERoles
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Employee")]
        Employee = 2,
        [Description("Farmer")]
        Farmer = 3,
        [Description("One Link")]
        OneLink = 4
    }
    public enum EReasonFor
    {
        Pending = 0,
        Approve = 1,
        Reject = 2,
        Revert = 3,
        Active = 4,
    }
    public enum EReasonOf
    {
        Farm = 1,
        Plan = 2,
        Order = 3,
        ChangedPlan_Employee = 4
    }
    public enum EPlanSummaryType
    {
        crop,
        product
    }
    public enum EFarmerProfileStatus
    {
        Pending = 1,
        Approved = 2,
        Blocked = 3,
        Editted = 4,
        Editable = 5
    }
    public enum EDesignation
    {
        [Description("Territory Sales Officer")]
        Territory_Sales_Officer = 1,
        [Description("Regional Sales Manager")]
        Regional_Sales_Manager = 2,
        [Description("National Sales Manager")]
        National_Sales_Manager = 3,
        [Description("Warehouse Incharge")]
        Warehouse_Incharge = 4,
        [Description("Admin")]
        Admin = 5
    }
    public enum ENotificationType
    {
        Email,
        SMS,
        FCM,
        Device
    }
    public enum EDeviceNotificationType
    {
        /// <summary>
        /// Your plan:[planID] is on [status] stage.
        /// Your [PLan] has been [status]
        /// </summary>
        Farmer_PlanStatusUpdated = 1,
        /// <summary>
        /// Your plan change request against plan:[planID] has been [status]
        /// </summary>
        Farmer_PlanChangeRequestUpdated = 2,
        /// <summary>
        /// Payment has been [status] against plan:[planID].
        /// </summary>
        Farmer_PlanOrder = 3,
        /// <summary>
        /// Your farm:[farmID] is on [status] stage.
        /// </summary>
        Farmer_FarmStatusUpdated = 4
    }
    public enum EDeviceNotificationRequestStatus
    {
        [Description("TSO Approval")]
        Plan_TSOProcessing = 1,
        [Description("RSM Approval")]
        Plan_RSMProcessing = 2,
        [Description("NSM Approval")]
        Plan_NSMProcessing = 3,
        [Description("Approved")]
        Plan_Approved = 4,
        [Description("Declined")]
        Plan_Declined = 5,
        [Description("Completed")]
        Plan_Completed = 6,
        [Description("Reverted")]
        Plan_Revert = 7,
        [Description("Removed")]
        Plan_Removed = 8,
        [Description("saved in draft")]
        Plan_Draft = 9,
        [Description("Rejected")]
        Plan_Rejected = 10,
        [Description("Active")]
        Order_Active = 11,
        [Description("Blocked")]
        Order_Blocked = 12,
        [Description("Deleted")]
        Order_Deleted = 13,
        [Description("Expired")]
        Order_Expired = 14,
        [Description("Locked")]
        Order_Locked = 15,
        [Description("Draft")]
        Farm_Draft = 16,
        [Description("Pending at TSO")]
        Farm_PendingForTSO = 17,
        [Description("Pending at RSM")]
        Farm_PendingforRSM = 18,
        [Description("Pending at Patwari")]
        Farm_PendingForPatwari = 19,
        [Description("Approved")]
        Farm_Approved = 20,
        [Description("Rejected")]
        Farm_Rejected = 21,
        [Description("Sent back")]
        Farm_SendBack = 22,
        [Description("Deleted")]
        Farm_Deleted = 23,
        [Description("Default")]
        PlanChange_Default = 24,
        [Description("Pending")]
        PlanChange_Pending = 25,
        [Description("Accept")]
        PlanChange_Accept = 26,
        [Description("Declined")]
        PlanChange_Declined = 27
    }

    public enum EActivityStatus
    {
        DeActive = 0,
        Active = 1,
        Deleted = 2
    }
    public enum EServiceVendorStatus
    {
        NotScheduled = 0,
        NotCompleted = 1,
        Scheduled = 2,
        Completed = 3
    }
    public enum EServiceRequestStatus
    {
        Pending = 0,
        Accept = 1,
        Decline = 2
    }
    public enum EAuthorityLetterStatus
    {
        DeActive = 0,
        Active = 1,
        Approved = 2
    }
    public enum EOrderPaymentStatus
    {
        [Description("Not paid")]
        NonPaid = 1,
        [Description("Paid")]
        Paid = 2,
        [Description("Refund")]
        Refund = 3,
        [Description("Payment Processing")]
        PaymentProcessing = 4,
        [Description("Acknowledged")]
        Acknowledged = 5,
        [Description("Ledger Update")]
        LedgerUpdate = 6,
        [Description("Forfeit")]
        Forfeit = 7
    }
    public enum EOrderStatus
    {
        [Description("Active")]
        Active = 1,
        [Description("Blocked")]
        Blocked = 2,
        [Description("Deleted")]
        Deleted = 3,
        [Description("Expired")]
        Expired = 4,
        [Description("Locked")]
        Locked = 5
    }
    public enum EDeliveryStatus
    {
        [Description("None")]
        None = 1,
        [Description("Partially Delivered")]
        PartiallyDelivered = 2,
        [Description("Shipment Complete")]
        ShipmentComplete = 3
    }
    public enum EBannerType
    {
        homeScreen = 1,
        loginScreen = 2
    }
    public enum EWeatherType
    {
        [Description("Sunny")]
        Sunny = 1,
        [Description("Mostly Sunny")]
        Mostly_Sunny = 2,
        [Description("Partly Sunny")]
        Partly_Sunny = 3,
        [Description("Intermittent Clouds")]
        Intermittent_Cloud = 4,
        [Description("Hazy Sunshine")]
        Hazy_Sunshine = 5,
        [Description("Mostly Cloudy")]
        Mostly_Cloudy = 6,
        [Description("Cloudy")]
        Cloudy = 7,
        [Description("Dreary (Overcast)")]
        Dreary_Overcast = 8,
        [Description("Fog")]
        Fog = 11,
        [Description("Showers")]
        Showers = 12,
        [Description("Mostly Cloudy w/ Showers")]
        Mostly_Cloudy_Showers = 13,
        [Description("Partly Sunny w/ Showers")]
        Partly_Cloudy_Showers = 14,
        [Description("T-Storms")]
        T_Storms = 15,
        [Description("Mostly Cloudy w/ T-Storms")]
        Mostly_Cloudy_Storm = 16,
        [Description("Partly Sunny w/ T-Storms")]
        Partly_Sunny_Storms = 17,
        [Description("Rain")]
        Rain = 18,
        [Description("Flurries")]
        Flurries = 19,
        [Description("Mostly Cloudy w/ Flurries")]
        Mostly_Cloudy_Flurries = 20,
        [Description("Partly Sunny w/ Flurries")]
        Partly_Sunny_Flurries = 21,
        [Description("Snow")]
        Snow = 22,
        [Description("Mostly Cloudy w/ Snow")]
        Mostly_Cloudy_Snow = 23,
        [Description("Ice")]
        Ice = 24,
        [Description("Sleet")]
        Sleet = 25,
        [Description("Freezing Rain")]
        Freezing_Rain = 26,
        [Description("Rain and Snow")]
        Rain_and_Snow = 29,
        [Description("Hot")]
        Hot = 30,
        [Description("Cold")]
        Cold = 31,
        [Description("Windy")]
        Windy = 32,
        [Description("Clear")]
        Clear = 33,
        [Description("Mostly Clear")]
        Mostly_Clear = 34,
        [Description("Partly Cloudy")]
        Partly_Cloudy = 35,
        [Description("Intermittent Clouds")]
        Intermittent_Clouds = 36,
        [Description("Hazy Moonlight")]
        Hazy_Moonlight = 37,
        [Description("Mostly Cloudy (Night)")]
        Night_Mostly_Cloudy = 38,
        [Description("Partly Cloudy w/ Showers")]
        Partly_Cloudy_w_Showers = 39,
        [Description("Mostly Cloudy w/ Showers")]
        Mostly_Cloudy_w_Showers = 40,
        [Description("Partly Cloudy w/ T-Storms")]
        Partly_Cloudy_Storms = 41,
        [Description("Mostly Cloudy w/ T-Storms")]
        Mostly_Cloudy_Storms = 42,
        [Description("Mostly Cloudy w/ Flurries")]
        Mostly_Cloudy_w_Flurries = 43,
        [Description("Mostly Cloudy w/ Snow")]
        Mostly_Cloudy_w_Snow = 44
    }
    public enum EOrderType
    {
        [Description("Advance")]
        Advance = 1,
        [Description("Product")]
        Product = 2,
        [Description("Order Reconcile")]
        OrderReconcile = 3,
        [Description("Advance Payment Reconcile")]
        AdvancePaymentReconcile = 4
    }
    public enum EFarmStatus
    {
        [Description("Draft")]
        Draft = 0,
        [Description("Pending at TSO")]
        PendingForTSO = 1,
        [Description("Pending at RSM")]
        PendingforRSM = 2,
        [Description("Pending at Patwari")]
        PendingForPatwari = 3,
        [Description("Approved")]
        Approved = 4,
        [Description("Rejected")]
        Rejected = 5,
        [Description("Sent back")]
        SendBack = 6,
        [Description("Deleted")]
        Deleted = 7
    }
    public enum EPlanCropEndorse
    {
        Ok = 1,
        Exception = 2
    }
    public enum EModeOfPayment
    {
        [Description("Partial Payment (5% Advance, 95% Order)")]
        Partial_Payment = 0,
        [Description("Full Payment (100% Order)")]
        Full_Payment = 1
    }

    public enum EChangeWarehouseStatus
    {
        [Description("Request not initiated")]
        Default = 0,
        [Description("RSM Approval")]
        RSMProcessing = 1,
        [Description("NSM Approval")]
        NSMProcessing = 2,
        [Description("Approved")]
        Approved = 3,
        [Description("Declined")]
        Declined = 4
    }

    public enum EPlanStatus
    {
        [Description("TSO Approval")]
        TSOProcessing = 1,
        [Description("RSM Approval")]
        RSMProcessing = 2,
        [Description("NSM Approval")]
        NSMProcessing = 3,
        [Description("Approved")]
        Approved = 4,
        [Description("Declined")]
        Declined = 5,
        [Description("Completed")]
        Completed = 6,
        [Description("Reverted")]
        Revert = 7,
        [Description("Removed")]
        Removed = 8,
        [Description("saved in draft")]
        Draft = 9,
        [Description("Rejected")]
        Rejected = 10,
    }
    public enum EPlanChangeRequest
    {
        [Description("Default")]
        Default = 0,
        [Description("Pending")]
        Pending = 1,
        [Description("Accept")]
        Accept = 2,
        [Description("Declined")]
        Declined = 3
    }

    public enum Email_Type
    {
        OTPEmail = 1,
        ForgetPasswordEmail = 2
    }
    public enum EOPTVerificationType
    {
        forgetPassword = 1,
        accountVerification = 2
    }

    public enum EAttachmentType
    {
        [Description("User CNIC Document")]
        User_CNIC_Document = 1,
        [Description("User NTN Document")]
        User_NTN_Document = 2,
        [Description("Farm Document")]
        Farm_Document = 3,
        [Description("General Document")]
        General_Document = 4,
        [Description("Verify AuthorityLetter NIC")]
        Verify_AuthorityLetter_NIC = 5,
        [Description("Home Banners")]
        HomeBanner = 6,
        [Description("Intro Banners")]
        IntroBanner = 7,
        [Description("Login Banners")]
        LoginBanner = 8,
        [Description("Products")]
        Product = 9,
        [Description("Crops")]
        Crop = 10,
        [Description("Services")]
        Service = 11,
        [Description("AuthorityLetter PDF")]
        PDF_AuthorityLetter = 12,
        [Description("Weather Icons")]
        Weather_Icons = 13
    }
    public enum EApplicationType
    {
        Farm_Application = 1,
    }
    public enum ETransactionStatus
    {
        Pending = 1,
        Completed = 2,
        Acknowledged = 3, //File is recieved
        SapLedgerUpdated = 4,//Using Do Payment
        Fulfilled = 5 // ALl COmplete
    }
    public enum ENotificationBody
    {
        signup = 1,
        signin = 2,
        farmAdded = 3,
        /// <summary>
        /// You have received an application [APPID] having [FarmName] has been Approved.
        /// </summary>
        farmApproved = 4,
        farmChangeRequest = 5,
        planAdded = 6,
        /// <summary>
        /// You plan [PlanID] has been approved.
        /// </summary>
        planApproved = 7,
        planDraft = 8,
        waitingOrderPayment = 9,
        orderPaymentDone = 10,
        farmRequestIsInPending = 11,
        /// <summary>
        /// Your One Time Password is [OTP]. 
        /// </summary>
        OTP = 12,
        /// <summary>
        /// You have received an authority letter [Authority Letter ID].
        /// </summary>
        AuthorityLetter_Warehouse = 13,
        PlanChangeRequest = 14,
        AuthorityLetter_Approved_WarehouseIncharge = 15,
        /// <summary>
        /// Your Order [OrderID] has been shipped with quantity [Authority Letter QTY]. 
        /// </summary>
        AuthorityLetter_Approved_Farmer = 16,
        FarmerApproved = 17,
        /// <summary>
        /// You plan [PlanID] has been sent to [Send to name].
        /// </summary>
        PlanEndorsed_Farmer = 18,
        /// <summary>
        /// You have received a plan [PlanID] for approval.<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        PlanEndorsed_Employee = 19,
        /// <summary>
        /// Check below for MIS File Automation report already done
        /// </summary>
        FinancePaymentReport = 20,
        FarmerNotificationForPaymentSuccess = 21,
        //new set
        /// <summary>
        /// You have received an application [APPID] having [FarmName] has been SendBack<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        farmSendBackByEmployee = 22,
        /// <summary>
        /// You have received an application [APPID] having [FarmName] has been rejected.<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        farmRejectionByEmployee = 23,
        /// <summary>
        /// You have received an application [APPID] having [FarmName] for approval.
        /// </summary>
        Employee_farmSubmitForApproval = 24,
        /// <summary>
        /// You have received an application [APPID] having [FarmName] for approval.
        /// </summary>
        Employee_FarmApplicationPendingForApproval = 25,
        /// <summary>
        /// Your application [APPID] having [FarmName] has been approved and send to [SendToManager] for approval. 
        /// </summary>
        farmApplicationSubmittedForApproval = 26,
        /// <summary>
        /// You have received a plan [PlanID] for approval.
        /// </summary>
        Employee_planPendingForApproval = 27,
        /// <summary>
        /// You plan [PlanID] has been rejected.<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        planRejectionByEmployee = 28,
        /// <summary>
        /// You plan [PlanID] has been SendBack for changes<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        planSendBackByEmployee = 29,
        /// <summary>
        /// [FarmerName] has requested to allow to change his plan [PlanID].<para></para>
        /// Comment:
        /// [Comments]<para></para>
        /// </summary>
        Employee_PlanChangeRequest = 30,
        /// <summary>
        /// Your request for Plan [PlanID] changes has been approved. Please do your required changes on plan and send back for approval<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        planChangeRequestApproved = 31,
        /// <summary>
        /// Your request for Plan [PlanID] changes has been rejected. You have to continue with same plan. Or for further clarity please contact your TSO Manager<para></para>
        /// Reasons<para></para>
        /// [Reasons Dropdown Option]<para></para>
        /// [Reason Comment Box]<para></para>
        /// </summary>
        planChangeRequestRejected = 32,
        /// <summary>
        /// You Payment on Consumer Number [ConsumerNumber] has been confirmed. Your order has been created on our system.<para></para>
        ///Your Order ID is [Order ID].<para></para>
        ///Engro Warehouse Name: [Warehouse Name]<para></para>
        ///Pick up location: [Google Map link with coordinated]<para></para>
        ///Pick up Date: [PickUP Date]<para></para>
        /// </summary>
        OrderPaymentProcessCompleted = 33,
        /// <summary>
        /// You Payment on Consumer Number [ConsumerNumber] has been confirmed. Your amount [PKR Amount] will be consumed once you pickup your Order. 
        /// </summary>
        OrderReconcilePaymentProcessCompleted = 34,
        /// <summary>
        /// You Payment on Consumer Number [ConsumerNumber] has been confirmed. Your advance amount [PKR Amount] is as security deposit. It will be consumed on your last order.
        /// </summary>
        AdvancePaymentProcessCompleted = 35,
        /// <summary>
        /// You Payment on Consumer Number [ConsumerNumber] has been received. Your payment is in proccessing stage. It will take 8 business hours. For further detail contact your TSO Manager.
        /// </summary>
        PaymentProcessing = 36,
        /// <summary>
        /// You Order [OrderID] of plan [PlanID] has been unBlocked. Your new payment date is [New Payment Due Date]
        /// </summary>
        OrderUnblocked = 37,
        /// <summary>
        /// You Order [OrderID] of plan [PlanID] has been Blocked permanently due to payment not received on due time. Please contact your TSO Manager for further clarity.
        /// </summary>
        OrderBlocked = 38,
        /// <summary>
        /// You plan [PlanID] has been rejected and advance is refunded Please read below notes carefuly
        /// Notes <para></para>
        /// 1. Case 1: Your all orders are deleted<para></para>
        /// 2. Case 2: If any order is partially shipped and any changes are done on that particular order. Then that order is marked as fully Shipped with delivered quantity. And marked as refunded.<para></para>
        /// 3. Case 3: If any order is UNPAID and any changes are done it will be deleted<para></para>
        /// 4. Case 4: If any order is fully shipped it will be remained unchanged.<para></para>
        /// </summary>
        planChangeRequestRejectedandRefunded = 39,
        /// <summary>
        /// You plan [PlanID] has been rejected and advance is forfieted. Please read below notes carefuly<para></para>
        /// Notes<para></para>
        /// 1. Case 1: Your all orders are deleted<para></para>
        /// 2. Case 2: If any order is partially shipped and any changes are done on that particular order. Then that order is marked as fully Shipped with delivered quantity. And marked as refunded.<para></para>
        /// 3. Case 3: If any order is UNPAID and any changes are done it will be deleted<para></para>
        /// 4. Case 4: If any order is fully shipped it will be remained unchanged.<para></para>
        /// </summary>
        planChangeRequestRejectedandForfieted = 40,
        /// <summary>
        /// You plan [PlanID] has been approved and advance is forfieted. Please read below notes carefuly<para></para>
        /// Notes<para></para>
        /// 1. Case 1: If total amount is increased. A new advance diffential order will be created.Pay to proceed for further orders<para></para>
        /// 2. Case 2: If any order is partially shipped and any changes are done on that particular order.Then that order is marked as fully Shipped with delivered quantity.And marked as refunded.And a new order with differential quanity is created.<para></para>
        /// 3. Case 3: If any order is UNPAID and any changes are done it will be deleted<para></para>
        /// 4. Case 4: If any order is fully shipped it will be remained unchanged.<para></para>
        /// </summary>
        planChangeRequestApprovedandForfieted = 41,
        /// <summary>
        /// You plan [PlanID] has been approved. Please ready notes carefully below if you have done your advance payment. If Advance payment is not done then below points will not be reflected.<para></para>
        /// Notes<para></para>
        /// 1. Case 1: If total amount is increased. A new advance diffential order will be created.Pay to proceed for further orders<para></para>
        /// 2. Case 2: If any order is partially shipped and any changes are done on that particular order.Then that order is marked as fully Shipped with delivered quantity.And marked as refunded.And a new order with differential quanity is created.<para></para>
        /// 3. Case 3: If any order is UNPAID and any changes are done it will be deleted<para></para>
        /// 4. Case 4: If any order is fully shipped it will be remained unchanged.<para></para>
        /// </summary>
        planChangeRequestApprovedandNoImpact = 42,
        /// <summary>
        /// You plan [PlanID] has been approved with complete advance and order refund. Please read below notes carefuly<para></para>
        /// Notes<para></para>
        /// 1. Case 1: If total amount is increased. A new advance diffential order will be created.Pay to proceed for further orders<para></para>
        /// 2. Case 2: If any order is partially shipped and any changes are done on that particular order.Then that order is marked as fully Shipped with delivered quantity.And marked as refunded.And a new order with differential quanity is created.<para></para>
        /// 3. Case 3: If any order is UNPAID and any changes are done it will be deleted<para></para>
        /// 4. Case 4: If any order is fully shipped it will be remained unchanged.<para></para>
        /// </summary>
        planChangeRequestApprovedandRefundPayment = 43,
        /// <summary>
        /// Your account has been generated on [project]. Here's your temporary password: [password].<br/> Kindly change it at your earliest convenience.
        /// </summary>
        employee_NewUserAddedNotification = 44


    }

    public enum EPaymentAcknowledgmentStatus
    {
        Imported = 1,
        Processed = 2,
        Failed = 3,
        FailedReprocessable = 4
    }

    public enum EPaymentAcknowledgmentFileStatus
    {
        Received = 1,
        Imported = 2,
        Processed = 3,
        Failed = 4,
        PartiallyImported = 5
    }
    public static class EPaymentText
    {
        public static string Initial5Percent = "Z040";
        public static string OrderPayment = "Z041";
        public static string OtherIncome = "Z042";
        public static string ReversePayment = "Z043";
        public static string AZAdvanceLock = "Z044";
    }


}
