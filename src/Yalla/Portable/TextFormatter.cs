using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Yalla
{
    /// <summary>
    /// Text formatter settings.
    /// </summary>
    public class TextFormatterSettings
    {
        /// <summary>
        /// Message format.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Exception message format.
        /// </summary>
        public string ExceptionMessage
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Text formatter.
    /// </summary>
    public class TextFormatter : DefaultFormatter
    {
        private delegate string GetSubstringDelegate(IFormatProvider provider, IDictionary<string, object> properties);

        private TextFormatterSettings _settings;
        private IEnumerable<GetSubstringDelegate> _messageSubs;
        private IEnumerable<GetSubstringDelegate> _exceptionMessageSubs;

        /// <summary>
        /// Appends an entry.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="provider">Format provider.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>The <see cref="StringBuilder"/> used to format the entry.</returns>
        protected override StringBuilder AppendEntry(StringBuilder builder, IFormatProvider provider, LogEntry entry)
        {
            FillProperties(entry);
            return base.AppendEntry(builder, provider, entry);
        }

        /// <summary>
        /// Appends the message.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="provider">Format provider.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>The <see cref="StringBuilder"/> used to format the entry.</returns>
        protected override StringBuilder AppendMessage(StringBuilder builder, IFormatProvider provider, LogEntry entry)
        {
            if (string.IsNullOrEmpty(Settings.Message))
                return base.AppendMessage(builder, provider, entry);
            return Append(builder, provider, entry, Settings.Message, _messageSubs);
        }

        /// <summary>
        /// Appends the exception.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="provider">Format provider.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>The <see cref="StringBuilder"/> used to format the entry.</returns>
        protected override StringBuilder AppendException(StringBuilder builder, IFormatProvider provider, LogEntry entry)
        {
            if (string.IsNullOrEmpty(Settings.ExceptionMessage))
                return base.AppendException(builder, provider, entry);
            return Append(builder, provider, entry, Settings.ExceptionMessage, _exceptionMessageSubs);
        }

        /// <summary>
        /// Gets or sets the text formatter settings.
        /// </summary>
        public TextFormatterSettings Settings
        {
            get { return _settings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _settings = value;
                _messageSubs = GetSubstringDelegates(Settings.Message);
                _exceptionMessageSubs = GetSubstringDelegates(Settings.ExceptionMessage);
            }
        }

        private static StringBuilder Append(StringBuilder builder, IFormatProvider provider, LogEntry entry,
            string format, IEnumerable<GetSubstringDelegate> appends)
        {
            foreach (var getValue in appends)
            {
                var value = getValue(provider, entry.Properties);
                builder.Append(value);
            }
            return builder;
        }

        private static void FillProperties(LogEntry entry)
        {
            entry.Properties = new Dictionary<string, object>
            {
                { "timestamp", entry.Timestamp },
                { "logger", entry.Name },
                { "level", entry.Level },
                { "message", entry.Message },
            };

            if (entry.Caller != null)
            {
                entry.Properties.Add("callerInfo", entry.Caller);
                entry.Properties.Add("filePath", entry.Caller.FilePath);
                entry.Properties.Add("fileName", entry.Caller.GetFileName());
                entry.Properties.Add("method", entry.Caller.MemberName);
                entry.Properties.Add("line", entry.Caller.LineNumber);
            }

            if (entry.Exception != null)
            {
                entry.Properties.Add("exception", entry.Exception);
                entry.Properties.Add("exceptionType", entry.Exception.GetType());
                entry.Properties.Add("exceptionMessage", entry.Exception.Message);
                entry.Properties.Add("exceptionStackTrace", entry.Exception.StackTrace);
                entry.Properties.Add("innerException", entry.Exception.InnerException);
            }
        }

        private static IEnumerable<GetSubstringDelegate> GetSubstringDelegates(string format)
        {
            if (format != null)
            {
                while (format.Length > 0)
                {
                    var match = Regex.Match(format, @"\$\{([^:}]+)(:([^}]+))?\}");
                    var index = match.Success
                        ? match.Index
                        : format.Length;
                    if (index > 0)
                    {
                        var substring = format.Substring(0, index);
                        yield return (provider, properties) => substring;
                        format = format.Substring(index);
                    }
                    if (match.Success)
                    {
                        yield return (provider, properties) =>
                            GetValueSubstring(provider, match.Groups[1].Value, match.Groups[3], properties);
                        format = format.Substring(match.Length);
                    }
                }
            }
        }

        private static string GetValueSubstring(IFormatProvider provider, string name, Group group, IDictionary<string, object> properties)
        {
            object value;
            if (!properties.TryGetValue(name, out value) || value == null)
                return null;
            return group.Success
                ? string.Format(provider, "{0:" + group.Value + "}", value)
                : string.Format(provider, "{0}", value);
        }
    }
}
