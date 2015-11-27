using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Yalla.Configuration;

namespace Yalla
{
    /// <summary>
    /// Default configuration source.
    /// </summary>
    public class DefaultConfigurationSource : IConfigurationSource
    {
        private static readonly DefaultConfigurationSource _instance =
            new DefaultConfigurationSource(() => ConfigurationManager.GetSection(ConfigurationSectionHandler.SectionName) as ConfigurationSectionHandler);

        /// <summary>
        /// Gets the default instance of the <see cref="Yalla.DefaultConfigurationSource"/> class.
        /// </summary>
        public static DefaultConfigurationSource Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.DefaultConfigurationSource"/> class.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        public static DefaultConfigurationSource FromSection(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName);
            return new DefaultConfigurationSource(section);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.DefaultConfigurationSource"/> class.
        /// </summary>
        /// <param name="sectionGroupName">Section group name.</param>
        /// <param name="sectionName">Section name.</param>
        public static DefaultConfigurationSource FromSection(string sectionGroupName, string sectionName)
        {
            var exePath = Assembly.GetEntryAssembly().Location;
            return FromExeConfiguration(exePath, sectionGroupName, sectionName);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.DefaultConfigurationSource"/> class.
        /// </summary>
        /// <param name="exePath">The path of the executable (exe) file.</param>
        public static DefaultConfigurationSource FromExeConfiguration(string exePath)
        {
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            return FromConfiguration(config, ConfigurationSectionHandler.SectionName);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.DefaultConfigurationSource"/> class.
        /// </summary>
        /// <param name="exePath">The path of the executable (exe) file.</param>
        /// <param name="sectionName">Section name.</param>
        public static DefaultConfigurationSource FromExeConfiguration(string exePath, string sectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            return FromConfiguration(config, sectionName);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.DefaultConfigurationSource"/> class.
        /// </summary>
        /// <param name="exePath">The path of the executable (exe) file.</param>
        /// <param name="sectionGroupName">Section group name.</param>
        /// <param name="sectionName">Section name.</param>
        public static DefaultConfigurationSource FromExeConfiguration(string exePath, string sectionGroupName, string sectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            return FromConfiguration(config, sectionGroupName, sectionName);
        }

        private static DefaultConfigurationSource FromConfiguration(System.Configuration.Configuration config, string sectionName)
        {
            var section = config.GetSection(sectionName);
            return new DefaultConfigurationSource(section);
        }

        private static DefaultConfigurationSource FromConfiguration(System.Configuration.Configuration config, string sectionGroupName, string sectionName)
        {
            var group = config.GetSectionGroup(sectionGroupName);
            var section = group.Sections.Get(sectionName);
            return new DefaultConfigurationSource(section);
        }

        private Lazy<ConfigurationSectionHandler> _section;

        private DefaultConfigurationSource(Func<ConfigurationSectionHandler> getSection)
        {
            _section = new Lazy<ConfigurationSectionHandler>(getSection);
        }

        private DefaultConfigurationSource(object value)
        {
            if (value == null)
                throw new InvalidOperationException("Section not found.");
            var section = value as ConfigurationSectionHandler;
            if (section == null)
            {
                var message = string.Format("Section type must be {0}.", typeof(ConfigurationSectionHandler).FullName);
                throw new InvalidOperationException(message);
            }
            _section = new Lazy<ConfigurationSectionHandler>(() => section);
        }

        /// <summary>
        /// Configures a log factory.
        /// </summary>
        public ILogFactory Configure()
        {
            if (Section == null)
                return NoOpLoggerFactory.Instance;
            return new LogFactory(
                CreateInstance<ILoggerFactoryAdapter>(Section.Adapter, "Adapter") ?? NoOpLoggerFactoryAdapter.Instance,
                CreateInstance<ILogFormatter>(Section.Formatter, "Formatter") ?? DefaultFormatter.Instance);
        }

        private T CreateInstance<T>(TypeConfigurationElement element, string displayName)
            where T : class
        {
            if (!element.IsPresent)
                return null;
            var typeName = element.GetPropertyValue(TypeConfigurationElement.TypePropertyName) as string;
            if (string.IsNullOrEmpty(typeName))
            {
                var message = string.Format("{0} type is required.", displayName);
                throw new InvalidOperationException(message);
            }
            var type = Type.GetType(typeName, true);
            var instance = Activator.CreateInstance(type) as T;
            if (instance == null)
            {
                var message = string.Format("{0}s must implement {1}.", displayName, typeof(T).FullName);
                throw new InvalidOperationException(message);
            }
            var property = type.GetProperty("Settings", BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                var settings = GetSettings(element, type, property.PropertyType);
                property.SetValue(instance, settings, null);
            }
            return instance;
        }

        private static object GetSettings(TypeConfigurationElement element, Type type, Type settingsType)
        {
            var settingsAttr = type.GetCustomAttribute<SettingsAttribute>();
            if (settingsAttr != null)
                settingsType = settingsAttr.Type;
            var settings = Activator.CreateInstance(settingsType);
            var properties = TypeDescriptor.GetProperties(settings)
                .Cast<PropertyDescriptor>()
                .Where(p => p.Name.ToLowerInvariant() != TypeConfigurationElement.TypePropertyName);
            foreach (var property in properties)
            {
                var propertyValue = GetPropertyValue(element, property.Name);
                if (propertyValue != null)
                {
                    var value = property.Converter.ConvertFrom(propertyValue);
                    property.SetValue(settings, value);
                }
            }
            return settings;
        }

        private static object GetPropertyValue(TypeConfigurationElement element, string name)
        {
            var property = element.ElementInformation.Properties
                .Cast<PropertyInformation>()
                .SingleOrDefault(p => p.Name.ToLowerInvariant() == name.ToLowerInvariant());
            return property != null
                ? property.Value
                : null;
        }

        private ConfigurationSectionHandler Section
        {
            get { return _section.Value as ConfigurationSectionHandler; }
        }
    }
}
