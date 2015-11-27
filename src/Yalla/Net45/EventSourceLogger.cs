using System;
#if NET_45
using System.Diagnostics.Tracing;
#else
using Microsoft.Diagnostics.Tracing;
#endif

namespace Yalla
{
    [EventSource(Name = "Yalla")]
    sealed class YallaEventSource : EventSource
    {
        [Event(1, Level=EventLevel.Critical)]
        public void Critical(int eventId, string message)
        {
            WriteEvent(eventId, message);
        }

        [Event(2, Level = EventLevel.Error)]
        public void Error(int eventId, string message)
        {
            WriteEvent(eventId, message);
        }

        [Event(3, Level = EventLevel.Warning)]
        public void Warning(int eventId, string message)
        {
            WriteEvent(eventId, message);
        }

        [Event(4, Level = EventLevel.Informational)]
        public void Informational(int eventId, string message)
        {
            WriteEvent(eventId, message);
        }

        [Event(5, Level = EventLevel.Verbose)]
        internal void Verbose(int eventId, string message)
        {
            WriteEvent(eventId, message);
        }
    }

    class EventSourceLogger : LoggerBase<EventLevel>
    {
        private readonly YallaEventSource _eventSource;

        public EventSourceLogger(string name)
            : base(name)
        {
            _eventSource = new YallaEventSource();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                _eventSource.Dispose();
        }

        protected override void Log(EventLevel level, LogEntry entry, string message, IFormatProvider provider)
        {
            var eventId = entry.GetHashCode();
            switch (level)
            {
                case EventLevel.Critical:
                    _eventSource.Critical(eventId, message);
                    break;
                case EventLevel.Error:
                    _eventSource.Error(eventId, message);
                    break;
                case EventLevel.Warning:
                    _eventSource.Warning(eventId, message);
                    break;
                case EventLevel.Informational:
                    _eventSource.Informational(eventId, message);
                    break;
                case EventLevel.Verbose:
                    _eventSource.Verbose(eventId, message);
                    break;
                default:
                    break;
            }
        }

        protected override bool IsEnabled()
        {
            return _eventSource.IsEnabled();
        }

        protected override bool IsEnabled(EventLevel level)
        {
            return _eventSource.IsEnabled(level, EventKeywords.None);
        }

        protected override EventLevel GetLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return EventLevel.Verbose;
                case LogLevel.Info:
                    return EventLevel.Informational;
                case LogLevel.Warn:
                    return EventLevel.Warning;
                case LogLevel.Error:
                    return EventLevel.Error;
                case LogLevel.Fatal:
                    return EventLevel.Critical;
                default:
                    return EventLevel.Verbose;
            }
        }
    }
}
