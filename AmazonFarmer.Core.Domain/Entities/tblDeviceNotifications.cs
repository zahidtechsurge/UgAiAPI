using System;
using System.ComponentModel.DataAnnotations;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblDeviceNotifications
    {
        [Key]
        public int NotificationID { get; set; }
        public EDeviceNotificationType Type { get; set; }
        public string Body { get; set; } = string.Empty;
        public EActivityStatus Status { get; set; }
        public virtual List<tblDeviceNotificationTranslation> DeviceNotificationTranslations { get; set; } = null!;
    }
}
