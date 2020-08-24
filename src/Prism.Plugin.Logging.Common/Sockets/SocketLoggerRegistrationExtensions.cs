using System;
using System.Net.Sockets;
using Prism.Logging;
using Prism.Logging.Sockets;

namespace Prism.Ioc
{
    public static class SocketLoggerRegistrationExtensions
    {
        public static IContainerRegistry RegisterSocketLogger<TOptions>(this IContainerRegistry containerRegistry)
            where TOptions : ISocketLoggerOptions
        {
            containerRegistry.RegisterSingleton<ISocketLoggerOptions, TOptions>();
            return containerRegistry.RegisterSingleton<ILogger, SocketLogger>()
                .RegisterSingleton<IAnalyticsService, SocketLogger>()
                .RegisterSingleton<ICrashesService>();
        }

        public static IContainerRegistry RegisterSocketLogger(this IContainerRegistry container, Action<SocketLoggerOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Socket Logger Options for the SocketLogger");

            var options = new SocketLoggerOptions();
            configureOptions(options);

            return RegisterInternal(container, options);
        }

        public static IContainerRegistry RegisterSocketLogger(this IContainerRegistry container, string hostOrIp, int port = 4040, ProtocolType protocolType = ProtocolType.Udp) =>
            RegisterInternal(container, new SocketLoggerOptions
            {
                HostOrIp = hostOrIp,
                Port = port,
                ProtocolType = protocolType
            });

        private static IContainerRegistry RegisterInternal(this IContainerRegistry container, SocketLoggerOptions options = null)
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.HostOrIp)) 
                {
                    Console.WriteLine("No Host or IP provided");
                    return container.RegisterNullLogger();
                }

                container.RegisterInstance<ISocketLoggerOptions>(options);
            }

            var instance = ((IContainerProvider)container).Resolve<SocketLogger>();
            container.RegisterInstance<ILogger>(instance)
                .RegisterInstance<ILoggerFacade>(instance)
                .RegisterInstance<IAnalyticsService>(instance)
                .RegisterInstance<ICrashesService>(instance);

            return container;
        }
    }
}
