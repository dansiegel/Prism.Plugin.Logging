using System;
using System.Collections.Generic;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppInsights;
using Prism.Logging.Internals;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppInsightsMicrosoftDependencyInjectionExtensions
    {
        public static IServiceCollection AddAppInsightsLogger<TOptions>(this IServiceCollection services)
            where TOptions : class, IApplicationInsightsOptions
        {
            services.AddSingleton<IApplicationInsightsOptions, TOptions>();
            return RegisterInternal(services);
        }

        public static IServiceCollection AddAppInsightsLogger(this IServiceCollection services, Action<ApplicationInsightsOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Application Insights Options for the AppInsightsLogger");

            var options = new ApplicationInsightsOptions {
                UserTraits = new Dictionary<string, string>()
            };
            configureOptions(options);

            return RegisterInternal(services, options);
        }

        public static IServiceCollection AddAppInsightsLogger(this IServiceCollection services, string instrumentationKey, IDictionary<string, string> userTraits = null) =>
            RegisterInternal(services, new ApplicationInsightsOptions {
                InstrumentationKey = instrumentationKey,
                UserTraits = userTraits
            });

        private static IServiceCollection RegisterInternal(this IServiceCollection services, ApplicationInsightsOptions options = null)
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.InstrumentationKey))
                {
                    Console.WriteLine("No Instrumentation Key provided");
                    return services.AddNullLogger();
                }

                services.AddSingleton<IApplicationInsightsOptions>(options);
            }

            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<IAggregableLogger, AppInsightsLogger>();

            return services.RegisterManySingleton<AppInsightsLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
