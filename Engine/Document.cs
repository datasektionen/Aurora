using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docs.Engine
{
    public class Document
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public Document(string Title, string Body)
        {

        }
    }
}
