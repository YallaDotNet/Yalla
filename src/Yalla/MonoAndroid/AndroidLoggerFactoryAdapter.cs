using System.Collections.Generic;

namespace Yalla
{
	/// <summary>
	/// Android logger factory adapter.
	/// </summary>
	public class AndroidLoggerFactoryAdapter : LoggerFactoryAdapterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Yalla.AndroidLoggerFactoryAdapter"/> class.
		/// </summary>
		public AndroidLoggerFactoryAdapter()
		{
		}

		/// <summary>
		/// Initializes the logging system.
		/// </summary>
		public override void Initialize(string prologue)
		{
		}


		/// <summary>
		/// Terminates the logging system.
		/// </summary>
		public override void Shutdown(string epilogue)
		{
		}

		/// <summary>
		/// Gets a logger instance by name.
		/// </summary>
		/// <param name="name">The name of the logger.</param>
		public override ILogger GetLogger(string name)
		{
			return new AndroidLogger(name);
		}
	}
}

