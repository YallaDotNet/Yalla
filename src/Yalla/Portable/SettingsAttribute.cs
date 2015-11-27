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
        /// Creates a new instance of <see cref="Yalla.SettingsAttribute"/>.
        /// </summary>
        /// <param name="type">The type of the settings object.</param>
        public SettingsAttribute(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// The type of the settings object.
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }
    }
}
