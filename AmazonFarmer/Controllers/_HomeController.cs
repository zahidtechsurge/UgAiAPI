using AmazonFarmer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AmazonFarmer.Controllers
{
    public class _HomeController : Controller
    {
        private readonly ILogger<_HomeController> _logger;

        public _HomeController(ILogger<_HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}