using Prism.Logging;

namespace Prism.Ioc
{
    public static class DefaultLoggingRegistrationExtensions
    {
        public static IContainerRegistry RegisterNullLogger(this IContainerRegistry containerRegistry)
        {
            // There's no reason to register the NullLogger if we already have a logger defined
            if (containerRegistry.IsRegistered<ILogger>())
                return containerRegistry;

            var logger = new NullLoggingService();
            return containerRegistry.RegisterInstance<ILogger>(logger)
                .RegisterInstance<IAnalyticsService>(logger)
                .RegisterInstance<ICrashesService>(logger);
        }

        public static IContainerRegistry RegisterConsoleLogger(this IContainerRegistry containerRegistry)
        {
            var logger = new ConsoleLoggingService();
            return containerRegistry.RegisterInstance<ILogger>(logger)
                .RegisterInstance<IAnalyticsService>(logger)
                .RegisterInstance<ICrashesService>(logger);
        }
    }
}
