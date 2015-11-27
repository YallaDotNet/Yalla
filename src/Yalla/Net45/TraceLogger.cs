using System;
using System.Diagnostics;

namespace Yalla
{
    /// <summary>
    /// Trace logger settings.
    /// </summary>
    public class TraceLoggerSettings : LoggerSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Yalla.TraceLogger"/> should include the log category.
        /// </summary>
        /// <value><c>true</c> to include the log category; otherwise, <c>false</c>.</value>
        public bool IncludeCategory
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Trace logger.
    /// </summary>
    public class TraceLogger : LoggerBase<string, TraceLoggerSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.TraceLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="settings">Logger settings.</param>
        public TraceLogger(string name, TraceLoggerSettings settings)
            : base(name, settings)
        {
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
            var category = IncludeCategory ? Name : null;
            Trace.Write(message, category);
        }

        /// <summary>
        /// Gets a value indicating whether this logger is enabled.
        /// </summary>
        /// <value><c>true</c> if this logger is enabled.</value>
        protected override bool IsEnabled()
        {
            return Trace.Listeners.Count > 0;
        }

        /// <summary>
        /// Gets the concrete log level.
        /// </summary>
        /// <param name="logLevel">Yalla log level.</param>
        protected override string GetLevel(LogLevel logLevel)
        {
            return logLevel.ToString();
        }

        private bool IncludeCategory
        {
            get { return Settings.IncludeCategory; }
        }
    }

    /// <summary>
    /// Trace logger factory adapter.
    /// </summary>
    public class TraceLoggerFactoryAdapter : LoggerFactoryAdapterBase<TraceLoggerSettings>
    {
        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="prologue">Prologue.</param>
        public override void Initialize(string prologue)
        {
            Trace.Write(prologue);
        }

        /// <summary>
        /// Terminates the logging system.
        /// </summary>
        /// <param name="epilogue">Epilogue.</param>
        public override void Shutdown(string epilogue)
        {
            Trace.Write(epilogue);
            Trace.Close();
        }

        /// <summary>
        /// Gets a logger instance by name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public override ILogger GetLogger(string name)
        {
            return new TraceLogger(name, Settings);
        }
    }
}
