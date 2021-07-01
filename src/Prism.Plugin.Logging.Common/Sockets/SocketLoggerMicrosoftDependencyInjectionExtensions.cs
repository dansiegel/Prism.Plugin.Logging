using System;
using System.Net.Sockets;
using Prism.Logging;
using Prism.Logging.Internals;
using Prism.Logging.Sockets;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SocketLoggerMicrosoftDependencyInjectionExtensions
    {
        public static IServiceCollection AddSocketLogger<TOptions>(this IServiceCollection services)
       where TOptions : class, ISocketLoggerOptions
        {
            services.AddSingleton<ISocketLoggerOptions, TOptions>();
            return RegisterInternal(services);
        }

        public static IServiceCollection AddSocketLogger(this IServiceCollection services, Action<SocketLoggerOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Socket Logger Options for the SocketLogger");

            var options = new SocketLoggerOptions();
            configureOptions(options);

            return RegisterInternal(services, options);
        }

        public static IServiceCollection AddSocketLogger(this IServiceCollection services, string hostOrIp, int port = 4040, ProtocolType protocolType = ProtocolType.Udp) =>
            RegisterInternal(services, new SocketLoggerOptions {
                HostOrIp = hostOrIp,
                Port = port,
                ProtocolType = protocolType
            });

        private static IServiceCollection RegisterInternal(this IServiceCollection services, SocketLoggerOptions options = null)
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.HostOrIp))
                {
                    Console.WriteLine("No Host or IP provided");
                    return services.AddNullLogger();
                }

                services.AddSingleton<ISocketLoggerOptions>(options);
            }

            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<IAggregableLogger, SocketLogger>();

            return services.RegisterManySingleton<SocketLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
