using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{

    public partial class TaxCertificateRequest
    {
        public string companyCode { get; set; } = string.Empty;
        public string startDate { get; set; } = string.Empty;
        public string endDate { get; set; } = string.Empty;
    }


}
