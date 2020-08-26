using System.Collections.Generic;
using System.Linq;

namespace Prism.Logging
{
    public interface IAggregateLogger
    {
        IReadOnlyList<ILogger> Loggers { get; }

        void AddLogger(ILogger logger);

        void AddLoggers(params ILogger[] loggers);
    }

    public static class IAggregateLoggerExtensions
    {
        public static void AddLoggers(this IAggregateLogger logger, IEnumerable<ILogger> loggers) =>
            logger.AddLoggers(loggers.ToArray());
    }
}