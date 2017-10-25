using System.Threading.Tasks;

namespace Prism.Logging.Logger
{
    public interface ILogger
    {
        Task<bool> LogAsync(string message, Category category, Priority priority);
    }
}