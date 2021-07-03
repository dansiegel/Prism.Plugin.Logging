using System;
using System.Collections.Generic;
using Prism.Logging;

namespace Prism.Plugin.Logging.Mocks
{
    public class LoggerMock : ILogger
    {
        public LogMessageMock Sent { get; private set; }

        public void Log(string message, IDictionary<string, string> properties)
        {
            Sent = new LogMessageMock {
                Message = message,
                Properties = properties
            };
        }

        public void Report(Exception ex, IDictionary<string, string> properties)
        {
            Sent = new LogMessageMock {
                Exception = ex,
                Properties = properties
            };
        }

        public void TrackEvent(string name, IDictionary<string, string> properties)
        {
            Sent = new LogMessageMock {
                Message = name,
                Properties = properties
            };
        }
    }
}
