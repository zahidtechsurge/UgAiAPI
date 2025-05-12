using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using System.Collections.Generic;
using AmazonFarmer.Infrastructure.Persistence;
using AmazonFarmer.Infrastructure.Services.Repositories;
using System.IO;
using System.Net;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace AmazonFarmerAPI.Extensions
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
        public async Task<uploadAttachmentResp> uploadAttachment(_uploadAttachmentReq attachmentReq)
        {
            // Decode the base64 string into a byte array
            byte[] imageBytes = string.IsNullOrEmpty(attachmentReq.content) ? attachmentReq.contentBytes : Convert.FromBase64String(attachmentReq.content);
            string filePath = "";
            // Specify the directory where you want to save the image

            string fileName = string.Concat(DateTime.UtcNow.ToString("ddMMyyyy_hhmmff"), attachmentReq.name);


            filePath = Path.Combine(
                    getFilePathByRequestType((EAttachmentType)attachmentReq.requestTypeID),
                    fileName
             ); // Adjust the path as needed

            await _azureFileShareService.UploadFileAsync(imageBytes, filePath);

            //await _blobStorageService.UploadBlobAsync(filePath, imageBytes);


            // Write the image bytes to the file
            //System.IO.File.WriteAllBytes(filePath, imageBytes);

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

        private string getFilePathByRequestType(EAttachmentType requestType)
        {
            string resp = string.Empty;
            string privatePath = @"private-documents";

            switch (requestType)
            {
                case EAttachmentType.User_CNIC_Document:
                    resp = Path.Combine(privatePath, "CNIC");
                    break;
                case EAttachmentType.User_NTN_Document:
                    resp = Path.Combine(privatePath, "NTN");
                    break;
                case EAttachmentType.Farm_Document:
                    resp = Path.Combine(privatePath, "Farm-Document");
                    break;
                case EAttachmentType.General_Document:
                    resp = Path.Combine(privatePath, "General-Document");
                    break;
                case EAttachmentType.Verify_AuthorityLetter_NIC:
                    resp = Path.Combine(privatePath, "AuthorityLetter-CNIC");
                    break;
                case EAttachmentType.HomeBanner:
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
                case EAttachmentType.PDF_AuthorityLetter:
                    resp = Path.Combine(privatePath, "AuthorityLetter-PDF");
                    break;
                default:
                    resp = Path.Combine("attachments");
                    break;
            }
            return resp;
        }

    }
}
