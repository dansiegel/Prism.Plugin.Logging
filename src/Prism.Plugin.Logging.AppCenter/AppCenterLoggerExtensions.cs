using System;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Logging;
using Prism.Logging.AppCenter;

namespace Prism.Ioc
{
    public static class AppCenterLoggerExtensions
    {
        /// <summary>
        /// Registers the AppCenter Logger if AppCenter has been configured or if provided an appSecret
        /// </summary>
        /// <param name="container">The <see cref="IContainerRegistry"/>.</param>
        /// <param name="appSecret">Your App Center app secret. This could be multiplatform.</param>
        /// <param name="appCenterServices">Additional Services to register beyond Analytics &amp; Crash.</param>
        /// <returns>The <see cref="IContainerRegistry"/>.</returns>
        public static IContainerRegistry RegisterAppCenterLogger(this IContainerRegistry container, string appSecret = null, params Type[] appCenterServices)
        {
            if(!AppCenter.Configured)
            {
                var services = new List<Type>
                {
                    typeof(Analytics),
                    typeof(Crashes)
                };

                foreach(var service in appCenterServices)
                {
                    if (!services.Contains(service))
                        services.Add(service);
                }

                AppCenter.Start(appSecret, services.ToArray());
            }
            else if (string.IsNullOrEmpty(appSecret))
            {
                Console.WriteLine("The App Secret is null or empty. The Null Logging Service will be registered if no other logger has been registered.");
                return container.RegisterNullLogger();
            }

            if (container.IsRegistered<IAggregateLogger>())
                return container.RegisterSingleton<IAggregableLogger, AppCenterLogger>();

            return container.RegisterManySingleton<AppCenterLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
