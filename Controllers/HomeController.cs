using Microsoft.AspNet.Mvc;

namespace aurora.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View("About");
        }

        [Route("/about")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
    }
}
