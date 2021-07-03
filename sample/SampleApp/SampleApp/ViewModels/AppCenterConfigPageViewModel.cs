using System;
using System.Collections.Generic;
using System.Text;
using Prism.Events;
using Prism.Mvvm;
using SampleApp.Services;

namespace SampleApp.ViewModels
{
    public class AppCenterConfigPageViewModel : ConfigViewModelBase<IAppCenterConfig>
    {
        public AppCenterConfigPageViewModel(IAppCenterConfig config, IEventAggregator eventAggregator)
            : base(config, eventAggregator)
        {
            LoggerType = Events.LoggerType.AppCenter;
        }
    }
}
