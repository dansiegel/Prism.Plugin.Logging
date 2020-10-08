using System.Collections.Generic;
using Prism.Logging;

namespace Prism.Ioc
{
    public static class DefaultLoggingRegistrationExtensions
    {
        public static IContainerRegistry UseAggregateLogger(this IContainerRegistry container)
        {
            return container.RegisterSingleton<AggregateLogger>(c =>
            {
                var logger = new AggregateLogger();
                var aggregate = (IAggregateLogger)logger;
                aggregate.AddLoggers(c.Resolve<IEnumerable<IAggregableLogger>>());
                return logger;
            })
                .Register<IAggregateLogger>(c => c.Resolve<AggregateLogger>())
                .Register<ILogger>(c => c.Resolve<AggregateLogger>())
                .Register<IAnalyticsService>(c => c.Resolve<AggregateLogger>())
                .Register<ICrashesService>(c => c.Resolve<AggregateLogger>());
        }

        public static IContainerRegistry RegisterNullLogger(this IContainerRegistry container)
        {
            // There's no reason to register the NullLogger if we already have a logger defined
            if (container.IsRegistered<ILogger>())
                return container;

            return container.RegisterManySingleton<NullLoggingService>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }

        public static IContainerRegistry RegisterConsoleLogger(this IContainerRegistry container)
        {
            if (container.IsRegistered<IAggregateLogger>())
                return container.RegisterSingleton<IAggregableLogger, ConsoleLoggingService>();

            return container.RegisterManySingleton<ConsoleLoggingService>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
