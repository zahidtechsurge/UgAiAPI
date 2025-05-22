using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
//using AmazonFarmer.Infrastructure.Persistence.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper; // Constructor injection of IRepositoryWrapper.
        public ConfigurationController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        /// <summary>
        /// Retrieves a paginated, sorted, and searchable list of configuration records.
        /// </summary>
        /// <param name="Request">
        /// A <see cref="ReportPagination_Req"/> object containing pagination, sorting, and search parameters.
        /// </param>
        /// <returns>
        /// An <see cref="APIResponse"/> containing a paginated list of configuration records, total count, and filtered count.
        /// </returns>
        /// <remarks>
        /// This endpoint supports:
        /// - Search by ID, Name, Description, or Value (case-insensitive).
        /// - Sorting by fields such as configurationName, configurationID, etc.
        /// - Pagination using page number and page size.
        /// </remarks>
        /// <response code="200">Returns paginated, filtered, and sorted configuration records.</response>

        [HttpPost("getConfiguration")]
        public async Task<APIResponse> GetConfiguration(ReportPagination_Req Request)
        {
            var resp = new APIResponse(); // Main response wrapper
            var InResp = new pagination_Resp(); // Paginated response data

            // Fetch all configuration records by config type
            List<tblConfig> tblconfig = await _repoWrapper.CommonRepo.GetConfigurationValueByConfigType();

            // Apply search filter if a search term is provided
            if (!string.IsNullOrEmpty(Request.search))
            {
                tblconfig = tblconfig.Where(x =>
                    x.Id.ToString().Contains(Request.search) ||
                    x.Name.ToLower().Contains(Request.search.ToLower()) ||
                    x.Description.ToLower().Contains(Request.search.ToLower()) ||
                    x.Value.ToLower().Contains(Request.search.ToLower())
                ).ToList();
            }

            // Apply sorting based on the request
            tblconfig = SortConfigurations(tblconfig, Request.sortColumn, Request.sortOrder);

            // Set total records before pagination
            InResp.totalRecord = tblconfig.Count();

            // Apply pagination using Skip and Take
            tblconfig = tblconfig.Skip(Request.pageNumber * Request.pageSize)
                         .Take(Request.pageSize).ToList();

            // Set filtered records after pagination
            InResp.filteredRecord = tblconfig.Count();
            
            // Map tblConfig to the DTO or anonymous object for output
            InResp.list = tblconfig
                .Select(c => new
                {
                    id = c.Id,
                    value = c.Value,
                    description = c.Description,
                    name = c.Name,
                    statusID = (int)c.Status
                })
                .ToList();

            // Attach data to the response wrapper
            resp.response = InResp;

            return resp;
        }

        /// <summary>
        /// Sorts a list of <see cref="tblConfig"/> records based on a specified column and order.
        /// Defaults to sorting by <c>Id</c> in descending order if no valid column is provided.
        /// </summary>
        /// <param name="tblConfigList">The list of configuration records to sort.</param>
        /// <param name="sortColumn">The name of the column to sort by (e.g., "configurationName").</param>
        /// <param name="sortOrder">The direction of the sort ("ASC" or "DESC").</param>
        /// <returns>A sorted list of <see cref="tblConfig"/> records.</returns>
        private List<tblConfig> SortConfigurations(List<tblConfig> tblConfigList, string sortColumn, string sortOrder)
        {
            // If no sort column is provided, return default sort by Id descending
            if (string.IsNullOrWhiteSpace(sortColumn))
                return tblConfigList.OrderByDescending(x => x.Id).ToList();

            // Mapping of sortable fields to their corresponding lambda selectors
            var sortMap = new Dictionary<string, Func<tblConfig, object>>(StringComparer.OrdinalIgnoreCase)
            {
                { "configurationID", x => x.Id },
                { "configurationName", x => x.Name },
                { "configurationDesc", x => x.Description },
                { "configurationValue", x => x.Value },
                { "status", x => x.Status }
            };

            // If the requested sortColumn exists in the map, perform sorting accordingly
            if (sortMap.TryGetValue(sortColumn, out var keySelector))
            {
                // Check sort order and apply appropriate sorting
                if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                {
                    return tblConfigList.OrderBy(keySelector).ToList();
                }
                else
                {
                    return tblConfigList.OrderByDescending(keySelector).ToList();
                }
            }

            // If sortColumn is not recognized, fall back to default sort by Id descending
            return tblConfigList.OrderByDescending(x => x.Id).ToList();
        }



        //[Authorize]
        /// <summary>
        /// Updates an existing configuration record by its ID with the provided values.
        /// </summary>
        /// <param name="Request">An <see cref="UpdateConfigRequest"/> object containing the updated configuration values.</param>
        /// <returns>
        /// An <see cref="APIResponse"/> object indicating the success or failure of the operation.
        /// If the configuration is not found, the response will contain an error message.
        /// </returns>
        /// <response code="200">Returns a success response if the update is completed.</response>
        /// <response code="404">Returns an error if the configuration record is not found.</response>
        /// <remarks>
        /// This endpoint updates the following fields: Name, Value, Description, and Status.
        /// </remarks>
        /// <example>
        /// PUT /api/configuration/updateConfiguration
        /// {
        ///   "configID": 5,
        ///   "name": "SiteTitle",
        ///   "value": "My Website",
        ///   "description": "Title shown on the browser tab",
        ///   "status": 1
        /// }
        /// </example>
        [HttpPut("updateConfiguration")]
        public async Task<APIResponse> UpdateConfiguration(UpdateConfigRequest Request)
        {
            // Create a response object to encapsulate the result
            APIResponse response = new APIResponse();

            // Retrieve the configuration entry by ID
            tblConfig? Config = await _repoWrapper.CommonRepo.GetConfigurationByID(Request.configID);

            // If configuration not found, return error response
            if (Config == null)
            {
                response.isError = true;
                response.message = "Configuration not found";
                return response;
            }

            // Update fields of the configuration entity
            Config.Name = Request.name;
            Config.Value = Request.value;
            Config.Description = Request.description;
            Config.Status = (EConfigStatus)Request.status;

            // Mark the entity as updated in the repository
            _repoWrapper.CommonRepo.UpdateConfigurationValue(Config);

            // Persist the changes to the database
            await _repoWrapper.SaveAsync();

            // Return a success message
            response.message = "Record updated";
            return response;
        }

    }
}
