using System.Text;
using System.Text.RegularExpressions;

namespace docs.Engine
{
    public static class Util
    {
        /// <summary>
        /// Converts a text string to a URL slug
        /// </summary>
        public static string Slugify(string raw)
        {
            // Convert foreign accent characters
            raw = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(raw));

            // Lower-case slug and remove .md extension (if existent)
            raw = raw.ToLower().Replace(".md", "");

            // Filter out everything that is not an allowed character
            return Regex.Replace(raw, "[^A-Za-z0-9-]", "");
        }


    }
}
