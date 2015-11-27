using Xamarin.Forms.Xaml;

namespace Yalla
{
    public class XamlConfigurationSource : IConfigurationSource
    {
        public void Configure()
        {
            var factory = new LogFactory().LoadFromXaml(typeof(Log));
            LogManager.Initialize(factory);
        }
    }
}
