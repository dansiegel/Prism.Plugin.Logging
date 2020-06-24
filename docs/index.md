# Logging

It is important to understand that Prism is not considered to provide any sort of logger out of the box. The `ILoggerFacade` interface is provided as a legacy interface and does provide some debug output primarily for Prism.WPF apps, and is not really used by Prism.Forms. There are however several logging implementations provided as a Plugin for Prism which work with any Prism application.

## Support

This project is maintained by Dan Siegel. If this project or others maintained by Dan have helped you please help support the project by [sponsoring Dan](https://xam.dev/sponsor-prism-logging) on GitHub!

[![GitHub Sponsors](https://github.blog/wp-content/uploads/2019/05/mona-heart-featured.png?fit=600%2C315)](https://xam.dev/sponsor-prism-logging)

## NuGet

| Package | NuGet | Sponsor Connect |
|-------|:-----:|:------:|
| Prism.Plugin.Logging.Abstractions | [![Latest NuGet][AbstractionsLoggingShield]][AbstractionsLoggingNuGet] | [![Latest CI Package][AbstractionsLoggingSponsorConnectShield]][AbstractionsLoggingSponsorConnect] |
| Prism.Plugin.Logging.Common | [![Latest NuGet][CommonLoggingShield]][CommonLoggingNuGet] | [![Latest CI Package][CommonLoggingSponsorConnectShield]][CommonLoggingSponsorConnect] |
| Prism.Plugin.Logging.AppCenter | [![Latest NuGet][AppCenterLoggingShield]][AppCenterLoggingNuGet] | [![Latest CI Package][AppCenterLoggingSponsorConnectShield]][AppCenterLoggingSponsorConnect] |
| Prism.Plugin.Logging.AppInsights | [![Latest NuGet][AppInightsLoggingShield]][AppInightsLoggingNuGet] | [![Latest CI Package][AppInightsLoggingSponsorConnectShield]][AppInightsLoggingSponsorConnect] |
| Prism.Plugin.Logging.Graylog | [![Latest NuGet][GraylogLoggingShield]][GraylogLoggingNuGet] | [![Latest CI Package][GraylogLoggingSponsorConnectShield]][GraylogLoggingSponsorConnect] |
| Prism.Plugin.Logging.Loggly | [![Latest NuGet][LogglyLoggingShield]][LogglyLoggingNuGet] | [![Latest CI Package][LogglyLoggingSponsorConnectShield]][LogglyLoggingSponsorConnect] |
| Prism.Plugin.Logging.Syslog | [![Latest NuGet][SyslogLoggingShield]][SyslogLoggingNuGet] | [![Latest CI Package][SyslogLoggingSponsorConnectShield]][SyslogLoggingSponsorConnect] |

[AbstractionsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Abstractions.svg
[AbstractionsLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Abstractions%2Fvpre

[CommonLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Common
[CommonLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Common.svg
[CommonLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.Common
[CommonLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Common%2Fvpre

[AppInightsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppInsights.svg
[AppInightsLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.AppInsights%2Fvpre

[AppCenterLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppCenter.svg
[AppCenterLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.AppCenter%2Fvpre

[GraylogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Graylog
[GraylogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Graylog.svg
[GraylogLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.Graylog
[GraylogLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Graylog%2Fvpre

[LogglyLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Loggly
[LogglyLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Loggly.svg
[LogglyLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.Loggly
[LogglyLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Loggly%2Fvpre

[SyslogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Syslog
[SyslogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Syslog.svg
[SyslogLoggingSponsorConnect]: https://sponsorconnect.dev/packages/Prism.Plugin.Logging.Syslog
[SyslogLoggingSponsorConnectShield]: https://img.shields.io/endpoint?url=https%3A%2F%2Fsponsorconnect.dev%2Fshield%2FPrism.Plugin.Logging.Syslog%2Fvpre
