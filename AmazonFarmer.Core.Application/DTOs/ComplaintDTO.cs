using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class AddComplaintRequest
    {
        public string title { get; set; } = string.Empty;
        public string desc { get; set; } = string.Empty;
    }
    public class UpdateComplaintRequest
    {
        public int complaintID { get; set; }
        public int statusID { get; set; }
    }
    public class GetComplaintRequest
    {
        public string search { get; set; } = string.Empty;
        public int skip { get; set; } = 0;
        public int take { get; set; } = 10;

    }
    public class GetComplaintsList
    {
        public int complaintID { get; set; }
        public string createdBy { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string desc { get; set; } = string.Empty;
        public int statusID { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
