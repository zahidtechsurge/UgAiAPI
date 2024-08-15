using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.WSDL.Helpers
{
    public class WsdlConfig
    {
        public bool IsDevMode { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<WsdlUrl> EndPoints { get; set; } = new List<WsdlUrl>();
    }
    public class WsdlUrl
    {
        public string Url { get; set; } = string.Empty;
        public string EndpointConfiguration { get; set; } = string.Empty;
    }
}
