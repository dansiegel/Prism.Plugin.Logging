namespace Prism.Logging.Graylog
{
    public interface IGelfLogger : IAggregableLogger
    {
        void Log(string message, Level level = Level.Debug);

        void Log(GelfMessage message);
    }
}