using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Logging
{
    public interface ICrashesService
    {
        void Report(Exception ex, IDictionary<string, string> properties);
    }
}
