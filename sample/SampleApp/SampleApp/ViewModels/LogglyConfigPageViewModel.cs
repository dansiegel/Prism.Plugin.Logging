using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using Prism.Events;
using SampleApp.Events;
using SampleApp.Services;

namespace SampleApp.ViewModels
{
    public class LogglyConfigPageViewModel : ConfigViewModelBase<ILogglyConfig>
    {
        public LogglyConfigPageViewModel(ILogglyConfig config, IEventAggregator eventAggregator)
            : base(config, eventAggregator)
        {
            LoggerType = LoggerType.LogglyHttp;
            AddTagCommand = new DelegateCommand<string>(OnAddTagCommandExecuted);
        }

        public string[] Modes { get; } = new string[] { "Http", "Syslog" };

        private string _selectedMode = "Http";
        public string SelectedMode
        {
            get => _selectedMode;
            set => SetProperty(ref _selectedMode, value, () => LoggerType = value == "Http" ? LoggerType.LogglyHttp : LoggerType.LogglySyslog);
        }

        public DelegateCommand<string> AddTagCommand { get; }

        private void OnAddTagCommandExecuted(string tag)
        {
            Config.Tags.Add(tag);
        }
    }
}
