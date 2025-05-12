using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System; // Added to use Exception class
using System.Threading.Tasks; // Added to use Task

namespace AmazonFarmerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        // Constructor to inject IRepositoryWrapper dependency
        public RoleController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // Action method to get roles
        [AllowAnonymous]
        [HttpPost("getRoles")]
        public async Task<APIResponse> GetRoles(getRoles req)
        {
            APIResponse resp = new APIResponse();
                // Call repository method to get roles
                resp.response = await _repoWrapper.RoleRepo.GetRoles(req.search, req.skip, req.take);
            return resp;
        }

        // Action method to get permissions by role ID
        [AllowAnonymous]
        [HttpPost("getPermissionByRoleID")]
        public async Task<APIResponse> GetPermissionByRoleID(getPermission req)
        {
            APIResponse resp = new APIResponse();
                // Call repository method to get permissions by role ID
                resp.response = await _repoWrapper.RoleRepo.getModules(req.roleID);
            return resp;
        }
    }
}
