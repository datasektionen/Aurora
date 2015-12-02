using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using docs.Engine;
using System.IO;

namespace aurora.Controllers
{
    [Route("/Backoffice/")]
    public class BackofficeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var root = new DirectoryInfo(Directory.GetCurrentDirectory());
            ViewData["Title"] = "Backoffice";
            ViewData["RepoFolder"] = Engine.RepoFolder;
            ViewData["RepoURL"] = Engine.RepoURL;
            ViewData["Now"] = Engine.Now;
            ViewData["HasContent"] = Engine.HasContent;
            ViewData["Repos"] = root.GetDirectories();

            return View("Backoffice");
        }

        [Route("Clone")]
        public async Task Clone()
        {
            Engine.Now = DateTime.Now;
            await Task.Factory.StartNew(() => LibGit2Sharp.Repository.Clone(Engine.RepoURL, Engine.RepoFolder));
            Response.Redirect("/Backoffice/");
        }

        [Route("Parse")]
        public IActionResult Parse()
        {
            string repoFolder = Engine.RepoFolder;
            var repo = new DirectoryInfo(repoFolder);

            // For all directories
            foreach (var d in repo.GetDirectories())
            {
                foreach (var f in repo.GetFiles())
                {
                    var content = new StreamReader(f.FullName).ReadToEnd();
                }
            }

            return View();
        }
    }
}
