using System;
using System.Runtime.CompilerServices;

namespace Yalla
{
    /// <summary>
    /// Logging facade.
    /// </summary>
    public sealed partial class LogManager
    {
        private static Lazy<ILogFactory> _factory;

        static LogManager()
        {
            SystemConfigurationSource.Default.Configure();
        }

        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="factory">Log factory.</param>
        public static void Initialize(ILogFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            _factory = new Lazy<ILogFactory>(() => InitializeFactory(factory));
        }

        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="adapter">Logger factory adapter.</param>
        public static void Initialize(ILoggerFactoryAdapter adapter)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter");
            _factory = new Lazy<ILogFactory>(() => InitializeFactory(adapter));
        }

        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="adapter">Logger factory adapter.</param>
        /// <param name="formatter">Log formatter.</param>
        public static void Initialize(ILoggerFactoryAdapter adapter, ILogFormatter formatter)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter");
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            _factory = new Lazy<ILogFactory>(() => InitializeFactory(adapter, formatter));
        }

        /// <summary>
        /// Terminates the logging system.
        /// </summary>
        public static void Shutdown()
        {
            Factory.Shutdown();
        }

        /// <summary>
        /// Retrieves or creates a named log.
        /// </summary>
        /// <param name="name">The name of the log to retrieve.</param>
        /// <returns>Log.</returns>
        public static ILog GetLogger(string name)
        {
            return Factory.GetLogger(name);
        }

        /// <summary>
        /// Retrieves or creates a named log.
        /// </summary>
        /// <param name="type">The type to retrieve the log for.</param>
        /// <returns>Log.</returns>
        public static ILog GetLogger(Type type)
        {
            return Factory.GetLogger(type);
        }

        /// <summary>
        /// Retrieves or creates a named log.
        /// </summary>
        /// <typeparam name="T">The type to retrieve the log for.</typeparam>
        /// <returns>Log.</returns>
        public static ILog GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        /// <summary>
        /// Retrieves or creates a log named after the caller source file.
        /// </summary>
        /// <param name="filePath">The name of the log to retrieve.</param>
        /// <returns>Log.</returns>
        public static ILog GetLoggerForCurrentFile([CallerFilePath] string filePath = null)
        {
            return GetLogger(filePath);
        }

        private static ILogFactory InitializeFactory(ILogFactory factory)
        {
            factory.Initialize();
            return factory;
        }

        private static ILogFactory InitializeFactory(ILoggerFactoryAdapter adapter)
        {
            var factory = new LogFactory(adapter);
            factory.Initialize();
            return factory;
        }

        private static ILogFactory InitializeFactory(ILoggerFactoryAdapter adapter, ILogFormatter formatter)
        {
            var factory = new LogFactory(adapter, formatter);
            factory.Initialize();
            return factory;
        }

        private static ILogFactory Factory
        {
            get
            {
                return _factory.Value;
            }
        }
    }
}
