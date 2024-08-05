using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Extensions;
using AmazonFarmer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

namespace AmazonFarmer.Controllers
{
    public class BaseController : Controller
    {
        UserDTO _user;
        SignInManager<TblUser> _SignInManager;

        public UserDTO currentUser
        {
            get
            {
                if (HttpContext.Session.GetObjectFromJson<UserDTO>("UserSession") != null)
                {
                    _user = HttpContext.Session.GetObjectFromJson<UserDTO>("UserSession");
                }
                return _user;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (HttpContext.Session.GetObjectFromJson<UserDTO>("UserSession") == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            else
            {
                var user = HttpContext.Session.GetObjectFromJson<UserDTO>("UserSession");

                var ControllerName = ControllerContext.ActionDescriptor.ControllerName;
                var actionName = ControllerContext.ActionDescriptor.ActionName;

                var Currrentpage = "/" + ControllerName + "/" + actionName;

                var jsonString = ((ClaimsIdentity)User.Identity).GetSpecificClaim("Modules");
                List<NavigationMenuDTO> menuItems = JsonSerializer.Deserialize<List<NavigationMenuDTO>>(jsonString);

                bool hasPermission = false;
                foreach (NavigationMenuDTO menuItem in menuItems)
                {
                    if (menuItem.Pages.Where(x => x.Controller == ControllerName && x.ActionMethod == actionName).Count() > 0)
                    {
                        hasPermission = true;
                    }
                }
                if (!hasPermission)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult(new { success = false, message = "You're not authorized to access this resource!" });
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/Login/Index");
                    }
                    //filterContext.Result = new RedirectResult("/Login/Index");
                }
                

            }

        }
    }

    public static class HttpRequestExtensions
    {
        // Extension method to check if the request is an AJAX request
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }


}
