using Prism.Logging;
using Prism.Logging.Internals;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DefaultMicrosoftRegistrationExtensions
    {
        public static IServiceCollection UseAggregateLogger(this IServiceCollection services)
        {
            return services.AddSingleton<AggregateLogger>(sp =>
            {
                var logger = new AggregateLogger();
                var aggregate = (IAggregateLogger)logger;
                aggregate.AddLoggers(sp.GetServices<IAggregableLogger>());
                return logger;
            })
                .AddSingleton<IAggregateLogger>(sp => sp.GetRequiredService<AggregateLogger>())
                .AddSingleton<ILogger>(sp => sp.GetRequiredService<AggregateLogger>())
                .AddSingleton<IAnalyticsService>(sp => sp.GetRequiredService<AggregateLogger>())
                .AddSingleton<ICrashesService>(sp => sp.GetRequiredService<AggregateLogger>());
        }

        public static IServiceCollection AddNullLogger(this IServiceCollection services)
        {
            // There's no reason to register the NullLogger if we already have a logger defined
            if (services.IsRegistered<ILogger>())
                return services;

            return services.RegisterManySingleton<NullLoggingService>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }

        public static IServiceCollection AddConsoleLogger(this IServiceCollection services)
        {
            if (services.IsRegistered<IAggregateLogger>())
                return services.AddSingleton<IAggregableLogger, ConsoleLoggingService>();

            return services.RegisterManySingleton<ConsoleLoggingService>(
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger));
        }
    }
}
