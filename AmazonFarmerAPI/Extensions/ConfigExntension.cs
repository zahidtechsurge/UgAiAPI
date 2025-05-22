using System.ComponentModel;

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
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static TAttribute? GetAttribute<TEnum, TAttribute>(TEnum value)
        where TEnum : Enum
        where TAttribute : Attribute
        {
            var memberInfo = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
            return memberInfo?.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
        }
    }
}
