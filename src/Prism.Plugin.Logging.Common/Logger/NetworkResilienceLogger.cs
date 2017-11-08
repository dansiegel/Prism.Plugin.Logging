using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Logging.Logger
{
    public class NetworkResilienceLogger : ILoggerFacade
    {
        private readonly IUnsentLogsRepository _unsentLogs;
        private readonly ILogger _logger;

        public NetworkResilienceLogger(ILogger logger, IUnsentLogsRepository unsentLogs)
        {
            _logger = logger;
            _unsentLogs = unsentLogs;
        }

        public async void Log(string message, Category category, Priority priority)
        {
            bool result = false;
            if (await SendUnsentLogsAsync().ConfigureAwait(continueOnCapturedContext: false))
            {
                try
                {
                    result = await _logger.LogAsync(message, category, priority)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            if (!result)
            {
                SaveUnsentLog(message, category, priority);
            }
        }

        private async Task<bool> SendUnsentLogsAsync()
        {
            try
            {
                while (!_unsentLogs.IsEmpty)
                {
                    var log = _unsentLogs.GetLog();
                    if (log==null)
                    {
                        return false;
                    }

                    var result = await _logger.LogAsync(log.Message, log.Category, log.Priority)
                        .ConfigureAwait(continueOnCapturedContext: false);
                    if (!result)
                    {
                        return false;
                    }

                    if (_unsentLogs.Remove(log))
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

            return true;
        }


        private void SaveUnsentLog(string message, Category category, Priority priority)
        {
            var log=new Log()
            {
                Message = message,
                Category = category,
                Priority = priority
            };

            _unsentLogs.Add(log);
            
            Debug.WriteLine(message);
        }
    }
}
