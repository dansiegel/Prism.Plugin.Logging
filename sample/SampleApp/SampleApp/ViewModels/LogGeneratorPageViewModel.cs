using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using SampleApp.Collections;
using System;
using System.Collections.Generic;

namespace SampleApp.ViewModels
{
    public class LogGeneratorPageViewModel : BindableBase
    {
        private ILogger _logger;

        public LogGeneratorPageViewModel(ILogger logger)
        {
            _logger = logger;
            Properties = new ObservableDictionary<string, string>();

            TestAnalyticsCommand = new DelegateCommand(OnTestAnalyticsCommandExecuted);
            TestCrashCommand = new DelegateCommand(OnTestCrashCommandExecuted);
            TestLogCommand = new DelegateCommand(OnTestLogCommandExecuted);
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
                throw;
            }
        }

        private void OnTestLogCommandExecuted()
        {
            _logger.Log(Message);
        }
    }
}
