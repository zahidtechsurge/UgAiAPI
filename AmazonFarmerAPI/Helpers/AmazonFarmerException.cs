namespace AmazonFarmerAPI.Helpers
{
    public class AmazonFarmerException : Exception
    {
        private string? _message { get; set; }
        public AmazonFarmerException(string? message)

        {
            _message = message;
        }
    }
}
