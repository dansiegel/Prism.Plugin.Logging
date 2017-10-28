using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Prism.Logging.Logger
{
    public class UnsentLogsRepository : IDisposable, IUnsentLogsRepository
    {
        private IPlatformStringStorage _logsStorage;

        private ConcurrentQueue<Log> _logs;

        public UnsentLogsRepository(IPlatformStringStorage logsStorage)
        {
            _logsStorage = logsStorage;

            InitializeLogs();
        }

        private void InitializeLogs()
        {
            var json = _logsStorage.Read();

             _logs = JsonConvert.DeserializeObject<ConcurrentQueue<Log>>((string)json);

            _logs=new ConcurrentQueue<Log>();
            
        }

        public void Dispose()
        {
            StoreLogs();
        }

        private void StoreLogs()
        {
            string json = JsonConvert.SerializeObject(_logs);


            _logsStorage.Write(json);
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
