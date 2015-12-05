using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.IO;
using System.Text.RegularExpressions;

namespace aurora.Controllers
{
    [Route("/Backoffice/")]
    public class BackofficeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var dir = Directory.GetCurrentDirectory() + "/repos";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var root = new DirectoryInfo(dir);
            ViewData["Title"] = "Backoffice";
            ViewData["RepoFolder"] = Engine.RepoFolder;
            ViewData["RepoURL"] = Engine.RepoURL;
            ViewData["Now"] = Engine.Now;
            ViewData["HasContent"] = Engine.HasContent;
            ViewData["Repos"] = root.GetDirectories();
            ViewData["RootDoc"] = Engine.RootDoc;

            return View("Backoffice");
        }

        [Route("Update")]
        public async Task<ActionResult> Clone()
        {
            var repo = "repos/" + DateTime.Now.ToString("yyyyMMddhhmmss");
            await Task.Factory.StartNew(() => LibGit2Sharp.Repository.Clone(Engine.RepoURL, repo));

            var folder = new DirectoryInfo(repo);

            Engine.RootDoc = new Document("Dokumentation", "docs");

            // For all directories
            foreach (var d in folder.GetDirectories())
            {
                // Skip git folder
                if (d.Name.Equals(".git"))
                    continue;

                // Create a root document
                var slug = Regex.Replace(d.Name, "[^A-Za-z0-9-]", "").ToLower();
                var doc = new Document(d.Name, slug);
                var children = doc.Children;

                foreach (var f in d.GetFiles())
                {
                    var content = new StreamReader(f.FullName).ReadToEnd();

                    // Fill the root docuement
                    if (f.Name.ToLower().Equals("index.md"))
                    {
                        doc.Body = content;
                    }
                    // Or append it with sub-documents
                    else
                    {
                        doc.Children.Add(new Document(f.Name, slug, content));
                    }
                }

                Engine.RootDoc.Children.Add(doc);
            }

            return Redirect("/Backoffice/");
        }

        [Route("Read")]
        public void Read()
        {
            string repoFolder = Engine.RepoFolder;
            if (repoFolder.Equals("repo0"))
                Response.Redirect("/Backoffice/?err");

            var repo = new DirectoryInfo(repoFolder);
            Engine.RootDoc = new Document("Dokumentation", "docs");


            // For all directories
            foreach (var d in repo.GetDirectories())
            {
                // Create a root document
                var slug = Regex.Replace(d.Name, "[^A-Za-z0-9-]", "").ToLower();
                var doc = new Document(d.Name, slug);
                var children = doc.Children;

                foreach (var f in d.GetFiles())
                {
                    var content = new StreamReader(f.FullName).ReadToEnd();

                    // Fill the root docuement
                    if (f.Name.ToLower().Equals("index.md"))
                    {
                        doc.Body = content;
                    }
                    // Or append it with sub-documents
                    else
                    {
                        doc.Children.Add(new Document(f.Name, slug, content));
                    }
                }

                Engine.RootDoc.Children.Add(doc);
            }
            
            Response.Redirect("/Backoffice/");
        }
    }
}
