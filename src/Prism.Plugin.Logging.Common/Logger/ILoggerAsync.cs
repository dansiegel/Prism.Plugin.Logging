using System.Threading.Tasks;

namespace Prism.Logging.Logger
{
    public interface ILoggerAsync
    {
        Task<bool> LogAsync(string message, Category category, Priority priority);
    }
}