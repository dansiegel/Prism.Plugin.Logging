# Application Insights

!!! question "What is Application Insights"
    Application Insights, a feature of Azure Monitor, is an extensible Application Performance Management (APM) service for web developers on multiple platforms. Use it to monitor your live web application. It will automatically detect performance anomalies. It includes powerful analytics tools to help you diagnose issues and to understand what users actually do with your app. It's designed to help you continuously improve performance and usability.

!!! info "Info"
    For more information see the [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) documentation.

## Setup

In order to get started with Application Insights you will need to be sure to provide an implementation of the IApplicationInsightsOptions to provide the logger with your InstrumentationKey and any UserTraits which you want Application Insights to track. This may include properties such as information on what type of device, OS, etc that your user is using your application on.

```c#
public interface IApplicationInsightsOptions
{
    string InstrumentationKey { get; }

    IDictionary<string, string> UserTraits { get; }
}
```
