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
            // Populate ViewData and render
            ViewData["Title"] = Data.RootDoc.Title;
            ViewData["Body"] = Data.RootDoc.Body;
            ViewData["Children"] = Data.RootDoc.Children;
            ViewData["ParentTitle"] = Data.RootDoc.Title;
            ViewData["Slug"] = Data.RootDoc.Slug;
            ViewData["IsRoot"] = true;
            ViewData["Path"] = "/docs";

            return View("Document");
        }

        [Route("/docs/{section}")]
        public IActionResult Section(string section)
        {
            // Try to fetch document with the desired slug, /404 otherwise
            var getDoc = Data.RootDoc.Children.Where(d => d.Slug.Equals(section));
            if (getDoc.Count() != 1)
                return Redirect("/404");
            var doc = getDoc.First();

            // Populate ViewData and render
            ViewData["Title"] = doc.Title;
            ViewData["Body"] = doc.Body;
            ViewData["Children"] = doc.Children;
            ViewData["ParentTitle"] = doc.Title;
            ViewData["Slug"] = doc.Slug;
            ViewData["IsRoot"] = false;
            ViewData["Path"] = "/docs/" + doc.Slug;

            return View("Document");
        }

        [Route("/docs/{section}/{article}/")]
        public IActionResult Article(string section, string article)
        {
            // Try to fetch section with the desired slug, /404 otherwise
            var getSec = Data.RootDoc.Children.Where(s => s.Slug.Equals(section));
            if (getSec.Count() != 1)
                return Redirect("/404");
            var sec = getSec.First();

            // Try to fetch article with the desired slug, /404 otherwise
            var getArt = sec.Children.Where(d => d.Slug.Equals(article));
            if (getArt.Count() != 1)
                return Redirect("/404");
            var art = getArt.First();

            // Populate ViewData and render
            ViewData["Title"] = art.Title;
            ViewData["Body"] = art.Body;
            ViewData["Children"] = sec.Children;
            ViewData["ParentTitle"] = sec.Title;
            ViewData["Slug"] = art.Slug;
            ViewData["IsRoot"] = false;
            ViewData["Path"] = "/docs/" + sec.Slug;

            return View("Document");
        }
    }
}
