using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Prism.Logging.Logger;
using Xamarin.Forms;
using Log = Prism.Logging.Logger.Log;

namespace Prism.Plugin.Logging.XamarinLogsRepository
{
    public class UnsentLogsRepository : IDisposable, IUnsentLogsRepository
    {
        private const string UnsentLogKey = "UnsentLogs";

        private ConcurrentQueue<Log> _logs;

        public UnsentLogsRepository()
        {
            InitializeLogs();
           
        }

        private void InitializeLogs()
        {
            if (Application.Current.Properties.ContainsKey(UnsentLogKey))
            {
                if (Application.Current.Properties.TryGetValue(UnsentLogKey, out var json))
                {
                    _logs = JsonConvert.DeserializeObject<ConcurrentQueue<Log>>((string)json);
                }
            }

            _logs=new ConcurrentQueue<Log>();
            
        }

        public void Dispose()
        {
            StoreLogs();
        }

        private void StoreLogs()
        {
            string json = JsonConvert.SerializeObject(_logs);
            Application.Current.Properties.Add(UnsentLogKey, json);

            Application.Current.SavePropertiesAsync().Wait();
        }

        public bool IsEmpty => _logs.IsEmpty;

        public bool Add(Log log)
        {
            _logs.Enqueue(log);

            return true;
        }

        public bool Remove(Log log)
        {
            if (!_logs.TryPeek(out var removedLog))
            {
                return false;
            }
            if (removedLog != log)
            {
                return false;
            }

            if (_logs.TryDequeue(out removedLog))
            {
                return false;
            }
            if (removedLog != log)
            {
                _logs.Enqueue(removedLog);
                return false;
            }

            return true;
        }

        public Log GetLog()
        {
            if (!_logs.TryPeek(out var log))
            {
                return null;
            }

            return log;
        }
    }
}
