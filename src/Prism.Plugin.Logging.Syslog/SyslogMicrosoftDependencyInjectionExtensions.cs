using System;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.Internals;
using Prism.Logging.Syslog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SyslogMicrosoftDependencyInjectionExtensions
    {
        public static IServiceCollection AddSyslogLogger(this IServiceCollection services)
        {
            return RegisterInternal(services, new SyslogOptions());
        }

        public static IServiceCollection AddSyslogLogger<TOptions>(this IServiceCollection services)
            where TOptions : class, ISyslogOptions
        {
            services.AddSingleton<ISyslogOptions, TOptions>();
            return RegisterInternal(services);
        }

        public static IServiceCollection AddSyslogLogger(this IServiceCollection services, Action<SyslogOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Syslog Logger Options for the SyslogLogger");

            var options = new SyslogOptions();
            configureOptions(options);

            return RegisterInternal(services, options);
        }

        public static IServiceCollection AddSyslogLogger(this IServiceCollection services, string hostOrIp, int port = 514, string appName = "PrismApp")
        {
            return RegisterInternal(services, new SyslogOptions {
                AppNameOrTag = appName,
                HostNameOrIp = hostOrIp,
                Port = port
            });
        }

        private static IServiceCollection RegisterInternal(IServiceCollection services, ISyslogOptions options = null)
        {
            if (options != null)
                services.AddSingleton<ISyslogOptions>(options);

            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<ISyslogLogger, SyslogLogger>()
                    .AddSingleton<IAggregableLogger>(sp => sp.GetRequiredService<ISyslogLogger>());

            return services.RegisterManySingleton<SyslogLogger>(
                typeof(ISyslogLogger),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
