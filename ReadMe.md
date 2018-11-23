# Prism Logging Plugin's

Prism's ILoggerFacade provides logging of all internal Prism errors, and a quick and easy way for your WPF, UWP, or Xamarin Forms app to introduce logging throughout your ViewModels and Services. The implementation of ILoggerFacade is really left to the developer to determine how you want to handle your logging. While this "Works", it is also a more than a decade old definition that doesn't match modern application logging demands. For this reason the Prism Logging Plugins introduce some new logging interfaces:

- IAnalyticsService
  - adds `void TrackEvent(string name, IDictionary&lt;string, string&gt; properties)`
- ICrashesService
  - adds `void Report(Exception ex, IDictionary&lt;string, string&gt; properties)`
- ILogger (inherits from IAnalyticsService, ICrashesService, ILoggerFacade)
  - adds `void Log(string message, IDictionary&lt;string, string&gt; properties)`
  - has extensions for Debug, Info, Warn

[![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/Prism.Plugins.Logging-CI)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=29)

## NuGet

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Plugin.Logging.Abstractions | [![AbstractionsLoggingShield]][AbstractionsLoggingNuGet] | [![AbstractionsLoggingMyGetShield]][AbstractionsLoggingMyGet] |
| Prism.Plugin.Logging.Common | [![CommonLoggingShield]][CommonLoggingNuGet] | [![CommonLoggingMyGetShield]][CommonLoggingMyGet] |
| Prism.Plugin.Logging.AppCenter | [![AppCenterLoggingShield]][AppCenterLoggingNuGet] | [![AppCenterLoggingMyGetShield]][AppCenterLoggingMyGet] |
| Prism.Plugin.Logging.AppInsights | [![AppInightsLoggingShield]][AppInightsLoggingNuGet] | [![AppInightsLoggingMyGetShield]][AppInightsLoggingMyGet] |
| Prism.Plugin.Logging.Graylog | [![GraylogLoggingShield]][GraylogLoggingNuGet] | [![GraylogLoggingMyGetShield]][GraylogLoggingMyGet] |
| Prism.Plugin.Logging.Loggly | [![LogglyLoggingShield]][LogglyLoggingNuGet] | [![LogglyLoggingMyGetShield]][LogglyLoggingMyGet] |
| Prism.Plugin.Logging.Syslog | [![SyslogLoggingShield]][SyslogLoggingNuGet] | [![SyslogLoggingMyGetShield]][SyslogLoggingMyGet] |

## Support

If this project helped you reduce time to develop and made your app better, please help support this project.

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.me/dansiegel)

## Providers

Logging is an important facet of app development. It can help you identify how users are navigating through your application, alert you when unexcepted exceptions are thrown, and help shorten the Dev-Loop. Note that the sample below show registering the logger with Prism IContainerRegistry as a Transient Service. It is typically advisable to access the underlying container and register a single instance of your logger against any interfaces that may be called by your app.

```cs
// For DryIoc it might look something like this:
var container = containerRegistry.GetContainer();
container.RegisterMany<SyslogLogger>(Reuse.Singleton,
                                     ifAlreadyRegistered: IfAlreadyRegistered.Replace,
                                     serviceTypeCondition: t => typeof(SyslogLogger).ImplementsServiceType(t));

// For Unity it might look something like this:
var logger = Container.Resolve<SyslogLogger>();
containerRegistry.RegisterInstance<ILoggerFacade>(logger);
containerRegistry.RegisterInstance<ILogger>(logger);
containerRegistry.RegisterInstance<IAnalyticsService>(logger);
containerRegistry.RegisterInstance<ICrashesService>(logger);
containerRegistry.RegisterInstance<ISyslogLogger>(logger);
```

### App Center & Application Insights

The App Center and Application Insights packages both make some assumptions that while running a Debug build that the logging output should be sent to the Application Output (the console in the IDE). Simply running a Release build will trigger the logger to attempt to send telemetry using the App Center or Application Insights SDK's

Starting with version 1.2, two new interfaces were added for mocking Analytics and Crashes. While the `Report` API was previously available in the ILogger, this has been moved to the `ICrashesService`, with a new `IAnalyticsService` that exposes a TrackEvent API. Both of these interfaces are implemented through the ILogger. These services map directly App Center Analytics and Crashes API's. This allows you to null logging events you may use during development, while leaving intact specific tracking of events and crashes.

#### Using Application Insights

Application Insights requires that you have an Instrumentation Key to create the client. In addition you may pass any root telemetry you wish about your users. This could include the Device type, OS version, etc. You are able to pass both the Instrumentation Key and User Traits to the Application Insights Logger using the `IApplicationInsightsOptions`. Note that Prism.Forms developers may want implement this on a Platform Specific basis and register this with the `IPlatformInitializer`.

#### Debug Logging

Both the App Center and Application Insights loggers attempt to make a determination if the build is a Debug build. This is done based on the entry assembly. All Debug builds will automatically write to the Device Console. The Console will also be used if the underlying service is not available. This could occur becuase you failed to start App Center Crashes or Analyics, or provided the wrong client id for either App Center or Application Insights.

### SocketLogger

The SocketLogger is configurable to send messages using any Socket Type you choose. Typically you would choose Udp or Tcp. This works great for simple solutions where you simply want to receive logging messages over the network.

```cs
public class SocketOptions : ISocketLoggerOptions
{
    // e.g. 192.168.0.100
    // e.g. logging.contoso.com
    public string HostOrIp => Secrets.LoggingHost;

    public int Port => 12345;

    public ProtocolType ProtocolType => ProtocolType.Udp;
}

public class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<ISocketLoggerOptions, SocketOptions>();

        // Resolve the logger as a Transient Service
        containerRegistry.Register<ILoggerFacade, SocketLogger>();
        containerRegistry.Register<ILogger, SocketLogger>();
    }
}
```

### Syslog

The Syslog package will enable you to send your logging messages to the Syslog Server of your choice, and works great with [Visual Syslog Server](https://github.com/MaxBelkov/visualsyslog).

```cs
public class AwesomeAppOptions : ISyslogOptions
{
    // e.g. 192.168.0.100
    // e.g. logging.contoso.com
    public string HostNameOrIp => Secrets.LoggingHost;

    // If this is null, the SyslogLogger will use port 514
    public int? Port => int.Parse(Secrets.LoggingPort);

    public string AppNameOrTag => "AwesomeApp"
}

public class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<ISyslogOptions, AwesomeAppOptions>();

        // Registers the Syslog Logger as a Transient Service
        containerRegistry.Register<ILoggerFacade, SyslogLogger>();
        containerRegistry.Register<ILogger, SyslogLogger>();
    }
}
```

### Loggly

[Loggly](https://www.loggly.com/) offers both free and paid Logging as a Service plans. The Loggly package will allow you to choose from either their Syslog implementation or Rest service.

**NOTE** By default we will send all requests to `logs-01.loggly.com`. To override this you will need to inherit from the Logging class.

- For `LogglyHttpLogger` you will need to override `LogglyBaseUri`'s default value of `https://logs-01.loggly.com`
- For `LogglySyslogLogger` you will need to implement a Logger class that derives from `LogglySyslogLogger` and update the `HostNameOrIp` property.

```cs
public class LogglyOptions : ILogglyOptions
{
    public string Token => Secrets.LogglyToken;

    public string AppName => "AwesomeApp";

    public IEnumerable<string> Tags => new string[]
    {
        "sample"
    };
}

public class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Used by both the LogglyHttpLogger and LogglySyslogLogger
        containerRegistry.Register<ILogglyOptions, LogglyOptions>();

        containerRegistry.Register<ILoggerFacade, LogglyHttpLogger>();
        // Or
        containerRegistry.Register<ILoggerFacade, LogglySyslogLogger>();
    }
}
```

### Graylog (Graylog Extended Log Format)

Graylog is a very popular Logging platform which will accept Syslog messages. In addition to the Syslog Messages, you can also choose to use the Graylog package to take advantage of the Graylog REST Api and GELF Log Messages.

```cs
public class GelfOptions : IGelfOptions
{
    // e.g. http://graylog.example.org:12202
    public Uri Host => new Uri(Secrets.GraylogHost);
}

public class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<IGelfOptions, GelfOptions>();
        containerRegistry.Register<ILoggerFacade, GelfLogger>();
    }
}
```

[AbstractionsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Abstractions.svg
[AbstractionsLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Abstractions.svg

[CommonLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Common
[CommonLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Common.svg
[CommonLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.Common
[CommonLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Common.svg

[AppInightsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppInsights.svg
[AppInightsLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.AppInsights.svg

[AppCenterLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppCenter.svg
[AppCenterLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.AppCenter.svg

[GraylogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Graylog
[GraylogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Graylog.svg
[GraylogLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.Graylog
[GraylogLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Graylog.svg

[LogglyLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Loggly
[LogglyLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Loggly.svg
[LogglyLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.Loggly
[LogglyLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Loggly.svg

[SyslogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Syslog
[SyslogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Syslog.svg
[SyslogLoggingMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Plugin.Logging.Syslog
[SyslogLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Syslog.svg
