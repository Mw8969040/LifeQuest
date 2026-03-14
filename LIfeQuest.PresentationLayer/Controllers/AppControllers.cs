using Microsoft.AspNetCore.Mvc;

namespace LIfeQuest.PresentationLayer.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    public class AnalyticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
