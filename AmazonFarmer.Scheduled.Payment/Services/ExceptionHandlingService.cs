using AmazonFarmer.NotificationServices.Helpers;
using Newtonsoft.Json; 
using System.Text; 

namespace AmazonFarmer.Scheduled.Payment.Services
{
    public class ExceptionHandlingService
    {


        //For Exception Logging 
        public static async Task HandleExceptionAsync(Exception exception, string requestTypeName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            var exceptionDetails = GetExceptionDetails(exception);
            var json = JsonConvert.SerializeObject(exceptionDetails);
            string filePath = Path.Combine(currentDirectory, $"logs/Exceptions/{requestTypeName}_{DateTime.UtcNow:yyyy-MM-ddThhmmss-7199222}.json");
            await WriteFileAsync(filePath, json);
        }

        private static ExceptionDetails GetExceptionDetails(Exception ex)
        {
            if (ex == null) return null;

            return new ExceptionDetails
            {
                Type = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Data = ex.Data,
                InnerException = GetExceptionDetails(ex.InnerException)
            };
        }

        private static async Task WriteFileAsync(string filePath, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await stream.WriteAsync(byteData, 0, byteData.Length);
            }
        }
    }
}
