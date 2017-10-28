using System;
using System.Collections.Generic;
using System.Text;
using Prism.Logging.Logger;
using Xamarin.Forms;

namespace Prism.Plugin.Logging.XamarinLogsRepository
{
    public class XamarinPropertiesStorage : IPlatformStringStorage
    {
        private const string UnsentLogKey = "UnsentLogs";

        public string Read()
        {
            if (Application.Current.Properties.TryGetValue(UnsentLogKey, out var data))
            {
                return (string) data;
            }

            return String.Empty;
        }

        public bool Write(string data)
        {
            Application.Current.Properties[UnsentLogKey] = data;

            Application.Current.SavePropertiesAsync().Wait();

            return true;
        }
    }
}
