using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class ResponseLog
    {
        [Key]
        public int ResponseId { get; set; }
        public int RequestId { get; set; }
        public int StatusCode { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual RequestLog Request { get; set; }
    }
}
