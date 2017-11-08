using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Prism.Logging.Logger
{
    public class UnsentLogsRepository : IDisposable, IUnsentLogsRepository
    {
        private IPlatformStringStorage _logsStorage;

        private bool _disposed = false;
        private ConcurrentQueue<Log> _logs;

        public UnsentLogsRepository(IPlatformStringStorage logsStorage)
        {
            _logsStorage = logsStorage;

            InitializeLogs();
        }

        private void InitializeLogs()
        {
            var json = _logsStorage.Read();
            if (json != String.Empty)
            {
                _logs = JsonConvert.DeserializeObject<ConcurrentQueue<Log>>((string) json);
            }
            else
            {
                _logs = new ConcurrentQueue<Log>();
            }
            
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

            StoreLogs();
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    StoreLogs();
                }
                _disposed = true;
            }
        }

        ~UnsentLogsRepository() { Dispose(false); }
    }
}
