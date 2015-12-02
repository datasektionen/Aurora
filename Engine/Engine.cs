using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docs.Engine
{
    public class Engine
    {
        private static Engine instance = null;
        private static string _repoFolder;
        private static DateTime _now;
        private static bool _hasContent = false;
        public static string RepoURL = "https://github.com/datasektionen/Docs.git";

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

        public Engine getInstance()
        {
            if (instance == null)
                instance = new Engine();

            return instance;
        }

    }
}
