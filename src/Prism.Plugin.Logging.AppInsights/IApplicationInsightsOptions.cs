using System.Collections.Generic;

namespace Prism.Logging.AppInsights
{
    public interface IApplicationInsightsOptions
    {
        string InstrumentationKey { get; }

        IDictionary<string, string> UserTraits { get; }
    }
}
