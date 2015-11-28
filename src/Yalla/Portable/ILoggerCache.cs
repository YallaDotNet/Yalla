using System;

namespace Yalla
{
    /// <summary>
    /// Logger cache interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    public interface ILoggerCache<TKey, TLogger> : IDisposable
    {
        /// <summary>
        /// Gets or adds a logger.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="createLogger">Logger factory method.</param>
        /// <returns>Logger.</returns>
        TLogger GetOrAdd(TKey key, Func<TKey, TLogger> createLogger);
    }
}
