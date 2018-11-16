using Prism.Logging.Syslog;
using Prism.Mvvm;

namespace SampleApp.Services
{
    public class SyslogConfig : BindableBase, ISyslogOptions, ISyslogConfig
    {
        private string _hostOrIp;
        public string HostNameOrIp
        {
            get => _hostOrIp;
            set => SetProperty(ref _hostOrIp, value);
        }

        private int? _port;
        public int? Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private string _appName;
        public string AppNameOrTag
        {
            get => _appName;
            set => SetProperty(ref _appName, value);
        }
    }
}
