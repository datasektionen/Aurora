using System.Collections.Generic;

namespace aurora
{
    public class Document
    {
        public string Slug { get; private set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Document> Children { get; private set; }

        public Document(string Title, string Slug)
        {
            this.Title = Title;
            this.Slug = Slug;
            Children = new List<Document>();
        }

        public Document(string Title, string Slug, string Body)
        {
            this.Title = Title;
            this.Body = Body;
            this.Slug = Slug;
            Children = new List<Document>();
        }
    }
}
