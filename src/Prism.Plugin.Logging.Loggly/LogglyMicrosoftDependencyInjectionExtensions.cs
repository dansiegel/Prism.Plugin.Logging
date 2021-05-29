using System;
using System.Collections.Generic;
using System.Text;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.Internals;
using Prism.Logging.Loggly;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogglyMicrosoftDependencyInjectionExtensions
    {
        public static IServiceCollection RegisterLogglyHttpLogger<TOptions>(this IServiceCollection services)
    where TOptions : class, ILogglyOptions
        {
            services.AddSingleton<ILogglyOptions, TOptions>();
            return RegisterInternal<LogglyHttpLogger>(services);
        }

        public static IServiceCollection RegisterLogglyHttpLogger(this IServiceCollection services, Action<LogglyOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Loggly Logger Options for the LogglyHttpLogger");

            var options = new LogglyOptions();
            configureOptions(options);

            return RegisterInternal<LogglyHttpLogger>(services, options);
        }

        public static IServiceCollection AddLogglyHttpLogger(this IServiceCollection services, string token, string appName, params string[] tags)
        {
            return RegisterInternal<LogglyHttpLogger>(services, new LogglyOptions {
                AppName = appName,
                Token = token,
                Tags = tags
            });
        }

        public static IServiceCollection AddLogglySyslogLogger<TOptions>(this IServiceCollection services)
            where TOptions : class, ILogglyOptions
        {
            services.AddSingleton<ILogglyOptions, TOptions>();
            return RegisterInternal<LogglySyslogLogger>(services);
        }

        public static IServiceCollection RegisterLogglySyslogLogger(this IServiceCollection services, Action<LogglyOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Loggly Logger Options for the LogglySyslogLogger");

            var options = new LogglyOptions();
            configureOptions(options);

            return RegisterInternal<LogglySyslogLogger>(services, options);
        }

        public static IServiceCollection AddLogglySyslogLogger(this IServiceCollection services, string token, string appName, params string[] tags)
        {
            return RegisterInternal<LogglySyslogLogger>(services, new LogglyOptions {
                AppName = appName,
                Token = token,
                Tags = tags
            });
        }

        private static IServiceCollection RegisterInternal<T>(IServiceCollection services, ILogglyOptions options = null)
            where T : class, IAggregableLogger
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.Token))
                {
                    Console.WriteLine("Loggly has been improperly configured. There is no token available");
                    return services.AddNullLogger();
                }

                services.AddSingleton<ILogglyOptions>(options);
            }

            // TODO: Register ISyslogLogger when registering LogglySyslogLogger
            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<IAggregableLogger, T>();

            return services.RegisterManySingleton<T>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
