using System.Collections.Specialized;
using System.Diagnostics;

namespace Yalla
{
    /// <summary>
    /// A trace listener which makes use of the Yalla library.
    /// </summary>
    public class YallaTraceListener : TraceListener
    {
        /// <summary>
        /// Creates a new instance of the <see cref="YallaTraceListener"/> class.
        /// </summary>
        public YallaTraceListener()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="YallaTraceListener"/> class.
        /// </summary>
        /// <param name="properties">Properties.</param>
        public YallaTraceListener(NameValueCollection properties)
        {
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            Write(message, string.Empty);
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void Write(string message, string category)
        {
            var log = LogManager.GetLogger(category);
            var entry = new LogEntry(log.Logger.Name, Level, message, null, null);
            log.Logger.Log(entry, message, null);
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            Write(message);
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void WriteLine(string message, string category)
        {
            Write(message, category);
        }

        /// <summary>
        /// Log level.
        /// </summary>
        public LogLevel Level
        {
            get;
            set;
        }
    }
}
