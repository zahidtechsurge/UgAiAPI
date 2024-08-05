using AmazonFarmer.Core.Application.DTOs;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public class accuWeatherExtension
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public accuWeatherExtension(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = ConfigExntension.GetConfigurationValue("AccuWeather:ApiKey");
        }
        //public async Task<getWeatherAPI_Req> getLocation(WarehouseLocation req)
        //{
        //    getWeatherAPI_Req resp = new getWeatherAPI_Req();
        //    string apiUrl;

        //    // Choose one method to get the location key, either by address or by latitude/longitude
        //    if (!string.IsNullOrEmpty(req.address))
        //    {
        //        // Search by address
        //        apiUrl = $"http://dataservice.accuweather.com/locations/v1/search?q={req.address}&apikey={_apiKey}";
        //    }
        //    else
        //    {
        //        // Search by latitude/longitude
        //        apiUrl = $"http://dataservice.accuweather.com/locations/v1/cities/geoposition/search?apikey={_apiKey}&q={req.lat},{req.lng}";
        //    }

        //    using (var httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            var response = await httpClient.GetAsync(apiUrl);
        //            response.EnsureSuccessStatusCode(); // Throw exception if not successful

        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            var respObj = JsonConvert.DeserializeObject<AccuWeatherDTO>(responseBody);
        //            resp = new getWeatherAPI_Req()
        //            {
        //                locationKey = respObj.Key,
        //                weatherArea = respObj.AdministrativeArea.LocalizedName + ", " + respObj.AdministrativeArea.CountryId
        //            };
        //        }
        //        catch (HttpRequestException ex)
        //        { }
        //    }
        //    return resp;
        //}
        //public async Task<WeatherDTO> getWeather(getWeatherAPI_Req req)
        //{
        //    WeatherDTO resp = new WeatherDTO();
        //    string apiUrl = $"http://dataservice.accuweather.com/currentconditions/v1/{req.locationKey}?apikey={_apiKey}&details=true";

        //    using (var httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            var response = await httpClient.GetAsync(apiUrl);
        //            response.EnsureSuccessStatusCode(); // Throw exception if not successful

        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            var apiResp = JsonConvert.DeserializeObject<List<WeatherData>>(responseBody);

        //            resp = new WeatherDTO()
        //            {
        //                weatherIconID = apiResp[0].WeatherIcon,
        //                //weatherText = apiResp[0].WeatherText,
        //                weatherArea = req.weatherArea,
        //                weatherUnit = apiResp[0].Temperature.Metric.Unit,
        //                weatherValue = apiResp[0].Temperature.Metric.Value
        //            };
        //        }
        //        catch (HttpRequestException ex)
        //        { }
        //    }

        //    return resp;
        //}
    }
}
