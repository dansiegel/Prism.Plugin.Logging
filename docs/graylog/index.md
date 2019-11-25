# Graylog

Graylog is a leading centralized log management solution built to open standards for capturing, storing, and enabling real-time analysis of terabytes of machine data. This offers a better user experience by making analysis ridiculously fast, efficient, cost-effective, and flexible. Graylog is offered both as an Open Source and Enterprise solution.

For more information about [Graylog](https://www.graylog.org/) be sure to check out their website and docs.

!!! Info "Info"
    Graylog support is offered using the Graylog Extended Logging Format (GELF) although you may also elect to use the Syslog Logger to log to a Graylog Server

## Setup

The setup for Graylog is very easy as you simply need to provide a Uri for where to send the logs to.

```c#
public interface IGelfOptions
{
    Uri Host { get; }
}
```

!!! Note "Note"
    Logs are sent via a HttpClient asynchronously in the background. The logging plugin currently does not handle scenarios where you may lose connection. Any logs that are sent while offline may be lost.
