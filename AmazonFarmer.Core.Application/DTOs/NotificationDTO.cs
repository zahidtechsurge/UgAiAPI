// Importing necessary namespaces

using AmazonFarmer.Core.Domain.Entities;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class NotificationDTO // Data Transfer Object for Notification
    {
        public string title { get; set; } = string.Empty; // Property for title
        public string body { get; set; } = string.Empty; // Property for body
        public string fcmBody { get; set; } = string.Empty; // Property for body
        public string smsBody { get; set; } = string.Empty; // Property for body
        public string deviceBody { get; set; } = string.Empty; // Property for body
    }
    public class NotificationReplacementDTO // Data Transfer Object for Notification
    {
        public string PlanID { get; set; } = string.Empty; // Property for title
        public string OrderID { get; set; } = string.Empty; // Property for body
        public string UserName { get; set; } = string.Empty; // Can be used to Send to Name
        public string ApplicationId { get; set; } = string.Empty; // Property for body
        public string OTP { get; set; } = string.Empty; // Property for body
        public string AuthorityLetterId { get; set; } = string.Empty; // Property for body
        public string AuthorityLetterNo { get; set; } = string.Empty; // Property for body
        public string FarmID { get; set; } = string.Empty; // Property for body
        public string FarmName { get; set; } = string.Empty; // Property for body
        public string AuthorityLetterQuantity { get; set; } = string.Empty; // Property for body
        public string ReasonDropDownOptionId { get; set; } = string.Empty; // Property for body
        public string ReasonDropDownOption { get; set; } = string.Empty; // Property for body
        public string ReasonComment { get; set; } = string.Empty; // Property for body
        public string NewPaymentDueDate { get; set; } = string.Empty; // Property for body
        public string PKRAmount { get; set; } = string.Empty; // Property for body
        public string WarehouseId { get; set; } = string.Empty; // Property for body
        public string WarehouseName { get; set; } = string.Empty; // Property for body
        public string GoogleMapLinkWithCoordinated { get; set; } = string.Empty; // Property for body
        public string PickupDate { get; set; } = string.Empty; // Property for body
        public string ConsumerNumber { get; set; } = string.Empty; // Property for body
        public string Status { get; set; } = string.Empty; // Property for body
        public string Project { get; set; } = string.Empty; // Property for body
        public string Password { get; set; } = string.Empty; // Property for body
        public string farmerName { get; set; } = string.Empty; // Property for body
        public ENotificationBody? NotificationBodyTypeID { get; set; }
    }
    /// <summary>
    /// Data Transfer Object for Device Notifications
    /// </summary>
    public class DeviceNotificationDTO
    {
        /// <summary>
        /// Property for Notification ID
        /// </summary>
        public int notificationID { get; set; }
        /// <summary>
        /// Property for context type routing
        /// </summary>
        public int typeID { get; set; }
        /// <summary>
        /// Property for body
        /// </summary>
        public string body { get; set; } = string.Empty;
        /// <summary>
        /// Property for Date time
        /// </summary>
        public DateTime createdOn { get; set; }
        /// <summary>
        /// Property to check if the Notification is read
        /// </summary>
        public bool isRead { get; set; } = false;
        public Int64 orderID { get; set; }
        public int planID { get; set; }
        public int farmID { get; set; }
        public int authorityLetterID { get; set; }
        public string authorityLetterNo { get; set; } = string.Empty;
        public int farmApplicationID { get; set; }
        public string farmName { get; set; } = string.Empty;
        public string newPaymentDueDate { get; set; } = string.Empty;
        public string consumerNumber { get; set; } = string.Empty;
        public string pKRAmount { get; set; } = string.Empty;
        public string googleMapLinkWithCoordinated { get; set; } = string.Empty;
        public int warehouseID { get; set; }
        public string pickUPDate { get; set; } = string.Empty;
        public int reasonsDropdownID { get; set; }
        public string reasonCommentBox { get; set; } = string.Empty;
    }
    /// <summary>
    /// Data Transfer Object for DeviceNotification
    /// </summary>
    public class DeviceNotification_ReqDTO
    {
        /// <summary>
        /// Property for paggination skip
        /// </summary>
        public int skip { get; set; }

        /// <summary>
        /// Property for paggination take
        /// </summary>
        public int take { get; set; }
    }
    /// <summary>
    /// Data Transfer Object for NotificationID
    /// </summary>
    public class NotificationClickedRequest
    {
        /// <summary>
        /// Property for NotificationID
        /// </summary>
        public int notificationID { get; set; }
    }
    public class GetNotificationRequestStatus
    {
        public string languageCode { get; set; } = string.Empty;
        public EDeviceNotificationRequestStatus requestStatus { get; set; }
    }
}
