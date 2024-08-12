using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Administrator.API.Controllers
{
    [EnableCors("corsPolicy")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Admin/City")]
    public class CityController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public CityController(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }
        [HttpPost("getCities")]
        public async Task<APIResponse> GetCities(pagination_Req req)
        {
            APIResponse resp = new APIResponse();

            return resp;
        }
    }
}
