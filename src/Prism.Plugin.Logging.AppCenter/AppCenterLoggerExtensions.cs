using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Logging;
using Prism.Logging.AppCenter;

namespace Prism.Ioc
{
    public static class AppCenterLoggerExtensions
    {
        public static IContainerRegistry RegisterAppCenterLogger(this IContainerRegistry container, string appSecret = null)
        {
            if (string.IsNullOrEmpty(appSecret))
            {
                return container.RegisterNullLogger();
            }

            AppCenter.Start(appSecret, typeof(Analytics), typeof(Crashes));
            var instance = new AppCenterLogger();
            container.RegisterInstance<ILogger>(instance)
                .RegisterInstance<ILoggerFacade>(instance)
                .RegisterInstance<IAnalyticsService>(instance)
                .RegisterInstance<ICrashesService>(instance);

            return container;
        }
    }
}
