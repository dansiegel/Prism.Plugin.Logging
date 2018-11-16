using Prism.Logging.Syslog;

namespace SampleApp.Services
{
    public interface ISyslogConfig
    {
        string AppNameOrTag { get; set; }
        string HostNameOrIp { get; set; }
        int? Port { get; set; }
    }
}