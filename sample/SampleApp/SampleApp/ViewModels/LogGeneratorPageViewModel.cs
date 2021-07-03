using System;
using System.Collections.Generic;
using DryIoc;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using SampleApp.Collections;
using SampleApp.Events;

namespace SampleApp.ViewModels
{
    public class LogGeneratorPageViewModel : BindableBase
    {
        private ILogger _logger;

        public LogGeneratorPageViewModel(IEventAggregator eventAggregator, ILogger logger)
        {
            _logger = logger;
            eventAggregator.GetEvent<LoggerUpdatedEvent>().Subscribe(OnLoggerUpdated);
            Properties = new ObservableDictionary<string, string>();

            TestAnalyticsCommand = new DelegateCommand(OnTestAnalyticsCommandExecuted);
            TestCrashCommand = new DelegateCommand(OnTestCrashCommandExecuted);
            TestLogCommand = new DelegateCommand(OnTestLogCommandExecuted);
        }

        private void OnLoggerUpdated(ILogger logger)
        {
            _logger = logger;
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableDictionary<string, string> Properties { get; }

        public DelegateCommand TestAnalyticsCommand { get; }

        public DelegateCommand TestCrashCommand { get; }

        public DelegateCommand TestLogCommand { get; }

        private void OnTestAnalyticsCommandExecuted()
        {
            _logger.TrackEvent("User Defined Event");
        }

        private void OnTestCrashCommandExecuted()
        {
            try
            {
                throw new Exception("This is a test crash...");
            }
            catch (Exception ex)
            {
                _logger.Report(ex, new Dictionary<string, string> { { "caller", nameof(OnTestCrashCommandExecuted) } });
            }
        }

        private void OnTestLogCommandExecuted()
        {
            _logger.Log(Message);
        }
    }
}
