using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces
using System;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// DTO for email details
    /// </summary>
    public class emailDTO
    {
        public string name { get; set; } // Property for recipient name
        public string subject { get; set; } // Property for email subject
        public string body { get; set; } // Property for email body
        public bool isHTML { get; set; } // Property to indicate whether email body is HTML formatted
        public string fromUser { get; set; } // Property for sender email
        public string toUser { get; set; } // Property for recipient email
        public Email_Type EmailType { get; set; } // Property for email type
    }
}
