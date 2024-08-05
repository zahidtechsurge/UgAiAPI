using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Google.Apis.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Esf;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonFarmer.Administrator.API.Extensions
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
        private static string? removeLastChar(string s)
        {
            return (s == null || s.Length == 0)
              ? null
              : (s.Substring(0, s.Length - 1));
        }
        public async Task<getDistance> GetDistanceBetweenLocations(getDistance req)
        {
            string warehouseLocations = "";
            string farmLocation = $"{req.farmLatitude},{req.farmLongitude}";
            foreach (LocationDTO wlocation in req.WarehouseLocations)
            {
                warehouseLocations += $"{wlocation.latitude},{wlocation.longitude}|";
            }
            warehouseLocations = removeLastChar(warehouseLocations);

            string apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={farmLocation}&destinations={warehouseLocations}&key={_apiKey}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
               throw new AmazonFarmerException("Failed to retrieve distance from Google Maps API.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(content);

            var respObj = JsonConvert.DeserializeObject<Root>(content);

            for (int i = 0; i < req.WarehouseLocations.Count(); i++)
            {
                req.WarehouseLocations[i].distance = respObj.rows[0].elements[i].distance.value;
            }
            return req;
        }
        public async Task<getFarmLocation> GetlatLngForAddress(FarmAddressDTO req)
        {
            string completeAddress = $"{req.Address1}, {req.Address2}, {req.City}, {req.Tehsil}, {req.District}";

            string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={completeAddress}&key={_apiKey}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
               throw new AmazonFarmerException("Failed to retrieve distance from Google Maps API.");
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
