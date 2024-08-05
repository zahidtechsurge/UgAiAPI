using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TehsilController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public TehsilController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [AllowAnonymous]
        [HttpPost("getTehsils")]
        public async Task<APIResponse> getTehsils(getTehsil_Req req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                if (string.IsNullOrEmpty(req.languageCode))
                    throw new Exception(_exceptions.languageCodeRequired);

                resp.response = await _repoWrapper.TehsilRepo.getTehsils(req);
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
