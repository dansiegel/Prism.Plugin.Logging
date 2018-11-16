using Prism.Logging.Graylog;
using Prism.Mvvm;
using System;

namespace SampleApp.Services
{
    public class GelfConfig : BindableBase, IGelfConfig, IGelfOptions
    {
        private Uri _host;
        public Uri Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }
    }
}
