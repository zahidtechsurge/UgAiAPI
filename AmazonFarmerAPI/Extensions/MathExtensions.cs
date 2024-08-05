using AmazonFarmer.Core.Application.DTOs;

namespace AmazonFarmerAPI.Extensions
{
    public class MathExtensions
    {
        // Method to calculate the advance price
        public decimal AdvanceValue(decimal percent, decimal totalPrice)
        {
            // Calculating the advance amount based on the percentage of the total price
            decimal advanceAmount = (totalPrice * percent) / 100;

            return advanceAmount;
        }

        // Method to convert degrees to radians
        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        // Method to calculate distance between two points using Haversine formula
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Radius of the Earth in kilometers
            double radius = 6371;

            // Convert latitude and longitude from degrees to radians
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            // Calculate Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = radius * c;

            return distance;
        }

        // Method to find nearest locations
        public List<LocationDTO> FindNearestLocations(currentLocation currentLocation, List<LocationDTO> locations)
        {
            //// Calculate distances from current location to all locations
            //foreach (var location in locations)
            //{
            //    location.distance = CalculateDistance(
            //        currentLocation.latitude,
            //        currentLocation.longitude,
            //        location.latitude,
            //        location.longitude
            //    );
            //}

            // Sort locations by distance
            locations.Sort((x, y) => x.distance.CompareTo(y.distance));

            return locations;
        }
    }
}
