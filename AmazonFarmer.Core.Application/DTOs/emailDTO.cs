using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class emailDTO
    {
        public string name { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool isHTML { get; set; }
        public string fromUser { get; set; }
        public string toUser { get; set; }
        public Email_Type EmailType { get;set; }
    }
}
