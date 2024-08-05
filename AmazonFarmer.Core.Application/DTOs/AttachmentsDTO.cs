using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces

namespace AmazonFarmer.Core.Application.DTOs
{
    public class AttachmentsDTO // Data Transfer Object for Attachments
    {
        public string fileName { get; set; } // Property for file Name
        public string filePath { get; set; } // Property for file path
        public string fileType { get; set; } // Property for file type
        public EAttachmentType attachmentType { get; set; } // Property for attachment type
    }

    public class attachAttachment // Data Transfer Object for attaching attachment
    {
        public string userID { get; set; } // Property for user ID
        public int farmID { get; set; } // Property for farm ID
        public int attachmentID { get; set; } // Property for attachment ID
    }

    public class farmAttachment // Data Transfer Object for farm attachment
    {
        public string name { get; set; } // Property for name
        public string GUID { get; set; } // Property for content
    }
    public class uploadAttachmentReq
    {
        public List<_uploadAttachmentReq> attachments { get; set; }
    }
    public class _uploadAttachmentReq // Data Transfer Object for attachment upload request
    {
        /// <summary>
        /// Property for name
        /// </summary>
        public string name { get; set; } = string.Empty;
        /// <summary>
        /// Property for content
        /// </summary>
        public string content { get; set; } = string.Empty;
        /// <summary>
        /// Property for content bytes
        /// </summary>
        public byte[] contentBytes { get; set; } = [];
        /// <summary>
        /// Property for requestTypeID
        /// </summary>
        public int requestTypeID { get; set; }
    }
    public class uploadAttachmentResp // Data Transfer Object for attachment upload response
    {
        public int id { get; set; } // Property for ID
        public string name { get; set; } = string.Empty; // Property for name
        public string type { get; set; } = string.Empty; // Property for content
        public string guid { get; set; } /// <summary>Property for GUID</summary>
    }
    /// <summary>Data Transfer Object for fetching the attachment by attachment GUID</summary>
    public class getAttachment_Req
    {
        public Guid guid { get; set; } /// <summary>Property for GUID</summary>
    }
}
