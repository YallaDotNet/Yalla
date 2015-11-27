using Android.Util;
using Java.Lang;

namespace Yalla
{
	class AndroidLogger : LoggerBase<LogPriority>
	{
		public AndroidLogger(string name)
			: base(name)
		{
		}

        protected override void Log(LogPriority level, LogEntry entry, string message)
        {
            if (entry.Exception == null)
                global::Android.Util.Log.WriteLine(level, Name, message);
            else
                Log(level, entry.Exception, message);
        }

		protected override bool IsEnabled(LogPriority level)
		{
			return global::Android.Util.Log.IsLoggable(Name, level);
		}

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
}
