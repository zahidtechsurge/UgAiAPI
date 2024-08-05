using AmazonFarmer.Core.Application;
using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Exceptions;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Controllers
{
    public class RegisterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public RegisterController(IRepositoryWrapper repoWrapper)
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
        public async Task<IActionResult> Index(addUserDTO req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.FirstName))
                    throw new Exception(_exceptions.firstnameRequired);
                else if (string.IsNullOrEmpty(req.Phone))
                    throw new Exception(_exceptions.phoneRequired);
                else if (string.IsNullOrEmpty(req.Email))
                    throw new Exception(_exceptions.emailRequired);
                else if (string.IsNullOrEmpty(req.Password))
                    throw new Exception(_exceptions.passwordRequired);
                else if (string.IsNullOrEmpty(req.ConfirmPassword))
                    throw new Exception(_exceptions.confirmPasswordRequired);
                else if (req.ConfirmPassword != req.Password)
                    throw new Exception(_exceptions.confirmPasswordNotMatch);
                else
                {
                    //farmer settings
                    req.DesignationID = (int)EDesignation.Farmer;
                    req.RoleID = "bfa863ab-934d-49ba-9200-988c88353728";
                    await _repoWrapper.UserRepo.addUser(req);

                    return LocalRedirect("/Login");
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
