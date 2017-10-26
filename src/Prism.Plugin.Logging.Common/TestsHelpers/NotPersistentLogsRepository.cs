using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Prism.Logging.Logger;

namespace Prism.Logging.TestsHelpers
{
    public class NotPersistentLogsRepository : IUnsentLogsRepository
    {
        ConcurrentQueue<Log> _logs=new ConcurrentQueue<Log>();

        public bool IsEmpty => _logs.IsEmpty;

        public bool Add(Log log)
        {
            _logs.Enqueue(log);
            Debug.WriteLine($"Add: {log.Message}");
            return true;
        }

        public bool Remove(Log log)
        {
            _logs.TryDequeue(out var removedLog);
            Debug.WriteLine($"Remove: {log.Message} / {removedLog.Message}");
            return true;
        }

        public Log GetLog()
        {
            _logs.TryPeek(out var log);
            Debug.WriteLine($"Get: {log.Message}");
            return log;
        }
    }
}
