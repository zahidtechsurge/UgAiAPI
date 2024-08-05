using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class ServiceDTO
    {
        public int serviceID { get; set; }
        public string serviceName { get; set; }
        public string filePath { get; set; }
        public int postDeliveryIn { get; set; }
    }
    public class Server_Req
    {
        public int serviceID { get; set;}
        public string startDate { get; set;}
        public string endDate { get; set;}
    }
}
