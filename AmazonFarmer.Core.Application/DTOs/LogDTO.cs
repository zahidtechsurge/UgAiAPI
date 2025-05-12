using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class LogDTO
    {
        public int recID { get; set; }
        public string method { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string request { get; set; } = string.Empty;
        public DateTime? requestDatetime { get; set; }
        public int statusCode { get; set; }
        public string status { get; set; } = string.Empty;
        public string response { get; set; } = string.Empty;
        public DateTime? responseDatetime { get; set; }
    }
    public class NotificationLogDTO
    {
        public int recID { get; set; }
        public int typeID { get; set; }
        public string recipient { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public DateTime? sentDate { get; set; }
        public bool isSent { get; set; } = false;
    }
}
