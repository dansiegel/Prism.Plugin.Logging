using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Logging.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PrismCompatExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool IsRegistered<T>(this IServiceCollection services) =>
            services.Any(x => x.ServiceType == typeof(T));

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IServiceCollection RegisterManySingleton<T>(this IServiceCollection services, params Type[] serviceTypes)
            where T : class
        {
            services.AddSingleton<T>();
            foreach(var type in serviceTypes)
            {
                services.AddSingleton(type, sp => sp.GetRequiredService<T>());
            }

            return services;
        }

    }
}
