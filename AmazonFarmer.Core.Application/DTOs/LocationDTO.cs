using System;
using System.Collections.Generic;

namespace AmazonFarmer.Core.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for location information.
    /// </summary>
    public class LocationDTO
    {
        // The location ID
        public int locationID { get; set; }

        // The name of the location
        public string locationName { get; set; }

        // The latitude coordinate of the location
        public double latitude { get; set; }

        // The longitude coordinate of the location
        public double longitude { get; set; }

        // The distance of the location
        public double distance { get; set; }
        // The distance label of the location
        public string distanceText { get; set; }
    }

    /// <summary>
    /// DTO for current location.
    /// </summary>
    public class currentLocation
    {
        // The ID of the farm
        public int farmID { get; set; }
    }

    /// <summary>
    /// Response DTO for pickup locations.
    /// </summary>
    public class getPickupLocation_Resp
    {
        // The latitude coordinate
        public double latitude { get; set; }

        // The longitude coordinate
        public double longitude { get; set; }

        // List of pickup locations
        public List<LocationDTO> pickupLocation { get; set; }
    }

    /// <summary>
    /// DTO to get distance between farm and warehouse locations.
    /// </summary>
    public class getDistance
    {
        // The latitude coordinate of the farm
        public double farmLatitude { get; set; }

        // The longitude coordinate of the farm
        public double farmLongitude { get; set; }

        // List of warehouse locations
        public List<LocationDTO> WarehouseLocations { get; set; }
    }

    /// <summary>
    /// DTO for warehouse location.
    /// </summary>
    public class WarehouseLocation
    {
        // The latitude coordinate of the warehouse
        public double lat { get; set; }

        // The longitude coordinate of the warehouse
        public double lng { get; set; }

        // The address of the warehouse
        public string address { get; set; }

        // The city of the warehouse
        public string city { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Root
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
    }
}
