using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Prism.Logging.Internals;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppCenterMicrosoftDependencyInjectionExtensions
    {
        /// <summary>
        /// Registers the AppCenter Logger if AppCenter has been configured or if provided an appSecret
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="appSecret">Your App Center app secret. This could be multiplatform.</param>
        /// <param name="appCenterServices">Additional Services to register beyond Analytics &amp; Crash.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAppCenterLogger(this IServiceCollection services, string appSecret = null, params Type[] appCenterServices)
        {
            if (!AppCenter.AppCenter.Configured)
            {
                var servicesToAdd = new List<Type>
                {
                    typeof(Analytics),
                    typeof(Crashes)
                };

                foreach (var service in appCenterServices)
                {
                    if (!servicesToAdd.Contains(service))
                        servicesToAdd.Add(service);
                }

                AppCenter.AppCenter.Start(appSecret, servicesToAdd.ToArray());
            }
            else if (string.IsNullOrEmpty(appSecret))
            {
                Console.WriteLine("The App Secret is null or empty. The Null Logging Service will be registered if no other logger has been registered.");
                return services.AddNullLogger();
            }

            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<IAggregableLogger, AppCenterLogger>();

            return services.RegisterManySingleton<AppCenterLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
