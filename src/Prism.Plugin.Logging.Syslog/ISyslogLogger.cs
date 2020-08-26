namespace Prism.Logging.Syslog
{
    public interface ISyslogLogger : IAggregableLogger
    {
        void Log(string message, Level level, Facility facility = Facility.Local0);
    }
}