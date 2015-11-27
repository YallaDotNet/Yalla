using System;
using System.Collections.Concurrent;

namespace Yalla
{
    partial class LoggerCache<TKey, TLogger>
    {
        private readonly ConcurrentDictionary<TKey, WeakReference> _cache =
            new ConcurrentDictionary<TKey, WeakReference>();

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose(_cache);
            }
        }

        private WeakReference GetLoggerRef(TKey key, Func<TKey, TLogger> createLogger)
        {
            return _cache.GetOrAdd(key, k => new WeakReference(createLogger(k)));
        }
    }
}
