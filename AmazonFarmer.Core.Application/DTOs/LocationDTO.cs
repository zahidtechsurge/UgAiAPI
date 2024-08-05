using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Application.DTOs
{
    public class LocationDTO
    {
        public int locationID { get; set; }
        public string locationName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double distance { get; set; }
    }
    public class currentLocation
    {
        public int farmID { get; set; }
    }
    public class getPickupLocation_Resp
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<LocationDTO> pickupLocation { get; set; }
    }
    public class getDistance
    {
        public double farmLatitude { get; set; }
        public double farmLongitude { get; set; }
        public List<WarehouseLocation> WarehouseLocations { get; set; }
    }
    public class WarehouseLocation
    {
        public double lat { get; set; }
        public double lng{ get; set; }
    }
}
