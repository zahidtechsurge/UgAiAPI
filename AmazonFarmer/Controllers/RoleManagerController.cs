using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Controllers
{
    public class RoleManagerController : BaseController
    {

        private IRepositoryWrapper _repoWrapper;
        private readonly RoleManager<TblRole> _roleManager;
        public RoleManagerController(IRepositoryWrapper repoWrapper, RoleManager<TblRole> roleManager)
        {
            _repoWrapper = repoWrapper;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> getRoles()
        {
            // Initialization.
            int start = 0, length = 0, draw = 0, recordsTotal = 0, recordsFiltered = 0;
            string search = "";
            List<TblRole> roles = new List<TblRole>();
            try
            {
                start = Convert.ToInt32(Request.Form["start"].ToString());
                length = Convert.ToInt32(Request.Form["length"].ToString());
                search = Request.Form["search[value]"].ToString();
                draw = Convert.ToInt32(Request.Form["draw"].ToString());

                roles = null;// await _repoWrapper.RoleRepo.GetRoles(search, start, length);

                recordsTotal = roles.Count;
                recordsFiltered = roles.Count;
            }
            catch (Exception)
            { }

            return Json(new
            {
                draw = Convert.ToInt32(draw + 1),
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = roles.ToList()
            });
        }

        public IActionResult PermissionManager(string RoleID)
        {
            return View();
        }
        public async Task<IActionResult> getPermissionsByRoleID(string RoleID)
        {
            return Json(await _repoWrapper.RoleRepo.getModules(RoleID));
        }
        public async Task<JsonResult> updatePermissions(PermissionDTO req)
        {
            JSONResponse resp = new JSONResponse();
            try
            {
                var role = await _roleManager.FindByIdAsync(req.RoleId);
                var claims = await _roleManager.GetClaimsAsync(role);
                
                //removing previous claims
                foreach (var item in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, item);
                }

                foreach (var item in req.Permissions)
                {
                    var selectedClaims = item.ModuleClaims.Where(x => x.Selected).ToList();
                    foreach (var claim in selectedClaims)
                    {
                        await _roleManager.AddPermissionClaim(role, claim.Value);
                    }
                }
                resp.message = "permissions updated";
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.message = ex.Message;
            }
            return Json(new
            {
                resp
            });
        }


    }
}
