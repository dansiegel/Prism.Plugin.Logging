using Prism.Events;
using SampleApp.Services;

namespace SampleApp.ViewModels
{
    public class AppInsightsConfigPageViewModel : ConfigViewModelBase<IAppInsightsConfig>
    {
        public AppInsightsConfigPageViewModel(IAppInsightsConfig config, IEventAggregator eventAggregator)
            : base(config, eventAggregator)
        {
            LoggerType = Events.LoggerType.AppInsights;
        }
    }
}
