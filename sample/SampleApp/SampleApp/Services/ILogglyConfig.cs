using System.Collections.ObjectModel;

namespace SampleApp.Services
{
    public interface ILogglyConfig
    {
        string AppName { get; set; }
        ObservableCollection<string> Tags { get; }
        string Token { get; set; }
    }
}