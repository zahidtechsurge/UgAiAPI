using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using Google.Apis.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Esf;

namespace AmazonFarmerAPI.Extensions
{
    public class GoogleLocationExtension
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public GoogleLocationExtension(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = ConfigExntension.GetConfigurationValue("GoogleMaps:ApiKey");
        }

        public async Task<double> GetDistanceBetweenLocations(getDistance req)
        {
            string warehouseLocations = JsonConvert.SerializeObject(req.WarehouseLocations);
            string farmLocation = JsonConvert.SerializeObject(new { lat = req.farmLatitude, lng = req.farmLongitude });

            string apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={warehouseLocations}&destinations={warehouseLocations}&key={_apiKey}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve distance from Google Maps API.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(content);

            var distanceInMeters = (double)jsonObject["rows"][0]["elements"][0]["distance"]["value"];
            var distanceInKilometers = distanceInMeters / 1000;
            //var distanceInKilometers = 0;// distanceInMeters / 1000;

            return distanceInKilometers;
        }
        public async Task<getFarmLocation> GetlatLngForAddress(FarmAddressDTO req)
        {
            string completeAddress = $"{req.Name}, {req.Address1}, {req.Address2}, {req.City}, {req.Tehsil}, {req.District}";

            string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={completeAddress}&key={_apiKey}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve distance from Google Maps API.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(content);

            var lat = (double)jsonObject["results"][0]["geometry"]["location"]["lat"];
            var lng = (double)jsonObject["results"][0]["geometry"]["location"]["lng"];

            return new getFarmLocation()
            {
                latitude = lat,
                longitude = lng,
            };
        }
    }
}
