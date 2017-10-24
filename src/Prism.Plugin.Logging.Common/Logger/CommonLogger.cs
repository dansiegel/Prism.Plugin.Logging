using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Logging.Logger
{
    public abstract class CommonLogger:ILoggerFacade
    {
        public async void Log(string message, Category category, Priority priority)
        {
            var result = await LogAsync(message, category, priority).ConfigureAwait(continueOnCapturedContext: false);

            if (!result)
            {
                SaveUnsentLog(message, category, priority);
            }
        }

        private void SaveUnsentLog(string message, Category category, Priority priority)
        {
            Debug.WriteLine(message);
        }

        protected abstract Task<bool> LogAsync(string message, Category category, Priority priority);
    }
}
