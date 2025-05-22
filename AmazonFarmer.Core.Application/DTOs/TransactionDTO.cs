using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class Reconfirmation_API_Configuration
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string APIKey { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;

    }
    public class Reconfirmation_API_Request
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Consumer_No { get; set; } = string.Empty;
        public string UCID { get; set; } = string.Empty;
    }
    public class Reconfirmation_API_Response
    {
        public string Response_Code { get; set; } = string.Empty;
        public string Consumer_No { get; set; } = string.Empty;
        public string Consumer_Detail { get; set; } = string.Empty;
        public string Initiator_Bank { get; set; } = string.Empty;
        public string Date_Paid { get; set; } = string.Empty;
        public string Amount_Paid { get; set; } = string.Empty;
        public string Tran_Auth_Id { get; set; } = string.Empty;
        public string Reserved { get; set; } = string.Empty;
    }
}