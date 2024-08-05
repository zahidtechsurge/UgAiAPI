using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class WSDLLog
    {
        [Key]
        public int RequestId { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public DateTime RequestTimestamp { get; set; }

        public string? Status { get; set; }
        public string? ResponseBody { get; set; }
        public DateTime? ResponseTimestamp { get; set; }
    }
}
