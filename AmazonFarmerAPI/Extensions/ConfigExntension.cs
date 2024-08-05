namespace AmazonFarmerAPI.Extensions
{
    public class ConfigExntension
    {
        public static string GetConfigurationValue(string ConfigName)
        {
            IConfiguration config = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", true, true)
                     .Build();
            IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

            return configuration[ConfigName];
        }
    }
}
