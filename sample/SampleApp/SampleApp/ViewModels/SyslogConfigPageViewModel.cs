using Prism.Events;
using SampleApp.Services;

namespace SampleApp.ViewModels
{
    public class SyslogConfigPageViewModel : ConfigViewModelBase<ISyslogConfig>
    {
        public SyslogConfigPageViewModel(ISyslogConfig config, IEventAggregator eventAggregator)
            : base(config, eventAggregator)
        {
            LoggerType = Events.LoggerType.Syslog;
        }
    }
}
