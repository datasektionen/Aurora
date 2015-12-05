using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.IO;

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

            // Get index.md, the root node of the documentation tree
            var index = new StreamReader(repo + "/index.md").ReadToEnd();
            index = CommonMark.CommonMarkConverter.Convert(index);

            // Flush the document tree, replacing it with the root node to build upon
            Data.RootDoc = new Document("docs", Util.GetTitle(index), index);

            // For all directories in the repository folder
            foreach (var d in folder.GetDirectories())
            {
                // Skip the .git folder
                if (d.Name.Equals(".git"))
                    continue;

                // Create a root document for this folder
                var slug = Util.Slugify(d.Name);
                var doc = new Document(slug);
                var children = doc.Children;

                // Parse through the documentation section's files
                foreach (var f in d.GetFiles())
                {
                    // Fetch file contents
                    var content = new StreamReader(f.FullName).ReadToEnd();

                    // Slugify the document file name, and convert body to HTML
                    slug = Util.Slugify(f.Name);
                    content = CommonMark.CommonMarkConverter.Convert(content);

                    // Grab the document's actual title
                    var title = Util.GetTitle(content);

                    // Fill the root docuement
                    if (f.Name.ToLower().Equals("index.md"))
                    {
                        doc.Body = content;
                        doc.Title = title;
                    }
                    // Or append it with sub-documents
                    else
                        doc.Children.Add(new Document(title, slug, content));
                }

                // Add folder/top level documents as children of the root document
                Data.RootDoc.Children.Add(doc);
            }

            // Once document refresh is done, display status page
            return Redirect("/Backoffice/");
        }
    }
}
