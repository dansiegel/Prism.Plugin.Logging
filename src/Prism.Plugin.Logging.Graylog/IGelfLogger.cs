using System.Threading.Tasks;

namespace Prism.Logging.Graylog
{
    public interface IGelfLogger
    {
        void Log(string message, Level level = Level.Debug);

        void Log(GelfMessage message);

        Task<bool> LogAsync(string message, Level level = Level.Debug);

        Task<bool> LogAsync(GelfMessage message);
    }
}