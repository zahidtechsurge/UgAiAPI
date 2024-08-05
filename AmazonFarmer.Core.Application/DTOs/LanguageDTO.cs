using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class LanguageDTO
    {
        public string languageCode { get; set; }
        public string languageName { get; set; }
    }
    public class LanguageReq
    {
        public string languageCode { get; set;}
    }

}
