using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using docs.Engine;
using Microsoft.AspNet.Http;
using System.IO;

namespace aurora.Controllers
{
    [Route("/Backoffice/")]
    public class GitController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var repos = new List<DirectoryInfo>();
            var root = new DirectoryInfo(Directory.GetCurrentDirectory());
            ViewData["Title"] = "Backoffice";
            ViewData["RepoFolder"] = Engine.RepoFolder;
            ViewData["RepoURL"] = Engine.RepoURL;
            ViewData["Now"] = Engine.Now;
            ViewData["HasContent"] = Engine.HasContent;

            foreach (DirectoryInfo dir in root.GetDirectories())
                repos.Add(dir);

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
    }
}
