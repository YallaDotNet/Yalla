using Android.Util;
using Java.Lang;
using System;

namespace Yalla
{
    /// <summary>
    /// Android system logger.
    /// </summary>
	class SystemLogger : LoggerBase<LogPriority>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.SystemLogger"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
		public SystemLogger(string name)
			: base(name)
		{
		}

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="message">The message.</param>
        /// <param name="provider">Format provider.</param>
        protected override void Log(LogPriority level, LogEntry entry, string message, IFormatProvider provider)
        {
            if (entry.Exception == null)
                global::Android.Util.Log.WriteLine(level, Name, message);
            else
                Log(level, entry.Exception, message);
        }

        /// <summary>
        /// Gets a value indicating whether logging of entries of the specified level is enabled.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <value><c>true</c> if logging of entries of the specified level is enabled.</value>
		protected override bool IsEnabled(LogPriority level)
		{
			return global::Android.Util.Log.IsLoggable(Name, level);
		}

        /// <summary>
        /// Gets the concrete log level.
        /// </summary>
        /// <param name="logLevel">Yalla log level.</param>
		protected override LogPriority GetLevel(LogLevel logLevel)
		{
			switch (logLevel)
			{
				case LogLevel.Trace:
					return LogPriority.Verbose;
				case LogLevel.Debug:
					return LogPriority.Debug;
				case LogLevel.Info:
					return LogPriority.Info;
				case LogLevel.Warn:
					return LogPriority.Warn;
				case LogLevel.Error:
					return LogPriority.Error;
				case LogLevel.Fatal:
					return LogPriority.Assert;
				default:
					return LogPriority.Verbose;
			}
		}

        private void Log(LogPriority level, System.Exception exception, string message)
        {
            var throwable = Throwable.FromException(exception);
            switch (level)
            {
                case LogPriority.Verbose:
                    global::Android.Util.Log.Verbose(Name, throwable, message);
                    break;
                case LogPriority.Debug:
                    global::Android.Util.Log.Debug(Name, throwable, message);
                    break;
                case LogPriority.Info:
                    global::Android.Util.Log.Info(Name, throwable, message);
                    break;
                case LogPriority.Warn:
                    global::Android.Util.Log.Warn(Name, throwable, message);
                    break;
                case LogPriority.Error:
                    global::Android.Util.Log.Error(Name, throwable, message);
                    break;
                case LogPriority.Assert:
                    global::Android.Util.Log.Wtf(Name, throwable, message);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Android system logger factory adapter.
    /// </summary>
    public class SystemLoggerFactoryAdapter : LoggerFactoryAdapterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.SystemLoggerFactoryAdapter"/> class.
        /// </summary>
        public SystemLoggerFactoryAdapter()
        {
        }

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
            return new SystemLogger(name);
        }
    }
}
