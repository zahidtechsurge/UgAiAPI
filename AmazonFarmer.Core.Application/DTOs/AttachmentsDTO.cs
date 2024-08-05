using AmazonFarmer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class AttachmentsDTO
    {
        public string filePath { get; set; }
        public string fileType { get; set; }
        public EAttachmentType attachmentType { get; set; }
    }
    public class attachAttachment
    {
        public string userID { get; set; }
        public int farmID { get; set; }
        public int attachmentID { get; set; }
    }
    public class farmAttachment
    {
        public string name { get; set; }
        public string content { get; set; }
    }
}
