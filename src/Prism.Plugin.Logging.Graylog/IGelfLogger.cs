namespace Prism.Logging.Graylog
{
    public interface IGelfLogger
    {
        void Log(string message, Level level = Level.Debug);

        void Log(GelfMessage message);
    }
}