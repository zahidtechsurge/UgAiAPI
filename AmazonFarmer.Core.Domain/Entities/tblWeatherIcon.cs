using System.ComponentModel.DataAnnotations;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblWeatherIcon
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public EWeatherType WeatherType { get; set; }
        public EActivityStatus Status { get; set; }
        public virtual List<tblWeatherIconTranslation>? WeatherIconTranslations { get; set; } = null;
    }
}
