namespace aurora
{
    public class Data
    {
        /// <summary>
        /// The git repo to clone.
        /// </summary>
        public static string RepoURL = "https://github.com/datasektionen/Docs.git";

        /// <summary>
        /// Root document container, carries the front page as well as
        /// pointers to child nodes.
        /// </summary>
        public static Document RootDoc = 
            new Document("docs", "Dokumentation", "Dokumentationen har inte lästs in ännu.");
    }
}
