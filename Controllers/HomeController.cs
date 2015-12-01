using Microsoft.AspNet.Mvc;

namespace Docs.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("About");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
