# Aggregate Logger

There may be times in which you want to log to multiple outputs. For this time an AggregateLogger has been provided in the Abstractions package. You can set it up as follows:

```c#
private AggregateLogger _logger;

protected override void OnInitialized()
{
    AppCenter.Start("{your application secrets}", typeof(Analytics), typeof(Crashes));
    _logger.AddLoggers(
        // NOTE: None of these need to be explicitly registered as you
        // are resolving concrete types
        Container.Resolve<SyslogLogger>(),
        Container.Resolve<ConsoleLoggingService>(),
        Container.Resolve<AppCenterLogger>()
    );
}

protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    _logger = new AggregateLogger();
    containerRegistry.RegisterInstance<ILogger>(_logger);
    containerRegistry.Register<ISyslogOptions, MySyslogOptions>();
}
```
