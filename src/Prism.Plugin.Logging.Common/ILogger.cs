using System;
using System.Collections.Generic;
using Prism.Logging;

namespace Prism.Logging
{
    public interface ILogger : ILoggerFacade
    {
        void Log(string message, IDictionary<string, string> properties);
        void Report(Exception ex, IDictionary<string, string> properties);
    }
}
