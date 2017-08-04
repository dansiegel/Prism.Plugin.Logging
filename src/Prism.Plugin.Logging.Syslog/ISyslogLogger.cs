namespace Prism.Logging.Syslog
{
    public interface ISyslogLogger
    {
        void Log(string message, Level level, Facility facility = Facility.Local0);
    }
}