using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DAPM_C_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}

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
