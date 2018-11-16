using Prism.Events;
using Prism.Mvvm;
using SampleApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
