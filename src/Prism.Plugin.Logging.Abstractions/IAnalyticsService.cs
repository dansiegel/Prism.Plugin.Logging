using System.Collections.Generic;

namespace Prism.Logging
{
    public interface IAnalyticsService
    {
        void TrackEvent(string name, IDictionary<string, string> properties);
    }
}
