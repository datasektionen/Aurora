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
        /// <summary>
        /// Display the back office dashboard, i.e. system status page
        /// </summary>
        public IActionResult Index()
        {
            // Our working directory is always ./repos
            var dir = Directory.GetCurrentDirectory() + "/repos";

            // Make sure the /repos folder exists
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Assign view data and render it
            ViewData["Title"] = "Backoffice";
            ViewData["RepoURL"] = Data.RepoURL;
            ViewData["Repos"] = new DirectoryInfo(dir).GetDirectories();
            ViewData["RootDoc"] = Data.RootDoc;

            return View("Backoffice");
        }

        /// <summary>
        /// Flush the current document tree (if existent), and run git clone to
        /// get the latest data. Parse it to a document tree with HTML content.
        /// </summary>
        [Route("Update")]
        public async Task<ActionResult> Clone()
        {
            // Repository folders are named after the current time
            var repo = "repos/" + DateTime.Now.ToString("yyyyMMddhhmmss");

            // Run a git clone as an asynchronous operation and list its contents
            await Task.Factory.StartNew(() => LibGit2Sharp.Repository.Clone(Data.RepoURL, repo));
            var folder = new DirectoryInfo(repo);

            // Once git clone is done, flush the document tree
            Data.RootDoc = new Document("Dokumentation", "docs");

            // For all directories in the repository folder
            foreach (var d in folder.GetDirectories())
            {
                // Skip the .git folder
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

                Data.RootDoc.Children.Add(doc);
            }

            return Redirect("/Backoffice/");
        }
    }
}
