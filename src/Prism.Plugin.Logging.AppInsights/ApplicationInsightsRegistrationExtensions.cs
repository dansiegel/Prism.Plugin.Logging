using System;
using System.Collections.Generic;
using Prism.Logging;
using Prism.Logging.AppInsights;

namespace Prism.Ioc
{
    public static class ApplicationInsightsRegistrationExtensions
    {
        public static IContainerRegistry RegisterAppInsightsLogger<TOptions>(this IContainerRegistry container)
            where TOptions : IApplicationInsightsOptions
        {
            container.RegisterSingleton<IApplicationInsightsOptions, TOptions>();
            return RegisterInternal(container);
        }

        public static IContainerRegistry RegisterAppInsightsLogger(this IContainerRegistry container, Action<ApplicationInsightsOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Application Insights Options for the AppInsightsLogger");

            var options = new ApplicationInsightsOptions {
                UserTraits = new Dictionary<string, string>()
            };
            configureOptions(options);

            return RegisterInternal(container, options);
        }

        public static IContainerRegistry RegisterAppInsightsLogger(this IContainerRegistry container, string instrumentationKey, IDictionary<string, string> userTraits = null) =>
            RegisterInternal(container, new ApplicationInsightsOptions {
                InstrumentationKey = instrumentationKey,
                UserTraits = userTraits
            });

        private static IContainerRegistry RegisterInternal(this IContainerRegistry container, ApplicationInsightsOptions options = null)
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.InstrumentationKey))
                {
                    Console.WriteLine("No Instrumentation Key provided");
                    return container.RegisterNullLogger();
                }

                container.RegisterInstance<IApplicationInsightsOptions>(options);
            }

            if (container.IsRegistered<IAggregateLogger>())
                return container.RegisterSingleton<IAggregableLogger, AppInsightsLogger>();

            return container.RegisterManySingleton<AppInsightsLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
