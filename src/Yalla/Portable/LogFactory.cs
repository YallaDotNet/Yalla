using System;

namespace Yalla
{
	/// <summary>
	/// Log factory.
	/// </summary>
    public partial class LogFactory : ILogFactory
    {
        private readonly ILoggerCache<string, ILog> _loggerCache = new LoggerCache<string, ILog>();
        private ILoggerFactoryAdapter _adapter;
        private ILogFormatter _formatter;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.LogFactory"/> class.
		/// </summary>
        public LogFactory()
            : this(NoOpLoggerFactoryAdapter.Instance)
        {
        }

		/// <summary>
        /// Initializes a new instance of the <see cref="Yalla.LogFactory"/> class.
		/// </summary>
		/// <param name="adapter">Logger factory adapter.</param>
        public LogFactory(ILoggerFactoryAdapter adapter)
            : this(adapter, DefaultFormatter.Instance)
        {
        }

		/// <summary>
        /// Initializes a new instance of the <see cref="Yalla.LogFactory"/> class.
		/// </summary>
		/// <param name="adapter">Logger factory adapter.</param>
		/// <param name="formatter">Log formatter.</param>
        public LogFactory(ILoggerFactoryAdapter adapter, ILogFormatter formatter)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter");
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            _adapter = adapter;
            _formatter = formatter;
        }

		/// <summary>
		/// Initializes the logging system.
		/// </summary>
        public void Initialize()
        {
            Adapter.Initialize(Formatter.Prologue);
        }

		/// <summary>
		/// Terminates the logging system.
		/// </summary>
        public void Shutdown()
        {
            Adapter.Shutdown(Formatter.Epilogue);
            LoggerCache.Dispose();
        }

		/// <summary>
		/// Retrieves or creates a named log.
		/// </summary>
		/// <param name="name">The name of the log to retrieve.</param>
        ILog ILogFactory.GetLogger(string name)
        {
            return LoggerCache.GetOrAdd(name, CreateLogger);
        }

		/// <summary>
		/// Retrieves or creates a named log.
		/// </summary>
		/// <param name="type">The type to retrieve the log for.</param>
        public ILog GetLogger(Type type)
        {
            return LoggerCache.GetOrAdd(type.FullName, _ => CreateLogger(type));
        }

		/// <summary>
		/// Gets or sets the logger factory adapter.
		/// </summary>
        public ILoggerFactoryAdapter Adapter
        {
            get { return _adapter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _adapter = value;
            }
        }

		/// <summary>
		/// Gets or sets the log formatter.
		/// </summary>
        public ILogFormatter Formatter
        {
            get { return _formatter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _formatter = value;
            }
        }

        private ILog CreateLogger(string name)
        {
            var logger = Adapter.GetLogger(name);
            return new Log(logger, Formatter);
        }

        private ILog CreateLogger(Type type)
        {
            var logger = Adapter.GetLogger(type);
            return new Log(logger, Formatter);
        }

        /// <summary>
        /// Gets the logger cache.
        /// </summary>
        private ILoggerCache<string, ILog> LoggerCache
        {
            get { return _loggerCache; }
        }
    }
}
