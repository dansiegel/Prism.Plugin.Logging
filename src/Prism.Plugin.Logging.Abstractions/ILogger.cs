using System.Collections.Generic;

namespace Prism.Logging
{
    public interface ILogger : IAnalyticsService, ICrashesService
    {
        void Log(string message, IDictionary<string, string> properties);
    }
}
