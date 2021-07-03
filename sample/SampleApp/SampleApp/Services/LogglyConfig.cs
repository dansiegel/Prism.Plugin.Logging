using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Logging.Loggly;
using Prism.Mvvm;

namespace SampleApp.Services
{
    public class LogglyConfig : BindableBase, ILogglyConfig, ILogglyOptions
    {
        private string _token;
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        private string _appName;
        public string AppName
        {
            get => _appName;
            set => SetProperty(ref _appName, value);
        }

        public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>();

        IEnumerable<string> ILogglyOptions.Tags => Tags;
    }
}
