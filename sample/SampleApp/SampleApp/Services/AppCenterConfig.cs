using Prism.Mvvm;

namespace SampleApp.Services
{
    public class AppCenterConfig : BindableBase, IAppCenterConfig
    {
        private string _secret;
        public string Secret
        {
            get => _secret;
            set => SetProperty(ref _secret, value);
        }
    }
}