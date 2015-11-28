using System;
using System.Collections.Generic;

namespace Yalla
{
    /// <summary>
    /// Logger cache.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    public sealed partial class LoggerCache<TKey, TLogger> : ILoggerCache<TKey, TLogger>
        where TLogger : class
    {
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="Yalla.LoggerCache{TKey,TValue}"/>
        /// is reclaimed by garbage collection.
        /// </summary>
        ~LoggerCache()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets or adds a logger.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="createLogger">Logger factory method.</param>
        /// <returns>Logger.</returns>
        public TLogger GetOrAdd(TKey key, Func<TKey, TLogger> createLogger)
        {
            var loggerRef = GetLoggerRef(key, createLogger);
            return GetLogger(key, createLogger, loggerRef);
        }

        private static void Dispose(IDictionary<TKey, WeakReference> cache)
        {
            foreach (var loggerRef in cache.Values)
            {
                var logger = loggerRef.Target as IDisposable;
                if (logger != null)
                    logger.Dispose();
            }
            cache.Clear();
        }

        private static TLogger GetLogger(TKey key, Func<TKey, TLogger> createLogger, WeakReference loggerRef)
        {
            var logger = loggerRef.Target as TLogger;
            if (logger == null)
            {
                lock (loggerRef)
                {
                    logger = loggerRef.Target as TLogger;
                    if (logger == null)
                    {
                        logger = createLogger(key);
                        loggerRef.Target = logger;
                    }
                }
            }
            return logger;
        }
    }
}
