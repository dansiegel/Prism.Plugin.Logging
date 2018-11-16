using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Xaml
{
    [ContentProperty(nameof(ResourceName))]
    [AcceptEmptyServiceProvider]
    public class EmbeddedImageExtension : IMarkupExtension<ImageSource>
    {
        public string ResourceName { get; set; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider)
        {
            var assembly = GetType().Assembly;
            var name = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(ResourceName, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(name)) return null;

            return ImageSource.FromStream(() => assembly.GetManifestResourceStream(name));
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => 
            ProvideValue(serviceProvider);
    }
}
