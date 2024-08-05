using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public RoleController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [AllowAnonymous]
        [HttpPost("getRoles")]
        public async Task<APIResponse> GetRoles(getRoles req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                resp.response = await _repoWrapper.RoleRepo.GetRoles(req.search, req.skip, req.take);
                
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return resp;
        }

        [AllowAnonymous]
        [HttpPost("getPermissionByRoleID")]
        public async Task<APIResponse> GetPermissionByRoleID(getPermission req)
        {
            APIResponse resp = new APIResponse();
            try
            {
                resp.response = await _repoWrapper.RoleRepo.getModules(req.roleID);
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
