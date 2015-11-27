using System.IO;
using System;

namespace Yalla
{
    /// <summary>
    /// Base writer logger.
    /// </summary>
    /// <typeparam name="TSettings">The type of the logger settings.</typeparam>
    public abstract class WriterLoggerBase<TSettings> : LoggerBase<string, TSettings>
        where TSettings : LoggerSettings
    {
        private readonly TextWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.WriterLoggerBase{TSettings}"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="settings">Logger settings.</param>
        /// <param name="writer">Text writer.</param>
        protected WriterLoggerBase(string name, TSettings settings, TextWriter writer)
            : base(name, settings)
        {
            _writer = writer;
        }

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="message">The message.</param>
        /// <param name="provider">Format provider.</param>
        protected override void Log(string level, LogEntry entry, string message, IFormatProvider provider)
        {
            _writer.Write(message);
        }

        /// <summary>
        /// Gets the concrete log level.
        /// </summary>
        /// <param name="logLevel">Yalla log level.</param>
        protected override string GetLevel(LogLevel logLevel)
        {
            return logLevel.ToString();
        }
    }
}
