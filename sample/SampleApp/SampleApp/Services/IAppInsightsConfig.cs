using System.Collections.Generic;

namespace SampleApp.Services
{
    public interface IAppInsightsConfig
    {
        string InstrumentationKey { get; set; }
        IDictionary<string, string> UserTraits { get; }
    }
}