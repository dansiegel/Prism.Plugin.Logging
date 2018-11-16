using DryIoc;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Prism.Logging.AppInsights;
using Prism.Logging.Graylog;
using Prism.Logging.Loggly;
using Prism.Logging.Syslog;
using SampleApp.Events;
using SampleApp.Services;
using SampleApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SampleApp
{
    public partial class App
    {
        protected override void OnInitialized()
        {
            InitializeComponent();
            var ea = Container.Resolve<IEventAggregator>();
            ea.GetEvent<UpdateLoggerEvent>().Subscribe(OnLoggerUpdated);
            NavigationService.NavigateAsync("MainPage/NavigationPage/LogGeneratorPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();
            container.RegisterMany<AppCenterConfig>(Reuse.Singleton,
                serviceTypeCondition: t => typeof(AppCenterConfig).ImplementsServiceType(t));
            container.RegisterMany<AppInsightsConfig>(Reuse.Singleton,
                serviceTypeCondition: t => typeof(AppInsightsConfig).ImplementsServiceType(t));
            container.RegisterMany<GelfConfig>(Reuse.Singleton,
                serviceTypeCondition: t => typeof(GelfConfig).ImplementsServiceType(t));
            container.RegisterMany<LogglyConfig>(Reuse.Singleton,
                serviceTypeCondition: t => typeof(LogglyConfig).ImplementsServiceType(t));
            container.RegisterMany<SyslogConfig>(Reuse.Singleton,
                serviceTypeCondition: t => typeof(SyslogConfig).ImplementsServiceType(t));

            container.RegisterMany<ConsoleLoggingService>(Reuse.Singleton,
                serviceTypeCondition: t => typeof(ConsoleLoggingService).ImplementsServiceType(t));

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<LogGeneratorPage>();
            containerRegistry.RegisterForNavigation<AppCenterConfigPage>();
            containerRegistry.RegisterForNavigation<AppInsightsConfigPage>();
            containerRegistry.RegisterForNavigation<GelfConfigPage>();
            containerRegistry.RegisterForNavigation<LogglyConfigPage>();
            containerRegistry.RegisterForNavigation<SyslogConfigPage>();
        }

        private void OnLoggerUpdated(LoggerType loggerType)
        {
            Container.Resolve<ILogger>().TrackEvent("Stopping Logger...");
            var container = Container.GetContainer();
            switch(loggerType)
            {
                case LoggerType.AppCenter:
                    AppCenter.Start(Container.Resolve<IAppCenterConfig>().Secret, typeof(Analytics), typeof(Crashes));
                    container.RegisterMany<AppCenterLogger>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(AppCenterLogger).ImplementsServiceType(t));
                    break;
                case LoggerType.AppInsights:
                    container.RegisterMany<AppInsightsLogger>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(AppInsightsLogger).ImplementsServiceType(t));
                    break;
                case LoggerType.Graylog:
                    container.RegisterMany<GelfLogger>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(GelfLogger).ImplementsServiceType(t));
                    break;
                case LoggerType.LogglyHttp:
                    container.RegisterMany<LogglyHttpLogger>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(LogglyHttpLogger).ImplementsServiceType(t));
                    break;
                case LoggerType.LogglySyslog:
                    container.RegisterMany<LogglySyslogLogger>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(LogglySyslogLogger).ImplementsServiceType(t));
                    break;
                case LoggerType.Syslog:
                    container.RegisterMany<SyslogLogger>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(SyslogLogger).ImplementsServiceType(t));
                    break;
                default:
                    container.RegisterMany<ConsoleLoggingService>(Reuse.Singleton,
                        ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                        serviceTypeCondition: t => typeof(ConsoleLoggingService).ImplementsServiceType(t));
                    break;
            }

            Container.Resolve<ILogger>().TrackEvent("Starting Logger...");
            NavigationService.NavigateAsync("/MainPage/NavigationPage/LogGeneratorPage");
        }
    }
}
