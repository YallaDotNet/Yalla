using System.Xaml;

namespace Yalla
{
    /// <summary>
    /// XAML configuration source.
    /// </summary>
    public class XamlConfigurationSource : IConfigurationSource
    {
        private readonly string _path;

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.XamlConfigurationSource"/> class.
        /// </summary>
        /// <param name="path">The path to use as source.</param>
        public XamlConfigurationSource(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Configures the logging system.
        /// </summary>
        public void Configure()
        {
            var factory = XamlServices.Load(_path) as ILogFactory;
            LogManager.Initialize(factory);
        }
    }
}
