using System.Threading.Tasks;
using Prism.Logging.Logger;

namespace Prism.Logging.TestsHelpers
{
    public class ErrorLogger:ILogger
    {
        public async Task<bool> LogAsync(string message, Category category, Priority priority)
        {
            if (message != "OK")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
