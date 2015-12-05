using Microsoft.AspNet.Mvc;
using System.Linq;

namespace aurora.Controllers
{
    [Route("/")]
    public class DocsController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["Title"] = Data.RootDoc.Title;
            ViewData["Body"] = Data.RootDoc.Body;
            ViewData["Children"] = Data.RootDoc.Children;
            ViewData["IsRoot"] = true;
            ViewData["Slug"] = "/docs";

            return View("Document");
        }

        [Route("/docs/{section}")]
        public IActionResult Section(string section)
        {
            var doc = Data.RootDoc.Children.Where(d => d.Slug.Equals(section)).First();

            if (doc == null)
                return Redirect("/404");

            ViewData["Title"] = doc.Title;
            ViewData["Body"] = doc.Body;
            ViewData["Children"] = doc.Children;
            ViewData["IsRoot"] = false;
            ViewData["Slug"] = "/docs/" + doc.Slug;

            return View("Document");
        }

        [Route("/docs/{section}/{article}/")]
        public IActionResult Article(string section, string article)
        {
            var sec = Data.RootDoc.Children.Where(s => s.Slug.Equals(section)).First();
            if (sec == null)
                return Redirect("/404");

            var art = sec.Children.Where(d => d.Slug.Equals(article)).First();
            if (art == null)
                return Redirect("/404");

            ViewData["Title"] = art.Title;
            ViewData["Body"] = art.Body;
            ViewData["Children"] = sec.Children;
            ViewData["IsRoot"] = false;
            ViewData["Slug"] = "/docs/" + sec.Slug;

            return View("Document");
        }
    }
}
