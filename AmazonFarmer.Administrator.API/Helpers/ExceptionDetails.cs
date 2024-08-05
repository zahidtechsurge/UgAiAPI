namespace AmazonFarmer.Administrator.API.Helpers
{
    public class ExceptionDetails
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public dynamic Data { get; set; }
        public ExceptionDetails? InnerException { get; set; }
    }
}
