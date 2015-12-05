using System.Text;
using System.Text.RegularExpressions;

namespace aurora
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

        /// <summary>
        /// Get the contents of the first h1 HTML element in a string
        /// </summary>
        public static string GetTitle(string body)
        {
            // Grab the positions of the first h1 element
            var start = body.IndexOf("<h1>");
            var end = body.IndexOf("</h1>");

            // If either tag is not found, the document is untitled
            if (start < 0 || end < 0)
                return "Namnlöst dokument";

            // Skip <h1> tag, calculate length of title
            var startIndex = start + 4;
            var endIndex = end - start - 4;

            // Return the document title
            return body.Substring(startIndex, endIndex);
        }
    }
}
