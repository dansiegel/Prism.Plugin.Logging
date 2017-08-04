namespace Prism.Logging.Graylog
{
    public static class IGelfLoggerExtensions
    {
        public static void Log(this IGelfLogger logger, object message, Level level = Level.Debug) =>
            logger.Log($"{message}", level);
    }
}