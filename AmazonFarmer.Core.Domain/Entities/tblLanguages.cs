using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public class tblLanguages
    {
        [Key]
        public required string LanguageCode { get; set; }
        public string LanguageName { get; set; } = string.Empty;
        public EActivityStatus Status { get; set; } = EActivityStatus.Active;
        public virtual List<tblBannerLanguages> BannerLanguages { get; set; } = null!;
        public virtual List<tblCityLanguages> CityLanguages { get; set; } = null!;
        public virtual List<tblCropTranslation> CropTranslations { get; set; } = null!;
        public virtual List<tblDistrictLanguages> DistrictLanguages { get; set; } = null!;
        public virtual List<tblHomeSliderLanguages> HomeSliderLanguages { get; set; } = null!;
        public virtual List<tblIntroLanguages> IntroLanguages { get; set; } = null!;
        public virtual List<tblMonthTranslation> MonthTranslations { get; set; } = null!;
        public virtual List<tblProductTranslation> ProductTranslations { get; set; } = null!;
        public virtual List<tblSeasonTranslation> SeasonTranslations { get; set; } = null!;
        public virtual List<tblTehsilLanguages> TehsilLanguages { get; set; } = null!;
        public virtual List<tblProductCategoryTranslation> ProductCategoryTranslations { get; set; } = null!;
        public virtual List<tblUnitOfMeasureTranslation> UnitOfMeasureTranslations { get; set; } = null!;
        public virtual List<tblWeatherIconTranslation>? WeatherIconTranslations { get; set; } = null;
        public virtual List<tblReasonTranslation> ReasonTranslation { get; set; } = null!;
        public virtual List<tblDeviceNotificationTranslation> DeviceNotificationTranslations { get; set; } = null!;
        public virtual List<tblEmailNotificationTranslations> EmailNotificationTranslations { get; set; } = null!;
        public virtual List<tblwarehouseTranslation> WarehouseTranslations { get; set; } = null!;
        //public virtual List<tblSequenceTranslation>? tblSequenceTranslation { get; set; } = null;
    }
}
