using System.Configuration;
using System.Xml;

namespace Yalla.Configuration
{
    /// <summary>
    /// Yalla configuration section handler.
    /// </summary>
    public class ConfigurationSectionHandler : ConfigurationSection
    {
        /// <summary>
        /// Yalla configuration section name.
        /// </summary>
        public const string SectionName = "yalla";
        
        /// <summary>
        /// Adapter configuration property name.
        /// </summary>
        public const string AdapterPropertyName = "factoryAdapter";

        /// <summary>
        /// Formatter configuration property name.
        /// </summary>
        public const string FormatterPropertyName = "formatter";

        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationSectionHandler"/> class.
        /// </summary>
        public ConfigurationSectionHandler()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationSectionHandler"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public ConfigurationSectionHandler(XmlReader reader)
        {
            DeserializeSection(reader);
        }

        /// <summary>
        /// Gets or sets the value of the adapter configuration property.
        /// </summary>
        [ConfigurationProperty(AdapterPropertyName)]
        public AdapterConfigurationElement Adapter
        {
            get { return (AdapterConfigurationElement)base[AdapterPropertyName]; }
            set { base[AdapterPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the formatter configuration property.
        /// </summary>
        [ConfigurationProperty(FormatterPropertyName)]
        public FormatterConfigurationElement Formatter
        {
            get { return (FormatterConfigurationElement)base[FormatterPropertyName]; }
            set { base[FormatterPropertyName] = value; }
        }
    }
}
