using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using docs.Engine;
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
            var root = new DirectoryInfo(Directory.GetCurrentDirectory());
            ViewData["Title"] = "Backoffice";
            ViewData["RepoFolder"] = Engine.RepoFolder;
            ViewData["RepoURL"] = Engine.RepoURL;
            ViewData["Now"] = Engine.Now;
            ViewData["HasContent"] = Engine.HasContent;
            ViewData["Repos"] = root.GetDirectories();
            ViewData["RootDoc"] = Engine.RootDoc;

            return View("Backoffice");
        }

        [Route("Clone")]
        public async Task Clone()
        {
            Engine.Now = DateTime.Now;
            await Task.Factory.StartNew(() => LibGit2Sharp.Repository.Clone(Engine.RepoURL, Engine.RepoFolder));
            Response.Redirect("/Backoffice/");
        }

        [Route("Read")]
        public void Read()
        {
            string repoFolder = Engine.RepoFolder;
            var repo = new DirectoryInfo(repoFolder);
            Engine.RootDoc = new Document("Dokumentation", "docs");

            if (repoFolder.Equals("repo0"))
                Response.Redirect("/Backoffice/?err");

            // For all directories
            foreach (var d in repo.GetDirectories())
            {
                // Create a root document
                var slug = Regex.Replace(d.Name, "[^a-z0-9-]", "");
                var doc = new Document(d.Name, slug);
                var children = doc.Children;

                foreach (var f in repo.GetFiles())
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
