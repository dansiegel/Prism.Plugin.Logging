# Prism Logging Plugin's

Prism's ILoggerFacade provides logging of all internal Prism errors, and a quick and easy way for your WPF, UWP, or Xamarin Forms app to introduce logging throughout your ViewModels and Services. The implementation of ILoggerFacade is really left to the developer to determine how you want to handle your logging. Using the Logging Plugins will allow you to rapidly configure remote network based logging.

**NOTE** These packages only take a dependency on Prism.Core and as such will work in any project using Prism.

![Current Build][buildStatus]

## NuGet

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Logging.Common | [![CommonLoggingShield]][CommonLoggingNuGet] | [![CommonLoggingMyGetShield]][CommonLoggingMyGet] |
| Prism.Logging.AppCenter | [![AppCenterLoggingShield]][AppCenterLoggingNuGet] | [![AppCenterLoggingMyGetShield]][AppCenterLoggingMyGet] |
| Prism.Logging.AppCenter | [![AppInsightsLoggingShield]][AppInsightsLoggingNuGet] | [![AppInsightsLoggingMyGetShield]][AppInsightsLoggingMyGet] |
| Prism.Logging.Graylog | [![GraylogLoggingShield]][GraylogLoggingNuGet] | [![GraylogLoggingMyGetShield]][GraylogLoggingMyGet] |
| Prism.Logging.Loggly | [![LogglyLoggingShield]][LogglyLoggingNuGet] | [![LogglyLoggingMyGetShield]][LogglyLoggingMyGet] |
| Prism.Logging.Syslog | [![SyslogLoggingShield]][SyslogLoggingNuGet] | [![SyslogLoggingMyGetShield]][SyslogLoggingMyGet] |

## Support

If this project helped you reduce time to develop and made your app better, please help support this project.

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.me/dansiegel)

## Providers

The Plugin supports various Network based Logging providers to better assist you in collecting data on your apps.

### App Center & Application Insights

The App Center and Application Insights packages both make some assumptions that while running a Debug build that the logging output should be sent to the Application Output (the console in the IDE). Simply running a Release build will trigger the logger to attempt to send telemetry using the App Center or Application Insights SDK's.

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
    protected override void RegisterTypes()
    {
        Container.Register<ISocketLoggerOptions, SocketOptions>();
        Container.Register<ILoggerFacade, SocketLogger>();
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
    protected override void RegisterTypes()
    {
        Container.Register<ISyslogOptions, AwesomeAppOptions>();
        Container.Register<ILoggerFacade, SyslogLogger>();
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
    protected override void RegisterTypes()
    {
        // Used by both the LogglyHttpLogger and LogglySyslogLogger
        Container.Register<ILogglyOptions, LogglyOptions>();

        Container.Register<ILoggerFacade, LogglyHttpLogger>();
        // Or
        Container.Register<ILoggerFacade, LogglySyslogLogger>();
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
    protected override void RegisterTypes()
    {
        Container.Register<IGelfOptions, GelfOptions>();
        Container.Register<ILoggerFacade, GelfLogger>();
    }
}
```

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

[buildStatus]: https://dansiegel.visualstudio.com/6e9062a8-b622-4f65-978b-1f630d6b7776/_apis/build/status/5
