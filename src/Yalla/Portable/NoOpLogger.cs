using System;

namespace Yalla
{
    class NoOpLogger : LoggerBase
    {
        private static readonly NoOpLogger _instance = new NoOpLogger();

        public static NoOpLogger Instance
        {
            get { return _instance; }
        }

        private NoOpLogger()
            : base(null)
        {
        }

        public override void Log(LogEntry entry, string message, IFormatProvider formatProvider)
        {
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }
    }

    /// <summary>
    /// No-op logger factory adapter.
    /// </summary>
    public class NoOpLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        private static readonly NoOpLoggerFactoryAdapter _instance = new NoOpLoggerFactoryAdapter();

        /// <summary>
        /// Gets the instance of the <see cref="Yalla.NoOpLoggerFactoryAdapter"/> class.
        /// </summary>
        public static NoOpLoggerFactoryAdapter Instance
        {
            get { return NoOpLoggerFactoryAdapter._instance; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.NoOpLoggerFactoryAdapter"/> class.
        /// </summary>
        public NoOpLoggerFactoryAdapter()
        {
        }

        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        public void Initialize(string prologue)
        {
        }

        /// <summary>
        /// Terminates the logging system.
        /// </summary>
        public void Shutdown(string epilogue)
        {
        }

        /// <summary>
        /// Gets a logger instance by name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public ILogger GetLogger(string name)
        {
            return NoOpLogger.Instance;
        }

        /// <summary>
        /// Gets a logger instance by type.
        /// </summary>
        /// <param name="type">The type to create the logger for.</param>
        public ILogger GetLogger(Type type)
        {
            return NoOpLogger.Instance;
        }
    }
}
