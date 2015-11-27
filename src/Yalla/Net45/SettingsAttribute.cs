using System;

namespace Yalla
{
    /// <summary>
    /// Specifies the type of the settings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SettingsAttribute : Attribute
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of <see cref="Yalla.SettingsAttribute"/>.
        /// </summary>
        /// <param name="type">The type of the settings.</param>
        public SettingsAttribute(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// The type of the settings.
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }
    }
}
