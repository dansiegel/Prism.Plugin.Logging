using System;
using Prism.Logging;
using Prism.Logging.Loggly;

namespace Prism.Ioc
{
    public static class LogglyRegistrationExtenions
    {
        public static IContainerRegistry RegisterLogglyHttpLogger<TOptions>(this IContainerRegistry container)
            where TOptions : ILogglyOptions
        {
            container.RegisterSingleton<ILogglyOptions, TOptions>();
            return RegisterInternal<LogglyHttpLogger>(container);
        }

        public static IContainerRegistry RegisterLogglyHttpLogger(this IContainerRegistry container, Action<LogglyOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Loggly Logger Options for the LogglyHttpLogger");

            var options = new LogglyOptions();
            configureOptions(options);

            return RegisterInternal<LogglyHttpLogger>(container, options);
        }

        public static IContainerRegistry RegisterLogglyHttpLogger(this IContainerRegistry container, string token, string appName, params string[] tags)
        {
            return RegisterInternal<LogglyHttpLogger>(container, new LogglyOptions {
                AppName = appName,
                Token = token,
                Tags = tags
            });
        }

        public static IContainerRegistry RegisterLogglySyslogLogger<TOptions>(this IContainerRegistry container)
            where TOptions : ILogglyOptions
        {
            container.RegisterSingleton<ILogglyOptions, TOptions>();
            return RegisterInternal<LogglySyslogLogger>(container);
        }

        public static IContainerRegistry RegisterLogglySyslogLogger(this IContainerRegistry container, Action<LogglyOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Loggly Logger Options for the LogglySyslogLogger");

            var options = new LogglyOptions();
            configureOptions(options);

            return RegisterInternal<LogglySyslogLogger>(container, options);
        }

        public static IContainerRegistry RegisterLogglySyslogLogger(this IContainerRegistry container, string token, string appName, params string[] tags)
        {
            return RegisterInternal<LogglySyslogLogger>(container, new LogglyOptions {
                AppName = appName,
                Token = token,
                Tags = tags
            });
        }

        private static IContainerRegistry RegisterInternal<T>(IContainerRegistry container, ILogglyOptions options = null)
            where T : IAggregableLogger
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.Token))
                {
                    Console.WriteLine("Loggly has been improperly configured. There is no token available");
                    return container.RegisterNullLogger();
                }

                container.RegisterInstance<ILogglyOptions>(options);
            }

            // TODO: Register ISyslogLogger when registering LogglySyslogLogger
            if (container.IsRegistered<IAggregateLogger>())
                return container.RegisterSingleton<IAggregableLogger, T>();

            return container.RegisterManySingleton<T>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
