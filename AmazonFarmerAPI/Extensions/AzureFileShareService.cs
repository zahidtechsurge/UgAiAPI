using Azure.Storage.Files.Shares;
using Azure;
using Microsoft.Identity.Client;
using Azure.Storage.Files.Shares.Models;
using AmazonFarmer.Core.Application.Exceptions;

namespace AmazonFarmerAPI.Extensions
{
    public class AzureFileShareService : IAzureFileShareService
    {
        private readonly string _connectionString;
        private readonly string _shareName;
        private readonly string _directoryName;

        public AzureFileShareService(string connectionString, string shareName, string directoryName)
        {
            _connectionString = connectionString;
            _shareName = shareName;
            _directoryName = directoryName;
        }
        public string[] allowedExtensions = new[]
            {
                ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", // Image files
                ".pdf",                                                    // PDF files
                ".doc", ".docx", ".txt", ".rtf",                           // Document files
                ".xls", ".xlsx",                                           // Excel files
            };

        public async Task UploadFileAsync(byte[] fileBytes, string fileName)
        {
            if (!IsValidFile(fileName))
            {
                throw new AmazonFarmerException(_exceptions.fileExtensionNotValid);
            }
            ShareClient shareClient = new ShareClient(_connectionString, _shareName);
            await shareClient.CreateIfNotExistsAsync();

            ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(_directoryName);
            await directoryClient.CreateIfNotExistsAsync();

            ShareFileClient fileClient = directoryClient.GetFileClient(fileName);

            using (MemoryStream stream = new MemoryStream(fileBytes))
            {
                long fileSize = stream.Length;
                long chunkSize = 3 * 1024 * 1024; // 3 MiB
                long offset = 0;

                await fileClient.CreateAsync(fileSize);

                while (offset < fileSize)
                {
                    long bytesToRead = Math.Min(chunkSize, fileSize - offset);
                    byte[] buffer = new byte[bytesToRead];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    using (MemoryStream chunkStream = new MemoryStream(buffer))
                    {
                        await fileClient.UploadRangeAsync(new HttpRange(offset, bytesRead), chunkStream);
                    }

                    offset += bytesRead;
                }
            }

            //using (MemoryStream stream = new MemoryStream(fileBytes))
            //{
            //    await fileClient.CreateAsync(stream.Length);
            //    await fileClient.UploadRangeAsync(new Azure.HttpRange(0, stream.Length), stream);
            //}
        }
        public bool IsValidFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            if (allowedExtensions.Contains(extension.ToLower()))
                return true;
            else return false;

        }
        public async Task<Stream> GetFileAsync(string fileName)
        {
            fileName = fileName.Replace("\\", "/");
            if (fileName.StartsWith("/"))
            {
                fileName = fileName.TrimStart('/');
            }
            ShareClient shareClient = new ShareClient(_connectionString, _shareName);
            ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(_directoryName);
            ShareFileClient fileClient = directoryClient.GetFileClient(fileName);

            if (await fileClient.ExistsAsync())
            {

                return await fileClient.OpenReadAsync();
            }
            throw new AmazonFarmerException("File not found");
        }
    }
}
