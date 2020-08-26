using System;
using System.Collections.Generic;

namespace Prism.Plugin.Logging.Mocks
{
    public class LogMessageMock
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}
