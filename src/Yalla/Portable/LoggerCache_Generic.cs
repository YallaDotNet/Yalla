using System;
using System.Collections.Generic;

namespace Yalla
{
    partial class LoggerCache<TKey, TLogger>
    {
        private readonly object _syncRoot = new object();
        private readonly IDictionary<TKey, WeakReference> _cache = new Dictionary<TKey, WeakReference>();

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_syncRoot)
                {
                    Dispose(_cache);
                }
            }
        }

        private WeakReference GetLoggerRef(TKey key, Func<TKey, TLogger> createLogger)
        {
            WeakReference loggerRef;
            lock (_syncRoot)
            {
                if (!_cache.TryGetValue(key, out loggerRef))
                {
                    loggerRef = new WeakReference(createLogger(key));
                    _cache.Add(key, loggerRef);
                }
            }
            return loggerRef;
        }
    }
}
