using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Logging
{
    public class NullLoggingService : ILogger
    {
        public void Log(string message, IDictionary<string, string> properties) { }

        public void Log(string message, Category category, Priority priority) { }

        public void Report(Exception ex, IDictionary<string, string> properties) { }

        public void TrackEvent(string name, IDictionary<string, string> properties) { }
    }
}
