using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Services.Repositories;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public class AttachmentExtension
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly IAzureFileShareService _azureFileShareService;
        public AttachmentExtension(IRepositoryWrapper repositoryWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repositoryWrapper;
            _azureFileShareService = azureFileShareService;
        }
        public async Task<uploadAttachmentResp> UploadAttachment(_uploadAttachmentReq attachmentReq)
        {
            // Decode the base64 string into a byte array
            string filePath = "";
            byte[] imageBytes = string.IsNullOrEmpty(attachmentReq.content) ? attachmentReq.contentBytes : Convert.FromBase64String(attachmentReq.content);
            string fileName = string.Concat(DateTime.UtcNow.ToString("ddMMyyyy_hhmmff"), attachmentReq.name); 
            filePath = Path.Combine(
                    getFilePathByRequestType((EAttachmentType)attachmentReq.requestTypeID),
                    fileName
             ); // Adjust the path as needed
            // Write the image bytes to the file
            File.WriteAllBytes(filePath, imageBytes);

            // Return the URL of the saved image
            string attachmentPath = $"{filePath}"; // Adjust the URL path as needed
            // Full file Info.
            FileInfo fileInfo = new FileInfo(attachmentPath);
            AttachmentsDTO attachmentsDTO = new AttachmentsDTO()
            {
                attachmentType = (EAttachmentType)attachmentReq.requestTypeID,
                filePath = attachmentPath,
                fileType = fileInfo.Extension,
                fileName = attachmentReq.name
            };
            tblAttachment uploadedAttachment = await _repoWrapper.AttachmentRepo.uploadAttachment(attachmentsDTO);

            uploadAttachmentResp attachment = new uploadAttachmentResp()
            {
                type = ((EAttachmentType)attachmentReq.requestTypeID).ToString(),
                name = attachmentReq.name,
                id = uploadedAttachment.ID,
                guid = uploadedAttachment.Guid.ToString()
            };
            return attachment;
        }

        public async Task<AttachmentsDTO> UploadAttachment(string name, string content, EAttachmentType requestTypeID)
        {
            string fileName = string.Concat(DateTime.UtcNow.ToString("ddMMyyyy_hhmmff"), name);
            byte[] imageBytes = Convert.FromBase64String(content);
            string filePath = Path.Combine(
                    getFilePathByRequestType(requestTypeID),
                    fileName
             ); // Adjust the path as needed
                // Write the image bytes to the file
                //File.WriteAllBytes(filePath, imageBytes);

            await _azureFileShareService.UploadFileAsync(imageBytes, filePath);
            // Return the URL of the saved image
            string attachmentPath = $"{filePath}"; // Adjust the URL path as needed
            FileInfo fileInfo = new FileInfo(attachmentPath); // Full file Info.
            AttachmentsDTO attachmentsDTO = new AttachmentsDTO()
            {
                attachmentType = requestTypeID,
                filePath = attachmentPath,
                fileType = fileInfo.Extension,
                fileName = name
            };
            return attachmentsDTO;
        }


        private string getFilePathByRequestType(EAttachmentType requestType)
        {
            string resp = string.Empty;

            switch (requestType)
            {case EAttachmentType.HomeBanner:
                    resp = Path.Combine("attachments", "home-banners");
                    break;
                case EAttachmentType.IntroBanner:
                    resp = Path.Combine("attachments", "intro-banners");
                    break;
                case EAttachmentType.LoginBanner:
                    resp = Path.Combine("attachments", "login-banners");
                    break;
                case EAttachmentType.Product:
                    resp = Path.Combine("attachments", "products");
                    break;
                case EAttachmentType.Crop:
                    resp = Path.Combine("attachments", "crops");
                    break;
                case EAttachmentType.Service:
                    resp = Path.Combine("attachments", "services");
                    break;
                default:
                    resp = Path.Combine("attachments");
                    break;
            }
            return resp;
        }
    }
}
