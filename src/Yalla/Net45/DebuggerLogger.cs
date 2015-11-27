using System;
using System.Diagnostics;

namespace Yalla
{
    /// <summary>
    /// Debugger logger settings.
    /// </summary>
    public class DebuggerLoggerSettings : LoggerSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Yalla.DebuggerLogger"/> should include the log category.
        /// </summary>
        /// <value><c>true</c> to include the log category; otherwise, <c>false</c>.</value>
        public bool IncludeCategory
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Debugger logger.
    /// </summary>
    public class DebuggerLogger : LoggerBase<int, DebuggerLoggerSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.DebuggerLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="settings">Logger settings.</param>
        public DebuggerLogger(string name, DebuggerLoggerSettings settings)
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
        protected override void Log(int level, LogEntry entry, string message, IFormatProvider provider)
        {
            var category = IncludeCategory ? Name : null;
            Debugger.Log(level, category, message);
        }

        /// <summary>
        /// Gets a value indicating whether this logger is enabled.
        /// </summary>
        /// <value><c>true</c> if this logger is enabled.</value>
        protected override bool IsEnabled()
        {
            return Debugger.IsLogging();
        }

        /// <summary>
        /// Gets the concrete log level.
        /// </summary>
        /// <param name="logLevel">Yalla log level.</param>
        protected override int GetLevel(LogLevel logLevel)
        {
            return (int)logLevel;
        }

        private bool IncludeCategory
        {
            get { return Settings.IncludeCategory; }
        }
    }

    /// <summary>
    /// Debugger logger factory adapter.
    /// </summary>
    public class DebuggerLoggerFactoryAdapter : LoggerFactoryAdapterBase<DebuggerLoggerSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.DebuggerLoggerFactoryAdapter"/> class.
        /// </summary>
        public DebuggerLoggerFactoryAdapter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.DebuggerLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <param name="settings">Debugger logger settings.</param>
        public DebuggerLoggerFactoryAdapter(DebuggerLoggerSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="prologue">Prologue.</param>
        public override void Initialize(string prologue)
        {
            Debugger.Log(-1, null, prologue);
        }

        /// <summary>
        /// Terminates the logging system.
        /// </summary>
        /// <param name="epilogue">Epilogue.</param>
        public override void Shutdown(string epilogue)
        {
            Debugger.Log(-1, null, epilogue);
        }

        /// <summary>
        /// Gets a logger instance by name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>Logger.</returns>
        public override ILogger GetLogger(string name)
        {
            return new DebuggerLogger(name, Settings);
        }
    }
}
