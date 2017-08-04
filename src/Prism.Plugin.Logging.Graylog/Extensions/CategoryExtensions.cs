using Prism.Logging;

namespace Prism.Logging.Graylog.Extensions
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
                    return Level.Info;
                case Category.Warn:
                    return Level.Warn;
                default:
                    return Level.Debug;
            }
        }
    }
}