using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using Google.Apis.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Esf;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonFarmerAPI.Extensions
{
    public class GoogleLocationExtension
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _distanceURL;
        private readonly string _geocodeURL;
        private readonly string _directionURL;
        public GoogleLocationExtension(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = ConfigExntension.GetConfigurationValue("GoogleMaps:ApiKey");
            _distanceURL = ConfigExntension.GetConfigurationValue("GoogleMaps:DistanceURL");
            _geocodeURL = ConfigExntension.GetConfigurationValue("GoogleMaps:GeocodeURL");
            _directionURL = ConfigExntension.GetConfigurationValue("GoogleMaps:DirectionURL");
        }
        private static string? removeLastChar(string s)
        {
            return (s == null || s.Length == 0)
              ? null
              : (s.Substring(0, s.Length - 1));
        }
        private static string BuildLocationsString(IEnumerable<LocationDTO> locations)
        {
            var sb = new StringBuilder();
            foreach (var location in locations)
            {
                sb.Append($"{location.latitude},{location.longitude}|");
            }
            return sb.ToString().TrimEnd('|');
        }
        private string BuildApiUrl(string farmLocation, string warehouseLocations, string apiKey)
        {
            return $"{_distanceURL}?origins={farmLocation}&destinations={warehouseLocations}&key={apiKey}";
        }
        private static List<Element> FlattenApiResponse(List<Root?> apiResponses)
        {
            return apiResponses
                .Where(resp => resp != null)
                .SelectMany(resp => resp.rows)
                .SelectMany(row => row.elements)
                .ToList();
        }
        private static void UpdateWarehouseLocationsWithDistances(List<LocationDTO> warehouseLocations, List<Element> deserializedElements)
        {
            for (int i = 0; i < warehouseLocations.Count; i++)
            {
                if (i < deserializedElements.Count)
                {
                    warehouseLocations[i].distance = deserializedElements[i].distance.value;
                    warehouseLocations[i].distanceText = deserializedElements[i].distance.text;
                }
                else
                {
                    // Handle cases where deserializedElements has fewer entries than warehouseLocations
                    warehouseLocations[i].distance = 0;
                    warehouseLocations[i].distanceText = "Unknown location";
                }
            }
        }

        public async Task<dynamic> GetDestination(string origin, string destination)
        {
            var apiUrl = $"{_directionURL}".Replace("[origin]", origin)
                                    .Replace("[destination]", destination)
                                    .Replace("[apiKey]", _apiKey);

            var response = await _httpClient.GetAsync(apiUrl).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve distance from Google Maps API.");
            }
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }

        public async Task<getDistance> GetDistanceBetweenLocations(getDistance req)
        {
            List<Root?> deserializedAPIResponse = new();
            int filterationChunkSize = 25;
            // Partition the list into chunks of 25
            var chunks = req.WarehouseLocations.Partition(filterationChunkSize);
            foreach (var chunk in chunks)
            {
                string warehouseLocations = BuildLocationsString(chunk);
                //string warehouseLocations = "";
                string farmLocation = $"{req.farmLatitude},{req.farmLongitude}";
                //foreach (LocationDTO wlocation in chunk)
                //{
                //    warehouseLocations += $"{wlocation.latitude},{wlocation.longitude}|";
                //}
                //warehouseLocations = removeLastChar(warehouseLocations);
                string apiUrl = BuildApiUrl(farmLocation, warehouseLocations, _apiKey);
                //string apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={farmLocation}&destinations={warehouseLocations}&key={_apiKey}";

                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new AmazonFarmerException("Failed to retrieve distance from Google Maps API.");
                }

                var content = await response.Content.ReadAsStringAsync();
                //var jsonObject = JObject.Parse(content);

                var respObj = JsonConvert.DeserializeObject<Root>(content);
                deserializedAPIResponse.Add(respObj);
            }
            List<Element> deserializedElements = FlattenApiResponse(deserializedAPIResponse);
            UpdateWarehouseLocationsWithDistances(req.WarehouseLocations, deserializedElements);
            //foreach (var apiResp in deserializedAPIResponse)
            //{
            //    deserializedElements.AddRange(apiResp.rows.First().elements);
            //}
            //for (int i = 0; i < req.WarehouseLocations.Count(); i++)
            //{
            //        req.WarehouseLocations[i].distance = deserializedElements[i].distance.value;
            //        req.WarehouseLocations[i].distanceText = deserializedElements[i].distance.text;
            //}

            return req;
        }
        
        public async Task<getFarmLocation> GetlatLngForAddress(FarmAddressDTO req)
        {
            string completeAddress = $"{req.Address1}, {req.Address2}, {req.City}, {req.Tehsil}, {req.District.Replace("#N/A", "")}";

            string apiUrl = $"{_geocodeURL}?address={completeAddress}&key={_apiKey}";

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
    public static class ListExtensions
    {
        public static IEnumerable<List<T>> Partition<T>(this List<T> source, int size)
        {
            for (int i = 0; i < source.Count; i += size)
            {
                yield return source.GetRange(i, Math.Min(size, source.Count - i));
            }
        }
    }
}
