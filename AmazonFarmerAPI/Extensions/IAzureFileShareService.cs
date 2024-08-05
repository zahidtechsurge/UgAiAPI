namespace AmazonFarmerAPI.Extensions
{
    public interface IAzureFileShareService
    {
        Task UploadFileAsync(byte[] fileBytes, string fileName);
        Task<Stream> GetFileAsync(string fileName);
    }
}
