using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aurora
{
    public class Engine
    {
        private static Engine instance = null;
        private static string _repoFolder;
        private static DateTime _now;
        private static bool _hasContent = false;
        public static string RepoURL = "https://github.com/datasektionen/Docs.git";
        public static Document RootDoc = new Document("Dokumentation", "docs");

        public static DateTime Now
        {
            get { return _now; }
            set
            {
                _now = value;
                _hasContent = true;
                _repoFolder = "repo" + value.ToBinary().ToString();
            }
        }

        public static bool HasContent { get { return _hasContent; } }

        public static string RepoFolder
        {
            get { return "repo" + _now.ToBinary().ToString(); }
        }
    }
}
