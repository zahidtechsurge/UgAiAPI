using System;
using System.Collections.Generic;
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
        public DateTime requestDatetime { get; set; }
        public int statusCode { get; set; }
        public string status { get; set; } = string.Empty;
        public string response { get; set; } = string.Empty;
        public DateTime? responseDatetime { get; set; }
    }
}
