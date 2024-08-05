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

        [HttpPost("getPickup")]
        public async Task<APIResponse> getPickup(currentLocation req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                List<LocationDTO> warehouseLocationDBs = await _repoWrapper.LocationRepo.getWarehouseLocationsByLanguageCode(User.FindFirst("languageCode")?.Value);
                if (warehouseLocationDBs == null || warehouseLocationDBs.Count() <= 0)
                    throw new Exception(_exceptions.warehouseNotFound);

                getFarmLocation getfarmLocation = await _repoWrapper.FarmRepo.getfarmLocationByFarmID(req.farmID);
                if (getfarmLocation == null)
                    throw new Exception(_exceptions.farmNotFound);

                getDistance getDistance = new getDistance
                {
                    farmLatitude = getfarmLocation.latitude,
                    farmLongitude = getfarmLocation.longitude,
                    WarehouseLocations = new List<WarehouseLocation>()
                };

                List<WarehouseLocation> warehouseLocations = new();

                foreach (var location in warehouseLocationDBs)
                {
                    WarehouseLocation warehouseLocation = new() { 
                        lat = location.latitude,
                        lng = location.longitude
                    };
                    getDistance.WarehouseLocations.Add(warehouseLocation);
                }

                var distance = await _googleLocationExtension.GetDistanceBetweenLocations(getDistance);

                //location.distance = distance;
                warehouseLocationDBs = warehouseLocationDBs.OrderBy(l => l.distance).ToList();

                resp.response = new getPickupLocation_Resp()
                {
                    pickupLocation = warehouseLocationDBs,
                    latitude = getfarmLocation.latitude,
                    longitude = getfarmLocation.longitude
                };

            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }
    }
}
