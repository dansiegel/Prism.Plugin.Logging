using System.Collections.Generic;

namespace Prism.Logging.AppInsights
{
    public class ApplicationInsightsOptions : IApplicationInsightsOptions
    {
        public string InstrumentationKey { get; set; }
        public IDictionary<string, string> UserTraits { get; set; }
    }
}
