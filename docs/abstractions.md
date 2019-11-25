There are several interfaces provided by Prism.Plugin.Logging in the Abstractions package. While you can register any of the given services, most applications would simply register an make use of the `ILogger` interface. This gives you the greatest flexibility to determine what type of logging you would like to do. You may notice that these interfaces provide a more modern context and largely match the API's provided by App Center for collecting Analytics and Tracking Errors within your application.

```c#
public interface IAnalyticsService
{
    void TrackEvent(string name, IDictionary<string, string> properties);
}

public interface ICrashesService
{
    void Report(Exception ex, IDictionary<string, string> properties);
}

public interface ILogger : ILoggerFacade, IAnalyticsService, ICrashesService
{
    void Log(string message, IDictionary<string, string> properties);
}
```

!!! Note "Note"
    Base implementations are also provided in the Abstractions package for Null Logging and Console Logging.
