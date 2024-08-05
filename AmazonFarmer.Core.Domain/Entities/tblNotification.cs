using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblNotification
    {
        [Key]
        public int ID { get; set; }
        public required string UserID { get; set; }
        public int DeviceNotificationID { get; set; }
        public int? PlanID { get; set; }
        public int? FarmID { get; set; }
        public Int64? OrderID { get; set; }
        //
        public int? AuthorityLetterID { get; set; }
        public string? AuthorityLetterNo { get; set; } = string.Empty;
        public int? FarmApplicationID { get; set; }
        public string? FarmName { get; set; } = string.Empty;
        public string? NewPaymentDueDate { get; set; } = string.Empty;
        public string? ConsumerNumber { get; set; } = string.Empty;
        public string? PKRAmount { get; set; } = string.Empty;
        public string? GoogleMapLinkWithCoordinated { get; set; } = string.Empty;
        public int? WarehouseID { get; set; }
        public string? PickUPDate { get; set; }
        public int? ReasonsDropdownID { get; set; }
        public string? ReasonCommentBox { get; set; } = string.Empty;

        public EDeviceNotificationRequestStatus? NotificationRequestStatus { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public bool IsClicked { get; set; } = false;
        public DateTime ClickedOn { get; set; } = DateTime.UtcNow;
        [ForeignKey("DeviceNotificationID")]
        public virtual tblEmailNotifications Notification { get; set; } = null!;
        //public virtual tblDeviceNotifications DeviceNotification { get; set; } = null!;
        [ForeignKey("UserID")]
        public virtual TblUser User { get; set; } = null!;
        [ForeignKey("ReasonsDropdownID")]
        public virtual tblReasons Reason { get; set; }
        [ForeignKey("WarehouseID")]
        public virtual tblwarehouse Warehouse { get; set; }
    }
}
