using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class RequestLog
    {
        [Key]
        public int RequestId { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<ResponseLog> Responses { get; set; }
    }
}
