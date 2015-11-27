using System;
using System.ComponentModel;
using System.Globalization;

namespace Yalla
{
    class ConsoleLoggerSettingsConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(ConsoleLoggerFactoryAdapter))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var settings = value as ConsoleLoggerSettings;
            if (settings != null)
                return new ConsoleLoggerFactoryAdapter(settings);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
