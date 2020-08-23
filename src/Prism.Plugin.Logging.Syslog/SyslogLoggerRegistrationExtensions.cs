using System;
using Prism.Logging;
using Prism.Logging.Syslog;

namespace Prism.Ioc
{
    public static class SyslogLoggerRegistrationExtensions
    {
        public static IContainerRegistry RegisterSyslogLogger<TOptions>(this IContainerRegistry container)
            where TOptions : ISyslogOptions
        {
            container.RegisterSingleton<ISyslogOptions, TOptions>();
            return RegisterInternal(container);
        }

        public static IContainerRegistry RegisterSyslogLogger(this IContainerRegistry container, Action<SyslogOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Syslog Logger Options for the SyslogLogger");

            var options = new SyslogOptions();
            configureOptions(options);

            return RegisterInternal(container, options);
        }

        public static IContainerRegistry RegisterSyslogLogger(this IContainerRegistry container, string hostOrIp, int port = 514, string appName = "PrismApp")
        {
            return RegisterInternal(container, new SyslogOptions
            {
                AppNameOrTag = appName,
                HostNameOrIp = hostOrIp,
                Port = port
            });
        }

        private static IContainerRegistry RegisterInternal(IContainerRegistry container, ISyslogOptions options = null)
        {
            var instance = ((IContainerProvider)container).Resolve<SyslogLogger>();
            container.RegisterInstance<ILogger>(instance)
                .RegisterInstance<ILoggerFacade>(instance)
                .RegisterInstance<IAnalyticsService>(instance)
                .RegisterInstance<ICrashesService>(instance);
            return container;
        }
    }
}
