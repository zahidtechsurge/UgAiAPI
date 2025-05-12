using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class HelpDTO_Resp
    {
        public string name { get; set; } = string.Empty;
        public string designation { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string district { get; set; } = string.Empty;
    }
}
