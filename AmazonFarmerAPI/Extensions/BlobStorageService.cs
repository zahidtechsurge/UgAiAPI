using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AmazonFarmerAPI.Extensions
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            _blobContainerClient = new BlobContainerClient(_connectionString, _containerName);
        }

        public async Task UploadBlobAsync(string blobName, byte[] content)
        {
            blobName = Regex.Replace(blobName, @"[\\]", "-");
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            using (var stream = new MemoryStream(content))
            {
                await blobClient.UploadAsync(stream, overwrite: false);
            }
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            var download = await blobClient.DownloadAsync();
            return download.Value.Content;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}