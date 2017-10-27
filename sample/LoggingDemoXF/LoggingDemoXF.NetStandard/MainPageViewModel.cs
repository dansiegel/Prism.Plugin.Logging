using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Prism.Logging;
using Prism.Logging.Logger;
using Prism.Logging.Loggly;
using Prism.Logging.TestsHelpers;
using Xamarin.Forms;

namespace LoggingDemoXF
{
    public class MainPageViewModel:INotifyPropertyChanged
    {
        private static string LogglyTokenKey = "LogglyToken";

        private ILoggerFacade _logger;

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                {
                    return;
                }
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _logglyToken;
        public string LogglyToken
        {
            get => _logglyToken;
            set
            {
                if (_logglyToken == value)
                {
                    return;
                }
                _logglyToken = value;
                OnPropertyChanged();
                ((Command)CreateLoggerCommand).ChangeCanExecute();
            }
        }


        public ICommand SendLogCommand { get; private set; }
        public ICommand CreateLoggerCommand { get; private set; }


        public MainPageViewModel()
        {
            SendLogCommand = new Command(SendLog, () =>
            {
                return _logger != null;
            });

            CreateLoggerCommand=new Command(CreateLogger, () =>
            {
                return _logger == null && !string.IsNullOrWhiteSpace(LogglyToken);
            });

            if (Application.Current.Properties.TryGetValue(LogglyTokenKey, out var token))
            {
                LogglyToken = (string)token;
            }
        }

        private void CreateLogger()
        {

            Application.Current.Properties[LogglyTokenKey] = LogglyToken;


            _logger = new NetworkResilienceLogger(new LogglyHttpLogger(GetLogglyOptions()), new NotPersistentLogsRepository());

            ((Command)SendLogCommand).ChangeCanExecute();
            ((Command)CreateLoggerCommand).ChangeCanExecute();
        }

        private ILogglyOptions GetLogglyOptions() =>
            new LogglyOptions
            {
                AppName = "LoggingDemoXF",
                Token = LogglyToken
            };

        private void SendLog()
        {
            _logger.Log(Message, Category.Debug, Priority.High);
            Message = String.Empty;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
