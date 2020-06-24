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

| Package | NuGet | Sponsor Connect |
|-------|:-----:|:------:|
| Prism.Plugin.Logging.Abstractions | [![AbstractionsLoggingShield]][AbstractionsLoggingNuGet] | [![AbstractionsLoggingSponsorConnectShield]][AbstractionsLoggingSponsorConnect] |
| Prism.Plugin.Logging.Common | [![CommonLoggingShield]][CommonLoggingNuGet] | [![CommonLoggingSponsorConnectShield]][CommonLoggingSponsorConnect] |
| Prism.Plugin.Logging.AppCenter | [![AppCenterLoggingShield]][AppCenterLoggingNuGet] | [![AppCenterLoggingSponsorConnectShield]][AppCenterLoggingSponsorConnect] |
| Prism.Plugin.Logging.AppInsights | [![AppInightsLoggingShield]][AppInightsLoggingNuGet] | [![AppInightsLoggingSponsorConnectShield]][AppInightsLoggingSponsorConnect] |
| Prism.Plugin.Logging.Graylog | [![GraylogLoggingShield]][GraylogLoggingNuGet] | [![GraylogLoggingSponsorConnectShield]][GraylogLoggingSponsorConnect] |
| Prism.Plugin.Logging.Loggly | [![LogglyLoggingShield]][LogglyLoggingNuGet] | [![LogglyLoggingSponsorConnectShield]][LogglyLoggingSponsorConnect] |
| Prism.Plugin.Logging.Syslog | [![SyslogLoggingShield]][SyslogLoggingNuGet] | [![SyslogLoggingSponsorConnectShield]][SyslogLoggingSponsorConnect] |

## Support

If this project helped you reduce time to develop and made your app better, please be sure to Star the project help support Dan.

[![GitHub Sponsors](https://github.blog/wp-content/uploads/2019/05/mona-heart-featured.png?fit=600%2C315)](https://xam.dev/sponsor-prism-logging)

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

If you're using the Prism.Container.Extensions you might simply register your logger like:

```cs
containerRegistry.RegisterManySingleton<SyslogLogger>();
```

### Aggregate Logger

There are times where you may actually want to have multiple logging providers. For instance you may wish to send logs to App Center and the device console at the same time. The Aggregate Logger by default will log to the console, but gives you the ability to explicitly define any loggers that you may wish to use. Note that when you add the first logger it will clear the default Console Logger and you will need to pass in a Console Logger if you wish to continue using the Console Logger as one of the Aggregate Loggers.

```cs
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.Register<ISyslogOptions, MySyslogOptions>();
    containerRegistry.RegisterManySingleton<AggregateLogger>();
}

protected override void OnInitialized()
{
    var aggLogger = Container.Resolve<IAggregateLogger>();
    aggLogger.AddLoggers(
        Container.Resolve<ConsoleLoggingService>(),
        Container.Resolve<SyslogLogger>()
    );
}
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
[AbstractionsLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Abstractions%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==

[CommonLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Common
[CommonLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Common.svg
[CommonLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.Common
[CommonLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Common%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==

[AppInightsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppInsights.svg
[AppInightsLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.AppInsights%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==

[AppCenterLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppCenter.svg
[AppCenterLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.AppCenter%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==

[GraylogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Graylog
[GraylogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Graylog.svg
[GraylogLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.Graylog
[GraylogLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Graylog%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==

[LogglyLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Loggly
[LogglyLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Loggly.svg
[LogglyLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.Loggly
[LogglyLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Loggly%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==

[SyslogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Syslog
[SyslogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Syslog.svg
[SyslogLoggingSponsorConnect]: https://sponsorconnect.dev/nuget/package/Prism.Plugin.Logging.Syslog
[SyslogLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Syslog%2Fvpre&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAFLElEQVQ4jTWUa2wcVx3FfzN3HjuzL9vxa20nrh3XThxn67SOHaUiNrgpKF9oJSoRIlTgE30IVeIbfEGgggIqQioFVUhtqURKaVQSQevQh1pKESFJ3aRkqe16cTZrx45jr5+zszuPHTST5i/dq6t7dc//3HN1jhRUk6BtEdbgYIIrV2IIsY7vezwwepgvjB8bfefsSz9SVf3y9cW1N4rLmx8GUKHm4bkOtVotuivLcrSWgmoCtO1oc2AgQS4XQ1HWo8Ndbc2PdLUl/jQ2shfH9dm2HUrrZbdccYvnPyn+cmFx6Tk+LyEEvu+joAXRzokTcXI5hVTKY3PTI27oPxja1/50X1czt9YsfL+GEDItOxKqrBndh3sTv37xzGbsarH8jKoqgBQBipGDMo9+S2ZiIoFheiTi6oG7Olq+uK+n5Se6rpqW7eHXahi6GgFWXZ+NssfBVpu7M8b4ucnV30qyXA7PQkDp9vs10VCf/LGmBE/EDT1dnzbpaK1H12RMTcXxAtyQoSxjGDFSesDdxk3u6Urz9OvFM29cmH9YVVVc10VIstrZWJ94vSmtfTMeE7EgCGirT9Ld2YXjSZRtG993qFYrWBWHays2Ttli5C6BoQt6MvE9E5fXP65UqtMhObGzveUbbY3xJ0I2yILGpM7xBw/RtbuX4ewe+nv3oukmLc2tmIpEvGZz4eoc+Zs2/bvS9Lfr5JfdzMy88wp4vvB9EnFTH880JdMe0KDByP4+Bu8dYiY/x4uvniFpaCQTSbL9e/jh1w/jbt/id+em+LhQobsRctfWlmZX5T9Qq7rCdZ1rVsU7VdpyZ/PXVwtpUx8e2NPN0a8dw761zObKIo0t7cRjMiP3ZdlhSEi+w/bGDRaXVlhxG6hrbLv8n5mF34OPkCQJv4aladpH9w92vvnkt48/rMq0Jj2b3p1NjB4eYWt7m66dGQb7Opm+cp5kMsXpdz/inp4mvvvYUxw5eKD3+uJSPF+48bYsyTK6ptCzq4mNso1bXst/5cgwa1WBg05NNXnw/nsZPrCf9UpA3a4B6gyZsiOR3j3CoX0dZHaYYcOF8FMk0zTRVIHr+dh2pWPm3PNFT8SIKwGtaY25WxZNTc34ngNBgKKbbOQv8tlUjjIpxkeHOfnyW9eee+29sdXV1YKsaTpC0bDKLn39fUO7Dw1hWxUmc5/xwmt/Y3auSF3KoC6dQhYqNd+j6rik4zFKyzf4X8nl9LuXng3BIoaGYUTGdj0Px/E6/vnKyaKhwtm/TrCxtsLwwftAGJFLjo6PU11bZP16DiHDM3/8cOb5iU+yUKve8bR8Jy00VYXAn3/1z3/5x/kLF2nYO8YjT/2C9XSWDz5dRpMDZAIURbBVtlFkqDj+cgiWTCQIpYsAQ2dEJUUuZL7kvP/Y49+hvU5jY3mB/S0aP/v+oxwZG2NppcSNkoXleFhb2+SKpSlkHVkINE27DRhOUUpIEpnmJt7519Vnp6cLft+OGlPvnSK7M0E6bnLh4iRbdgVRXaPDDPjN2UkuzSz8ShE1bNuORmQ9SZJxHIcvHcry0AMj1KUS5dn8nHjo2PhoV08frqwzNTXFbKHIvoxJ4b+XOXl6kpffnz4Kwb9DycIR/kOUNkIohI8eyvaSmylgWWGn2pGfPv7Vvw8N7MayLD649CmFmxuYsj//5sX8SyUr+LmEuxXU/M/VkgjTJiSGUBRkoTCU7SOm39YhrIZU/ES2u+VUpsF84cnjX/5eb2dmL6jIik5dOo2ihKF6R37ptobA/wFjXTUfqzHjCAAAAABJRU5ErkJggg==
