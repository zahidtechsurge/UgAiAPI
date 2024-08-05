using Microsoft.AspNetCore.Mvc;

namespace AmazonFarmer.Controllers
{
    public class DashboardController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
