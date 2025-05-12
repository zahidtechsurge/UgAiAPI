using AmazonFarmer.Core.Application.DTOs;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public interface IAzureFileShareService
    {
        Task UploadFileAsync(byte[] fileBytes, string fileName);
        Task<Stream> GetFileAsync(string fileName);
        Task<List<GetAzureFilesResponse>> GetAzureFilesByFilePath(string filePath);
        Task<bool> CheckIfFileExistsAsync(string fileName);
        Task<string> RenameFileAsync(string fileName);

    }
}
