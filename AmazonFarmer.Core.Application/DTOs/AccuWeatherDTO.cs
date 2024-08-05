using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class getWeatherAPI_Req
    {
        public string weatherArea { get; set; }
        public long locationKey { get; set; }
    }
    public class WeatherDTO
    {
        public string weatherIconID { get; set; }
        public string weatherIconPath { get; set; }
        public string weatherText { get; set; }
        public string weatherUnit { get; set; }
        public string weatherValue { get; set; }
        public string weatherArea { get; set; }
    }
    public partial class AccuWeatherDTO
    {
        [JsonProperty("Version")]
        public long Version { get; set; }

        [JsonProperty("Key")]
        public long Key { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Rank")]
        public long Rank { get; set; }

        [JsonProperty("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("EnglishName")]
        public string EnglishName { get; set; }

        [JsonProperty("PrimaryPostalCode")]
        public string PrimaryPostalCode { get; set; }

        [JsonProperty("Region")]
        public Accu_Country Region { get; set; }

        [JsonProperty("Country")]
        public Accu_Country Country { get; set; }

        [JsonProperty("AdministrativeArea")]
        public Accu_AdministrativeArea AdministrativeArea { get; set; }

        [JsonProperty("TimeZone")]
        public Accu_TimeZone TimeZone { get; set; }

        [JsonProperty("GeoPosition")]
        public Accu_GeoPosition GeoPosition { get; set; }

        [JsonProperty("IsAlias")]
        public bool IsAlias { get; set; }

        [JsonProperty("ParentCity")]
        public Accu_ParentCity ParentCity { get; set; }

        [JsonProperty("SupplementalAdminAreas")]
        public Accu_SupplementalAdminArea[] SupplementalAdminAreas { get; set; }

        [JsonProperty("DataSets")]
        public string[] DataSets { get; set; }
    }

    public partial class Accu_AdministrativeArea
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("EnglishName")]
        public string EnglishName { get; set; }

        [JsonProperty("Level")]
        public long Level { get; set; }

        [JsonProperty("LocalizedType")]
        public string LocalizedType { get; set; }

        [JsonProperty("EnglishType")]
        public string EnglishType { get; set; }

        [JsonProperty("CountryID")]
        public string CountryId { get; set; }
    }

    public partial class Accu_Country
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("EnglishName")]
        public string EnglishName { get; set; }
    }

    public partial class Accu_GeoPosition
    {
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("Elevation")]
        public Accu_Elevation Elevation { get; set; }
    }

    public partial class Accu_Elevation
    {
        [JsonProperty("Metric")]
        public Accu_Imperial Metric { get; set; }

        [JsonProperty("Imperial")]
        public Accu_Imperial Imperial { get; set; }
    }

    public partial class Accu_Imperial
    {
        [JsonProperty("Value")]
        public long Value { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }

        [JsonProperty("UnitType")]
        public long UnitType { get; set; }
    }

    public partial class Accu_ParentCity
    {
        [JsonProperty("Key")]
        public long Key { get; set; }

        [JsonProperty("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("EnglishName")]
        public string EnglishName { get; set; }
    }

    public partial class Accu_SupplementalAdminArea
    {
        [JsonProperty("Level")]
        public long Level { get; set; }

        [JsonProperty("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("EnglishName")]
        public string EnglishName { get; set; }
    }

    public partial class Accu_TimeZone
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("GmtOffset")]
        public long GmtOffset { get; set; }

        [JsonProperty("IsDaylightSaving")]
        public bool IsDaylightSaving { get; set; }

        [JsonProperty("NextOffsetChange")]
        public object NextOffsetChange { get; set; }
    }


    public class WeatherData
    {
        public DateTime LocalObservationDateTime { get; set; }
        public long EpochTime { get; set; }
        public string WeatherText { get; set; }
        public int WeatherIcon { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
        public bool IsDayTime { get; set; }
        public TemperatureData Temperature { get; set; }
        public TemperatureData RealFeelTemperature { get; set; }
        public TemperatureData RealFeelTemperatureShade { get; set; }
        public int RelativeHumidity { get; set; }
        public int IndoorRelativeHumidity { get; set; }
        public TemperatureData DewPoint { get; set; }
        public WindData Wind { get; set; }
        public WindData WindGust { get; set; }
        public int UVIndex { get; set; }
        public string UVIndexText { get; set; }
        public VisibilityData Visibility { get; set; }
        public string ObstructionsToVisibility { get; set; }
        public int CloudCover { get; set; }
        public CeilingData Ceiling { get; set; }
        public PressureData Pressure { get; set; }
        public PressureTendencyData PressureTendency { get; set; }
        public TemperatureData Past24HourTemperatureDeparture { get; set; }
        public TemperatureData ApparentTemperature { get; set; }
        public TemperatureData WindChillTemperature { get; set; }
        public TemperatureData WetBulbTemperature { get; set; }
        public TemperatureData WetBulbGlobeTemperature { get; set; }
        public PrecipitationData Precip1hr { get; set; }
        public PrecipitationSummaryData PrecipitationSummary { get; set; }
        public TemperatureSummaryData TemperatureSummary { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    public class TemperatureData
    {
        public TemperatureUnit Metric { get; set; }
        public TemperatureUnit Imperial { get; set; }
    }

    public class WindData
    {
        public WindDirectionData Direction { get; set; }
        public WindSpeedData Speed { get; set; }
    }

    public class WindDirectionData
    {
        public int Degrees { get; set; }
        public string Localized { get; set; }
        public string English { get; set; }
    }

    public class WindSpeedData
    {
        public SpeedUnit Metric { get; set; }
        public SpeedUnit Imperial { get; set; }
    }

    public class VisibilityData
    {
        public DistanceUnit Metric { get; set; }
        public DistanceUnit Imperial { get; set; }
    }

    public class CeilingData
    {
        public DistanceUnit Metric { get; set; }
        public DistanceUnit Imperial { get; set; }
    }

    public class PressureData
    {
        public PressureUnit Metric { get; set; }
        public PressureUnit Imperial { get; set; }
    }

    public class PressureTendencyData
    {
        public string LocalizedText { get; set; }
        public string Code { get; set; }
    }

    public class PrecipitationData
    {
        public DistanceUnit Metric { get; set; }
        public DistanceUnit Imperial { get; set; }
    }

    public class PrecipitationSummaryData
    {
        public PrecipitationData Precipitation { get; set; }
        public PrecipitationData PastHour { get; set; }
        public PrecipitationData Past3Hours { get; set; }
        public PrecipitationData Past6Hours { get; set; }
        public PrecipitationData Past9Hours { get; set; }
        public PrecipitationData Past12Hours { get; set; }
        public PrecipitationData Past18Hours { get; set; }
        public PrecipitationData Past24Hours { get; set; }
    }

    public class TemperatureSummaryData
    {
        public TemperatureRangeData Past6HourRange { get; set; }
        public TemperatureRangeData Past12HourRange { get; set; }
        public TemperatureRangeData Past24HourRange { get; set; }
    }

    public class TemperatureRangeData
    {
        public TemperatureData Minimum { get; set; }
        public TemperatureData Maximum { get; set; }
    }

    public class TemperatureUnit
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
        public string Phrase { get; set; }
    }

    public class SpeedUnit
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class DistanceUnit
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class PressureUnit
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

}
