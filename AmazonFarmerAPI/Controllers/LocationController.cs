using AmazonFarmer.Core.Application; // Importing necessary namespaces
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmerAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace AmazonFarmerAPI.Controllers // Defining namespace for the controller
{
    [ApiController] // Indicates that this class is an API controller
    [Authorize(AuthenticationSchemes = "Bearer")] // Authorizes access using Bearer authentication
    [Route("api/[controller]")] // Defines the base route for API endpoints, where [controller] will be replaced by the controller name
    public class LocationController : ControllerBase // Inherits from ControllerBase for API controller functionality
    {
        private IRepositoryWrapper _repoWrapper; // Repository wrapper to interact with data
        private readonly GoogleLocationExtension _googleLocationExtension; // Google location extension for distance calculations

        public LocationController(IRepositoryWrapper repoWrapper, GoogleLocationExtension googleLocationExtension) // Constructor for initializing repository wrapper and Google location extension
        {
            _repoWrapper = repoWrapper; // Initializing the repository wrapper
            _googleLocationExtension = googleLocationExtension; // Initializing the Google location extension
        }
        [AllowAnonymous]
        [HttpPost("getPickup")]
        public async Task<APIResponse> getPickup(currentLocation req) // Method to handle POST requests for getting pickup locations
        {
            APIResponse resp = new APIResponse(); // Initializing API response object
            List<LocationDTO> warehouseLocationDBs = await _repoWrapper.LocationRepo.getWarehouseLocationsByLanguageCode(User.FindFirst("languageCode")?.Value); // Retrieving warehouse locations by language code
            if (warehouseLocationDBs == null || warehouseLocationDBs.Count() <= 0) // Checking if warehouse locations are found
                throw new AmazonFarmerException(_exceptions.warehouseNotFound); // Throws exception if warehouse locations are not found

            getFarmLocation getfarmLocation = await _repoWrapper.FarmRepo.getFarmLocationByFarmID(req.farmID); // Retrieving farm location by farm ID
            if (getfarmLocation == null) // Checking if farm location is found
                throw new AmazonFarmerException(_exceptions.farmNotFound); // Throws exception if farm location is not found

            getDistance getDistance = new getDistance // Creating getDistance object for distance calculation
            {
                farmLatitude = getfarmLocation.latitude, // Setting farm latitude
                farmLongitude = getfarmLocation.longitude, // Setting farm longitude
                WarehouseLocations = warehouseLocationDBs // Initializing warehouse locations list
            };

            getDistance = await _googleLocationExtension.GetDistanceBetweenLocations(getDistance); // Getting distance between locations using Google location extension

            getDistance.WarehouseLocations = getDistance.WarehouseLocations.OrderBy(x => x.distance).ToList();

            resp.response = new getPickupLocation_Resp() // Creating response object
            {
                pickupLocation = getDistance.WarehouseLocations, // Setting pickup locations in response
                latitude = getfarmLocation.latitude, // Setting farm latitude in response
                longitude = getfarmLocation.longitude // Setting farm longitude in response
            };

            return resp; // Returning the API response
        }

        [HttpGet("getWarehouses")]
        public async Task<APIResponse> GetWarehouses()
        {
            List<LocationDTO> warehouseLocationDBs = await _repoWrapper.LocationRepo.getWarehouseLocationsByLanguageCode(User.FindFirst("languageCode")?.Value); // Retrieving warehouse locations by language code
            if (warehouseLocationDBs == null || warehouseLocationDBs.Count() <= 0) // Checking if warehouse locations are found
                throw new AmazonFarmerException(_exceptions.warehouseNotFound); // Throws exception if warehouse locations are not found
            return new APIResponse()
            {
                isError = false,
                message = string.Empty,
                response = warehouseLocationDBs
            };
        }

        [AllowAnonymous]
        [HttpGet("getdirections/{origin}/{destination}")]
        public async Task<dynamic> getDirections(string origin, string destination)
        {
            return await _googleLocationExtension.GetDestination(origin, destination);
        }
    }
}
