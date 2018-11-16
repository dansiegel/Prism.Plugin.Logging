using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SampleApp.Events;

namespace SampleApp.ViewModels
{
    public class ConfigViewModelBase<T> : BindableBase
    {
        protected IEventAggregator EventAggregator { get; }

        public ConfigViewModelBase(T config, IEventAggregator eventAggregator)
        {
            Config = config;
            EventAggregator = eventAggregator;

            UpdateLoggerCommand = new DelegateCommand(OnUpdateLoggerCommandExecuted);
        }

        public T Config { get; }

        private LoggerType _loggerType;
        public LoggerType LoggerType
        {
            get => _loggerType;
            set => SetProperty(ref _loggerType, value);
        }

        public DelegateCommand UpdateLoggerCommand { get; }

        private void OnUpdateLoggerCommandExecuted()
        {
            EventAggregator.GetEvent<UpdateLoggerEvent>().Publish(LoggerType);
        }
    }
}
