using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace aurora.Controllers
{
    [Route("/Backoffice/")]
    public class GitController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("Backoffice");
        }

        [Route("Clone")]
        public IActionResult Clone()
        {
            LibGit2Sharp.Repository.Clone("https://github.com/datasektionen/Docs.git", "repo");

            return View("Backoffice");
        }
    }
}
