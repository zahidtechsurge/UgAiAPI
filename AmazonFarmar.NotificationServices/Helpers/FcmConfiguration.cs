namespace AmazonFarmer.NotificationServices.Helpers
{
    public class FcmConfiguration
    {
        public string ServerKey { get; set; }
        public string FcmURL { get; set; }
    }
    public class SMSConfiguration
    {
        public string SMSOTPApi { get; set; }
        public string SMSApi { get; set; }
        public string SMSAction { get; set; }
        public string SMSUserName { get; set; }
        public string SMSPassword { get; set; }
        public string SMSOriginator { get; set; }
    }
    public class GoogleAPIConfiguration
    {
        public string ApiKey { get; set; }
        public string CordinateUrl { get; set; } 
    }
}
