using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblWeatherIconTranslation
    {
        [Key]
        public int ID {  get; set; }
        public int WeatherID {  get; set; }
        public string LanguageCode {  get; set; }
        public string Image {  get; set; }
        public string Text {  get; set; }
        [ForeignKey("WeatherID")]
        public virtual tblWeatherIcon WeatherIcon { get; set; }
        [ForeignKey("LanguageCode")]
        public virtual tblLanguages Language { get; set; }
    }
}
