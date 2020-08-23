using System;
using System.Collections.Generic;
using System.Text;
using Prism.Logging;
using Prism.Logging.Graylog;

namespace Prism.Ioc
{
    public static class GraylogRegistrationExtensions
    {
        public static IContainerRegistry RegisterGraylogLogger<TOptions>(this IContainerRegistry containerRegistry)
            where TOptions : IGelfOptions
        {
            containerRegistry.RegisterSingleton<IGelfOptions, TOptions>();
            return containerRegistry.RegisterSingleton<ILogger, GelfLogger>()
                .RegisterSingleton<IAnalyticsService, GelfLogger>()
                .RegisterSingleton<ICrashesService>();
        }

        public static IContainerRegistry RegisterGraylogLogger(this IContainerRegistry container, Action<GelfOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Graylog Logger Options for the GelfLogger");

            var options = new GelfOptions();
            configureOptions(options);

            return RegisterInternal(container, options);
        }

        public static IContainerRegistry RegisterSocketLogger(this IContainerRegistry container, Uri host) =>
            RegisterInternal(container, new GelfOptions
            {
                Host = host
            });

        private static IContainerRegistry RegisterInternal(this IContainerRegistry container, GelfOptions options = null)
        {
            if (options != null)
            {
                if (options.Host is null || !options.Host.IsAbsoluteUri)
                {
                    Console.WriteLine("No Host or invalid relative Uri was detected");
                    return container.RegisterNullLogger();
                }

                container.RegisterInstance<IGelfOptions>(options);
            }

            var instance = ((IContainerProvider)container).Resolve<GelfLogger>();
            container.RegisterInstance<ILogger>(instance)
                .RegisterInstance<ILoggerFacade>(instance)
                .RegisterInstance<IAnalyticsService>(instance)
                .RegisterInstance<ICrashesService>(instance);

            return container;
        }
    }
}
