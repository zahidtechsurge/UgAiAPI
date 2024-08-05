using AmazonFarmer.Core.Application.DTOs; // Importing necessary namespaces
using AmazonFarmer.Core.Domain.Entities; // Importing necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface IAttachmentRepo // Defining the interface for handling attachments
    {
        Task<tblAttachment> uploadAttachment(AttachmentsDTO req); // Method signature for uploading an attachment
        Task attachUserAttachment(attachAttachment req); // Method signature for attaching an attachment to a user
        Task updateUserAttachment(tblUserAttachments req); // Method signature for updating attachment
        Task updateFarmAttachment(tblFarmAttachments req); // Method signature for updating attachment
        Task<tblUserAttachments> getUserAttachment(int attachmentID); // Method signature for fetching user attachment by attachment ID
        Task<tblFarmAttachments> getfarmAttachment(int attachmentID); // Method signature for fetching user attachment by attachment ID
        Task attachFarmAttachment(attachAttachment req); // Method signature for attaching an attachment to a farm
        Task<tblAttachment> getAttachmentByGUID(Guid GUID); // Method signature for fetching an attachment by attachment GUID
        Task<tblAttachment> getAttachmentByID(int attachmentID); // Method signature for fetching an attachment by attachmentID
    }
}
