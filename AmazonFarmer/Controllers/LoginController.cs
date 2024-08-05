using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Controllers
{
    public class LoginController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public LoginController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetObjectFromJson<UserDTO>("UserSession") == null)
            {
                ViewBag.error = "";
                return PartialView();
            }
            else
            {
                return LocalRedirect("/Dashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(loginReq req)
        {
            try
            {

                if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
                    throw new Exception(_exceptions.nullUsernameOrPassword);
                else
                {
                    UserDTO resp = await _repoWrapper.UserRepo.getUserByEmailndPassword(req);
                    if (resp != null)
                    {

                        HttpContext.Session.SetObjectAsJson("UserSession", resp);
                        return LocalRedirect("/Dashboard");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
            }
            return PartialView();
        }
    }
}
