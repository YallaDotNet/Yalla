using System;
using MonoMac.Darwin;

namespace Yalla
{
    /// <summary>
    /// iOS/OSX system logger.
    /// </summary>
	class SystemLogger : LoggerBase<string>
	{
		private readonly SystemLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.SystemLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
		public SystemLogger(string name)
			: base(name)
		{
			_log = !string.IsNullOrEmpty(name)
				? new SystemLog(string.Empty, name)
				: SystemLog.Default;
		}

        /// <summary>
        /// Releases all resource used by the <see cref="Yalla.SystemLogger"/> object.
        /// </summary>
		public override void Dispose()
		{
            base.Dispose();
			if (Name != null)
				_log.Dispose();
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
            using (var msg = new Message(Message.Kind.Message)
            {
                Level = level,
                Sender = Name,
                Msg = message,
            })
            {
                if (entry.Properties != null)
                {
                    foreach (var kvp in entry.Properties)
                    {
                        if (kvp.Value != null)
                        {
                            var key = kvp.Key;
                            switch (key)
                            {
                                case "logger":
                                case "level":
                                case "message":
                                    continue;
                                case "timestamp":
                                    key = "Time";
                                    continue;
                                default:
                                    break;
                            }
                            msg[key] = string.Format(provider, "{0}", kvp.Value);
                        }
                    }
                }
                _log.Log(msg);
            }
		}

        /// <summary>
        /// Gets a value indicating whether logging of entries of the specified level is enabled.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <returns><c>true</c> if logging of entries of the specified level is enabled.</returns>
		protected override bool IsEnabled(string level)
		{
			return true;
		}

        /// <summary>
        /// Gets the concrete log level.
        /// </summary>
        /// <param name="logLevel">Yalla log level.</param>
        /// <returns>Log level string.</returns>
        protected override string GetLevel(LogLevel logLevel)
		{
			return logLevel.ToString();
		}
	}

    /// <summary>
    /// iOS/OSX system logger factory adapter.
    /// </summary>
    public class SystemLoggerFactoryAdapter : LoggerFactoryAdapterBase
    {
        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="prologue">Prologue.</param>
        public override void Initialize(string prologue)
        {
        }

        /// <summary>
        /// Terminates the logging system.
        /// </summary>
        /// <param name="epilogue">Epilogue.</param>
        public override void Shutdown(string epilogue)
        {
        }

        /// <summary>
        /// Gets a logger instance by name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>Logger.</returns>
        public override ILogger GetLogger(string name)
        {
            name = name ?? string.Empty;
            return _loggers.GetOrAdd(name, n => new SystemLogger(n));
        }
    }
}
