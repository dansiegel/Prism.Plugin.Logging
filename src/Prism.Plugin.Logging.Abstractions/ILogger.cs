using System;
using System.Collections.Generic;
using Prism.Logging;

namespace Prism.Logging
{
    public interface ILogger : ILoggerFacade, IAnalyticsService, ICrashesService
    {
        void Log(string message, IDictionary<string, string> properties);
    }
}
