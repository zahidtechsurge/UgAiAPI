using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Http;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infastructure.Persistence;
//using IBAGradsAdmin.CustomRoles.Permissions;
using System.Net.Http;
using AmazonFarmer.Infastructure;
using System.Net.Http.Json;
using AmazonFarmer.Core.Application.DTOs;
//using IBAGradsAdmin.CustomRoles.Sessions;
//using IBAGradsAdmin.CustomRoles.Extensions;
//using IBAGradsWebAPi.Domain.Entities.CustomRolesEntity;
using Newtonsoft.Json;
using System.Text;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infastructure.Persistence;

namespace Amazon_Farmer.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<TblUser> _userManager;
        private readonly SignInManager<TblUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly AmazonFarmerContext _context;

        public LoginModel(SignInManager<TblUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<TblUser> userManager,
            AmazonFarmerContext context)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // UserSession sess = new UserSession();

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var loggedInUser = await _userManager.FindByNameAsync(Input.UserName);
                    if (loggedInUser.Active == EActivityStatus.Active)
                    {
                        _logger.LogInformation("User logged in.");

                        //PermissionsChecker obj = new PermissionsChecker(_context);

                        //await obj.AccessToUser(loggedInUser.Id, HttpContext, Input.UserName, Input.Password);

                        //UserSession sess = (UserSession)HttpContext.Session.GetObjectFromJson<UserSession>("userObject");

                        //LoginDTO loginDTO = new()
                        //{
                        //    AppVersion = "1.0",
                        //    DeviceToken = "",
                        //    Email = Input.UserName,
                        //    Password = Input.Password
                        //};
                        var Currrentpage = "Chat/Index";
                        string isChatUser = "";// sess.PagePermissions.Where(p => p == Currrentpage).FirstOrDefault();
                        IList<string> roles = await _userManager.GetRolesAsync(loggedInUser);
                        if (isChatUser != null || roles.Contains("Admin"))
                        {
                            //try
                            //{
                            //    using (var client = new HttpClient())
                            //    {
                            //        client.BaseAddress = new Uri(ConfigSettings.GetWebApiURL());
                            //        // Serialize our concrete class into a JSON String
                            //        var stringPayload = JsonConvert.SerializeObject(loginDTO);

                            //        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                            //        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                            //        //HTTP POST 
                            //        var postTask = await client.PostAsync("/api/Authentication/SignIn", httpContent);

                            //        var result2 = postTask;
                            //        if (result2.IsSuccessStatusCode)
                            //        {
                            //            string content = await postTask.Content.ReadAsStringAsync();
                            //            var serializedReponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APILoginResponse>(content);
                            //            CookieOptions option = new CookieOptions();
                            //            option.Expires = DateTime.Now.AddDays(100);
                            //            sess.ApiSessionId = serializedReponse.result.token;
                            //            HttpContext.Session.SetObjectAsJson("userObject", sess);
                            //        }
                            //    }
                            //}
                            //catch (Exception)
                            //{
                            //    throw;
                            //}

                        }
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


    }
}
