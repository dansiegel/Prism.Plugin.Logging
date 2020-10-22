using System;
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
            return RegisterInternal(containerRegistry);
        }

        public static IContainerRegistry RegisterGraylogLogger(this IContainerRegistry container, Action<GelfOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Graylog Logger Options for the GelfLogger");

            var options = new GelfOptions();
            configureOptions(options);

            return RegisterInternal(container, options);
        }

        public static IContainerRegistry RegisterGraylogLogger(this IContainerRegistry container, Uri host) =>
            RegisterInternal(container, new GelfOptions {
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

            if (container.IsRegistered<IAggregateLogger>())
                return container.RegisterSingleton<IGelfLogger, GelfLogger>()
                    .Register<IAggregableLogger>(c => c.Resolve<IGelfLogger>());

            return container.RegisterManySingleton<GelfLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
