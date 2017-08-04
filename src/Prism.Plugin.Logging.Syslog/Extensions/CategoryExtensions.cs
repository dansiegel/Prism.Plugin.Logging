using Prism.Logging.Syslog;

namespace Prism.Logging.Syslog.Extensions
{
    public static class CategoryExtensions
    {
        public static Level ToLevel(this Category category)
        {
            switch (category)
            {
                case Category.Exception:
                    return Level.Error;
                case Category.Info:
                    return Level.Information;
                case Category.Warn:
                    return Level.Warning;
                default:
                    return Level.Debug;
            }
        }
    }
}