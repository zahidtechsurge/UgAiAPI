namespace AmazonFarmer.NotificationServices.Helpers
{
    public class EmailConfiguration
    {
        public bool IsAllowed { get; set; }
        public bool IsDevMode { get; set; }
        public string DevName { get; set; }
        public string DevMail { get; set; }
        public string Host { get; set; }
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string FromEmail { get; set; }
        public string ToFinanceEmail { get; set; } 

    }

}
