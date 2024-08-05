using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Services.Repositories;
using AmazonFarmerAPI.Extensions;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Net.Mail;

namespace AmazonFarmerAPI.Controllers
{
    /// <summary>
    /// Controller for managing attachment-related operations.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly IAzureFileShareService _azureFileShareService;
        // Constructor injection of IRepositoryWrapper.
        public AttachmentController(IRepositoryWrapper repoWrapper, IAzureFileShareService azureFileShareService)
        {
            _repoWrapper = repoWrapper;
            _azureFileShareService = azureFileShareService;
        }

        [AllowAnonymous]
        [HttpPost("uploadAttachments")]
        public async Task<APIResponse> uploadAttachments(uploadAttachmentReq req)
        {
            APIResponse resp = new APIResponse();
            resp.response = new List<uploadAttachmentResp>();

            AttachmentExtension attachmentExt = new AttachmentExtension(_repoWrapper, _azureFileShareService);
            foreach (_uploadAttachmentReq attachmentReq in req.attachments)
            {
                uploadAttachmentResp attachment = await attachmentExt.uploadAttachment(attachmentReq);
                resp.response.Add(attachment);
            }

            return resp;
        }
        [AllowAnonymous]
        [HttpGet("getAttachmentByGUID/{GUID}")]
        public async Task<dynamic> GetFile(Guid GUID)
        {
            if (string.IsNullOrEmpty(GUID.ToString()))
                return NotFound();
            tblAttachment attachment = await _repoWrapper.AttachmentRepo.getAttachmentByGUID(GUID);

            if (attachment == null)
                return NotFound();

            attachment.Path = attachment.Path;

            Stream? imageOutput = null;
            if (attachment.Path.Contains("private-documents"))
            {
                imageOutput = await _azureFileShareService.GetFileAsync(attachment.Path);
            }
            else
            {
                imageOutput = System.IO.File.OpenRead(attachment.Path);
            }
            var contentType = "image/jpeg";

            if (attachment.FileType == ".pdf")
                contentType = "application/pdf";

            return base.File(imageOutput, contentType, "");
        }

        [AllowAnonymous]
        [HttpGet("getPublicFile/{filePath}")]
        public dynamic GetPublicFile(string filePath)
        {
            //var connectionString = ConfigExntension.GetConfigurationValue("AzureFileStorage:ConnectionString");
            //var shareName = ConfigExntension.GetConfigurationValue("AzureFileStorage:ShareName");
            //var directoryName = ConfigExntension.GetConfigurationValue("AzureFileStorage:ServiceReportDirectory");
            //ShareClient shareClient = new ShareClient(connectionString, shareName);
            //ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(directoryName);
            //return await getSoilSampleFileByFileName(directoryClient,filePath);

            string currentDirectory = Directory.GetCurrentDirectory();
            var inputDirectory = currentDirectory + ConfigExntension.GetConfigurationValue("FTPFileStorage:tempFolder");
            var archiveDirectory = currentDirectory + ConfigExntension.GetConfigurationValue("FTPFileStorage:ArchiveDirectory");

            var host = ConfigExntension.GetConfigurationValue("FTPFileStorage:host");
            var port = ConfigExntension.GetConfigurationValue("FTPFileStorage:port");
            var username = ConfigExntension.GetConfigurationValue("FTPFileStorage:username");
            var password = ConfigExntension.GetConfigurationValue("FTPFileStorage:password");
            var remoteDirectory = ConfigExntension.GetConfigurationValue("FTPFileStorage:ServerPath");
            var remoteArchiveDirectory = ConfigExntension.GetConfigurationValue("FTPFileStorage:ServerArchive");

            using (var sftp = new SftpClient(host, Convert.ToInt32(port), username, password))
            {
                // Connect to the SFTP server
                sftp.Connect();
                // Download a file
                var remoteFiles = sftp.ListDirectory(remoteDirectory)
                            .Where(file => file.IsRegularFile && file.Name.ToLower().StartsWith(filePath.ToLower()) && file.Name.EndsWith(".pdf"))
                            .FirstOrDefault();
                if (remoteFiles != null)
                {
                    string remoteFilePath = $"{remoteDirectory}/{remoteFiles.Name}";
                    string dateTimeFileName = remoteFiles.Name.Replace(".pdf", "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf");
                    string localFilePath = Path.Combine(inputDirectory, remoteFiles.Name);
                    string remoteArchiveFilePath = $"{remoteArchiveDirectory}/{dateTimeFileName}";
                    // Ensure the local directory exists
                    Directory.CreateDirectory(inputDirectory);
                    // Download the file
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        sftp.DownloadFile(remoteFilePath, fileStream);
                    }
                    // Disconnect from the SFTP server
                    sftp.Disconnect();
                    return ReadPDFFromTemp(localFilePath);
                }
            }
            return NotFound();
        }

        private FileStreamResult ReadPDFFromTemp(string filePath)
        {
            Stream imageOutput = System.IO.File.OpenRead(filePath);
            return base.File(imageOutput, "application/pdf", "");

        }

        private async Task<dynamic> getSoilSampleFileByFileName(ShareDirectoryClient directoryClient, string filename)
        {
            ShareFileClient fileClient = directoryClient.GetFileClient(filename);
            try
            {
                ShareFileDownloadInfo download = await fileClient.DownloadAsync();
                MemoryStream memoryStream = new MemoryStream();
                await download.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset the stream position to the beginning
                return memoryStream;
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                // Handle file not found exception
                return NotFound();
            }

        }

    }
}
