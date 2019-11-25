# Logging

It is important to understand that Prism is not considered to provide any sort of logger out of the box. The `ILoggerFacade` interface is provided as a legacy interface and does provide some debug output primarily for Prism.WPF apps, and is not really used by Prism.Forms. There are however several logging implementations provided as a Plugin for Prism which work with any Prism application.

## Support

This project is maintained by Dan Siegel. If this project or others maintained by Dan have helped you please help support the project by [sponsoring Dan](https://xam.dev/sponsor-prism-logging) on GitHub!

[![GitHub Sponsors](https://github.blog/wp-content/uploads/2019/05/mona-heart-featured.png?fit=600%2C315)](https://xam.dev/sponsor-prism-logging)

## NuGet

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Plugin.Logging.Abstractions | [![Latest NuGet][AbstractionsLoggingShield]][AbstractionsLoggingNuGet] | [![Latest CI Package][AbstractionsLoggingMyGetShield]][AbstractionsLoggingMyGet] |
| Prism.Plugin.Logging.Common | [![Latest NuGet][CommonLoggingShield]][CommonLoggingNuGet] | [![Latest CI Package][CommonLoggingMyGetShield]][CommonLoggingMyGet] |
| Prism.Plugin.Logging.AppCenter | [![Latest NuGet][AppCenterLoggingShield]][AppCenterLoggingNuGet] | [![Latest CI Package][AppCenterLoggingMyGetShield]][AppCenterLoggingMyGet] |
| Prism.Plugin.Logging.AppInsights | [![Latest NuGet][AppInightsLoggingShield]][AppInightsLoggingNuGet] | [![Latest CI Package][AppInightsLoggingMyGetShield]][AppInightsLoggingMyGet] |
| Prism.Plugin.Logging.Graylog | [![Latest NuGet][GraylogLoggingShield]][GraylogLoggingNuGet] | [![Latest CI Package][GraylogLoggingMyGetShield]][GraylogLoggingMyGet] |
| Prism.Plugin.Logging.Loggly | [![Latest NuGet][LogglyLoggingShield]][LogglyLoggingNuGet] | [![Latest CI Package][LogglyLoggingMyGetShield]][LogglyLoggingMyGet] |
| Prism.Plugin.Logging.Syslog | [![Latest NuGet][SyslogLoggingShield]][SyslogLoggingNuGet] | [![Latest CI Package][SyslogLoggingMyGetShield]][SyslogLoggingMyGet] |

[AbstractionsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Abstractions.svg
[AbstractionsLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.Abstractions
[AbstractionsLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Abstractions.svg

[CommonLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Common
[CommonLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Common.svg
[CommonLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.Common
[CommonLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Common.svg

[AppInightsLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppInsights.svg
[AppInightsLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.AppInsights
[AppInightsLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.AppInsights.svg

[AppCenterLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.AppCenter.svg
[AppCenterLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.AppCenter
[AppCenterLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.AppCenter.svg

[GraylogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Graylog
[GraylogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Graylog.svg
[GraylogLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.Graylog
[GraylogLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Graylog.svg

[LogglyLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Loggly
[LogglyLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Loggly.svg
[LogglyLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.Loggly
[LogglyLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Loggly.svg

[SyslogLoggingNuGet]: https://www.nuget.org/packages/Prism.Plugin.Logging.Syslog
[SyslogLoggingShield]: https://img.shields.io/nuget/vpre/Prism.Plugin.Logging.Syslog.svg
[SyslogLoggingMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Plugin.Logging.Syslog
[SyslogLoggingMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Plugin.Logging.Syslog.svg
