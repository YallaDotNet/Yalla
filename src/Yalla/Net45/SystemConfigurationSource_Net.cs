using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Yalla.Configuration;

namespace Yalla
{
    partial class SystemConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.SystemConfigurationSource"/> class.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        public static SystemConfigurationSource FromSection(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName);
            return new SystemConfigurationSource(section);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.SystemConfigurationSource"/> class.
        /// </summary>
        /// <param name="exePath">The path of the executable (exe) file.</param>
        public static SystemConfigurationSource FromExeConfiguration(string exePath)
        {
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            return FromConfiguration(config, ConfigurationSectionHandler.SectionName);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Yalla.SystemConfigurationSource"/> class.
        /// </summary>
        /// <param name="exePath">The path of the executable (exe) file.</param>
        /// <param name="sectionName">Section name.</param>
        public static SystemConfigurationSource FromExeConfiguration(string exePath, string sectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            return FromConfiguration(config, sectionName);
        }

        private static SystemConfigurationSource FromConfiguration(System.Configuration.Configuration config, string sectionName)
        {
            var section = config.GetSection(sectionName);
            return new SystemConfigurationSource(section);
        }

        private Lazy<ConfigurationSectionHandler> _section =
            new Lazy<ConfigurationSectionHandler>(() => ConfigurationManager.GetSection(ConfigurationSectionHandler.SectionName) as ConfigurationSectionHandler);

        private SystemConfigurationSource(Func<ConfigurationSectionHandler> getSection)
        {
            _section = new Lazy<ConfigurationSectionHandler>(getSection);
        }

        private SystemConfigurationSource(object value)
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

        partial void DoConfigure()
        {
            if (Section == null)
                return;
            var adapter = CreateInstance<ILoggerFactoryAdapter>(Section.Adapter, "Adapter");
            if (adapter == null)
                return;
            var formatter = CreateInstance<ILogFormatter>(Section.Formatter, "Formatter");
            if (formatter != null)
                LogManager.Initialize(adapter, formatter);
            else
                LogManager.Initialize(adapter);
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
