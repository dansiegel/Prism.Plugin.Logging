using System;

namespace Prism.Logging.Graylog
{
    public class GelfLogger : IGelfLogger
    {
        public void Log(string message, Level level = Level.Debug)
        {
            throw new NotImplementedException();
        }

        public void Log(GelfMessage message)
        {
            throw new NotImplementedException();
        }
    }
}