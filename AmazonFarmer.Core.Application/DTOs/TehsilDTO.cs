using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class TehsilDTO
    {
        public int cityID { get; set; }
        public int tehsilID { get; set; }
        public string tehsilName { get; set; }
        public string languageCode { get; set; }
    }
    public class getTehsil_Req
    {
        public string languageCode { get; set; }
    }
}
