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
                    slug = f.Name.
                        ToLower().
                        Replace(".md", "").
                        Replace(".mdown", "").
                        Replace(".markdown", "");
                    slug = Regex.Replace(slug, "[^A-Za-z0-9-]", "");

                    var content = new StreamReader(f.FullName).ReadToEnd();
                    content = CommonMark.CommonMarkConverter.Convert(content);

                    var start = content.IndexOf("<h1>");
                    var end = content.IndexOf("</h1>");
                    var title = content.Substring(start + 4, end - start);

                    // Fill the root docuement
                    if (f.Name.ToLower().Equals("index.md"))
                    {
                        doc.Body = content;
                        doc.Title = title;
                    }
                    // Or append it with sub-documents
                    else
                    {
                        doc.Children.Add(new Document(title, slug, content));
                    }
                }

                Engine.RootDoc.Children.Add(doc);
            }

            return Redirect("/Backoffice/");
        }
    }
}
