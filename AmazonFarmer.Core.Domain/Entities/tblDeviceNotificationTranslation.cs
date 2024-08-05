using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblDeviceNotificationTranslation
    {
        [Key]
        public int ID { get; set; }
        public int DeviceNotificationID { get; set; }
        public required string LanguageCode { get; set; }
        public required string Text { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; } = null!;
        [ForeignKey("DeviceNotificationID")]
        public virtual tblDeviceNotifications DeviceNotification { get; set; } = null!;
    }
}
