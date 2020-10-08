using Prism.Events;
using SampleApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.ViewModels
{
    public class GelfConfigPageViewModel : ConfigViewModelBase<IGelfConfig>
    {
        public GelfConfigPageViewModel(IGelfConfig config, IEventAggregator eventAggregator)
            : base(config, eventAggregator)
        {
            LoggerType = Events.LoggerType.Graylog;
        }
    }
}
