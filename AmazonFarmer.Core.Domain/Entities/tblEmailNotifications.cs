using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblEmailNotifications
    {
        [Key]
        public int ID { get; set; }
        public ENotificationBody Type { get; set; }
        public EActivityStatus Status { get; set; }
        public virtual List<tblEmailNotificationTranslations> EmailNotificationTranslations { get; set; } = null!;
        public virtual List<tblNotification> Notifications { get; set; } = null!;
    }
    public class tblEmailNotificationTranslations
    {
        [Key]
        public int RecID { get; set; }
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Email Body
        /// </summary>
        public string Body { get; set; } = string.Empty;
        public string? SMSBody { get; set; }
        public string? FCMBody { get; set; }
        public string? DeviceBody { get; set; }
        public required string LanguageCode { get; set; }
        public int NotificationID { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Languages { get; set; } = null!;
        [ForeignKey("NotificationID")]
        public virtual tblEmailNotifications Notification { get; set; } = null!;
    }
}
