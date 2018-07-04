using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Logging.AppInsights
{
    public interface IApplicationInsightsOptions
    {
        string InstrumentationKey { get; }

        IDictionary<string, string> UserTraits { get; }
    }
}
