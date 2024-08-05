using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class JSONResponse
    {
        public bool isError { get; set; } = false;
        public string message { get; set; } = string.Empty;
    }
    public class APIResponse
    {
        public bool isError { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public dynamic response { get; set; } = string.Empty;
    }
    public class DropDownValues
    {
        public string languageCode { get; set; } = string.Empty;
        public string key { get; set; }
        public string value { get; set; }
    }
}
