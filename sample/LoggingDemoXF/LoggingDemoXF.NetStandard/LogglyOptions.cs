using System;
using System.Collections.Generic;
using System.Text;
using Prism.Logging.Loggly;

namespace LoggingDemoXF
{
    public class LogglyOptions : ILogglyOptions
    {
        public string Token { get; set; }

        public string AppName { get; set; }

        public IEnumerable<string> Tags => new string[]
        {
            "http",
            "test"
        };
    }
}
