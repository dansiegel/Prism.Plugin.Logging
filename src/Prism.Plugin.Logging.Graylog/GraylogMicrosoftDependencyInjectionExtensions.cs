using System;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.Graylog;
using Prism.Logging.Internals;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GraylogMicrosoftDependencyInjectionExtensions
    {
        public static IServiceCollection RegisterGraylogLogger<TOptions>(this IServiceCollection services)
       where TOptions : class, IGelfOptions
        {
            services.AddSingleton<IGelfOptions, TOptions>();
            return RegisterInternal(services);
        }

        public static IServiceCollection AddGraylogLogger(this IServiceCollection services, Action<GelfOptions> configureOptions)
        {
            if (configureOptions is null)
                throw new ArgumentNullException("You must provide a delegate function to configure the Graylog Logger Options for the GelfLogger");

            var options = new GelfOptions();
            configureOptions(options);

            return RegisterInternal(services, options);
        }

        public static IServiceCollection AddGraylogLogger(this IServiceCollection services, Uri host) =>
            RegisterInternal(services, new GelfOptions {
                Host = host
            });

        private static IServiceCollection RegisterInternal(this IServiceCollection services, GelfOptions options = null)
        {
            if (options != null)
            {
                if (options.Host is null || !options.Host.IsAbsoluteUri)
                {
                    Console.WriteLine("No Host or invalid relative Uri was detected");
                    return services.AddNullLogger();
                }

                services.AddSingleton<IGelfOptions>(options);
            }

            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<IGelfLogger, GelfLogger>()
                    .AddSingleton<IAggregableLogger>(sp => sp.GetRequiredService<IGelfLogger>());

            return services.RegisterManySingleton<GelfLogger>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
