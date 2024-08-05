using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class NotificationLog
    {
        [Key]
        public int Id { get; set; }
        public ENotificationType Type { get; set; }
        public string Recipient { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
