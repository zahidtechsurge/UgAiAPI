using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class Month
    {
        public int monthID { get; set; }
        public decimal totalProducts { get; set; }
        public int productID { get; set; }
        public string product { get; set; }
        public string uom { get; set; }
    }
    public class getMonths
    {
        public int monthID { get; set;}
        public string month { get; set;}
    }
}
