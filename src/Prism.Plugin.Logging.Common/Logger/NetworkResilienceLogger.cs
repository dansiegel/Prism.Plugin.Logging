using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Logging.Logger
{
    public class NetworkResilienceLogger : ILoggerFacade
    {
        private ILogger _logger;

        public NetworkResilienceLogger(ILogger logger)
        {
            _logger = logger;
        }

        public async void Log(string message, Category category, Priority priority)
        {
            bool result = false;
            try
            {
                result = await _logger.LogAsync(message, category, priority).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            if (!result)
            {
                SaveUnsentLog(message, category, priority);
            }
        }

        private void SaveUnsentLog(string message, Category category, Priority priority)
        {
            Debug.WriteLine(message);
        }
    }
}
