using System.Collections.Generic;
using Prism.Logging.AppInsights;
using Prism.Mvvm;
using SampleApp.Collections;

namespace SampleApp.Services
{
    public class AppInsightsConfig : BindableBase, IAppInsightsConfig, IApplicationInsightsOptions
    {
        private string _instrumentationKey;
        public string InstrumentationKey
        {
            get => _instrumentationKey;
            set => SetProperty(ref _instrumentationKey, value);
        }

        private readonly ObservableDictionary<string, string> _userTraits = new ObservableDictionary<string, string>();
        public IDictionary<string, string> UserTraits
        {
            get => _userTraits;
        }
    }
}
