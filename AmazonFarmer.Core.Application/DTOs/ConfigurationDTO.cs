using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class UpdateConfigRequest
    {
        public int configID { get; set; }
        public string value { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int status { get; set; }
    }
}
