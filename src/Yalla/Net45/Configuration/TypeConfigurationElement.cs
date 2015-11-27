using System.Configuration;
using System.Xml;

namespace Yalla.Configuration
{
    /// <summary>
    /// Base type configuration element.
    /// </summary>
    public abstract class TypeConfigurationElement : ConfigurationElement
    {
        private bool _isPresent;

        /// <summary>
        /// Type configuration property name.
        /// </summary>
        public const string TypePropertyName = "type";

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property, or <c>null</c> if the property is not present.</returns>
        public object GetPropertyValue(string name)
        {
            return base.Properties.Contains(name) ? base[name] : null;
        }

        /// <summary>
        /// Deserializes the configuration element.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> that reads from the configuration file.</param>
        /// <param name="serializeCollectionKey"><c>true</c> to serialize only the collection key properties; otherwise, <c>false</c>.</param>
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            for (var hasAttr = reader.MoveToFirstAttribute(); hasAttr; hasAttr = reader.MoveToNextAttribute())
            {
                var key = reader.LocalName;
                reader.ReadAttributeValue();
                Properties.Add(new ConfigurationProperty(key, typeof(string), reader.Value));
            }
            IsPresent = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this configuration element is present.
        /// </summary>
        /// <value><c>true</c> if the configuration element is present; otherwise, <c>false</c></value>
        public bool IsPresent
        {
            get { return _isPresent; }
            set { _isPresent = value; }
        }
    }
}
