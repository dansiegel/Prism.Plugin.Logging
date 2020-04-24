using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Logging
{
    public class AggregateLogger : ILogger, IAggregateLogger
    {
        private List<ILogger> _loggers { get; }
        private bool _initialized;

        public AggregateLogger()
        {
            _loggers = new List<ILogger>
            {
                new ConsoleLoggingService()
            };
        }

        IReadOnlyList<ILogger> IAggregateLogger.Loggers => _loggers;

        void IAggregateLogger.AddLogger(ILogger logger)
        {
            EnsureInitialized();
            _loggers.Add(logger);
        }

        void IAggregateLogger.AddLoggers(params ILogger[] loggers)
        {
            EnsureInitialized();
            _loggers.AddRange(loggers);
        }

        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                _loggers.Clear();
                _initialized = true;
            }
        }

        private void InvokeOnLogger(Action<ILogger> logAction)
        {
            foreach (var logger in _loggers)
            {
                logAction(logger);
            }
        }

        void ILogger.Log(string message, IDictionary<string, string> properties) =>
            InvokeOnLogger(l => l.Log(message, new Dictionary<string, string>(properties)));

        void ILoggerFacade.Log(string message, Category category, Priority priority) =>
            InvokeOnLogger(l => l.Log(message, category, priority));

        void IAnalyticsService.TrackEvent(string name, IDictionary<string, string> properties) =>
            InvokeOnLogger(l => l.TrackEvent(name, new Dictionary<string, string>(properties)));

        void ICrashesService.Report(Exception ex, IDictionary<string, string> properties) =>
            InvokeOnLogger(l => l.Report(ex, new Dictionary<string, string>(properties)));
    }
}
