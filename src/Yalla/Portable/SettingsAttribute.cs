using System;

namespace Yalla
{
    /// <summary>
    /// Specifies the type of the settings object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SettingsAttribute : Attribute
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.SettingsAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of the settings object.</param>
        public SettingsAttribute(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Gets the type of the settings object.
        /// </summary>
        /// <value>The type of the settings object.</value>
        public Type Type
        {
            get { return _type; }
        }
    }
}
