namespace Prism.Logging.Syslog
{
    public interface ISyslogOptions
    {
        string HostNameOrIp { get; }

        int? Port { get; }

        string AppNameOrTag { get; }
    }
}