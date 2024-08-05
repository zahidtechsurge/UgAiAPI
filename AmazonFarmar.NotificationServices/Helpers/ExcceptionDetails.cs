using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.NotificationServices.Helpers
{
    public class ExceptionDetails
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public dynamic Data { get; set; }
        public ExceptionDetails? InnerException { get; set; }
    }
}
