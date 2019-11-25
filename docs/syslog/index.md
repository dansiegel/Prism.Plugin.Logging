# Syslog

!!! question "What is Syslog?"
    Syslog is a way for network devices to send event messages to a logging server – usually known as a Syslog server. The Syslog protocol is supported by a wide range of devices and can be used to log different types of events. For example, a router might send messages about users logging on to console sessions, while a web-server might log access-denied events.
    https://www.networkmanagementsoftware.com/what-is-syslog/

The Syslog Logger can be a great development tool, particular for Mobile Developers which may be running test builds on devices while disconnected from Visual Studio and the Debug Console. This can be very easily achieved on Windows by installing any one of a number of Syslog Servers, where this will enabled you as a developer to get a stream of log data directly to your desktop separate from Visual Studio.

!!! Note "Note"
    The Syslog Logger is built on top of the Socket Messenger in the `Prism.Plugin.Logging.Common` NuGet package. All Syslog messages use the Udp Protocol with a max buffer size of 65500.

## Setup

Logging Syslog messages is quite easy and simple but requires a few properties to be set. You must set both the Port and AppNameOrTag properties for the Syslog Logger to know where to send the log message and which tag to use to help you filter logs.

```c#
public interface ISyslogOptions
{
    string HostNameOrIp { get; }

    int? Port { get; }

    string AppNameOrTag { get; }
}
```

!!! Note "Note"
    If no Port is specified the default Syslog port 514 will be used.

## See Also

- [Socket Logger](../socket/index.md)
- [Free Desktop Syslog Servers](https://www.ittsystems.com/best-free-syslog-server-windows/)
