namespace Prism.Logging.Syslog
{
    public class SyslogOptions : ISyslogOptions
    {
        public string HostNameOrIp { get; set; }
        public int? Port { get; set; }
        public string AppNameOrTag { get; set; }
    }
}