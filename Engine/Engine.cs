using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docs.Engine
{
    public class Engine
    {
        private static Engine instance = null;

        public Engine getInstance()
        {
            if (instance == null)
                instance = new Engine();

            return instance;
        }
    }
}
