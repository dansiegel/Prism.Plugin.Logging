using System;
using System.Collections.Generic;
using System.Text;
using Prism.Logging.Logger;

namespace Prism.Logging.TestsHelpers
{
    public class NullStorage:IPlatformStringStorage
    {
        public string Read()
        {
            return String.Empty;
        }

        public bool Write(string data)
        {
            return true;
        }
    }
}
