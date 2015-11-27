using System;
using System.IO;

namespace Yalla
{
    /// <summary>
    /// Console logger target.
    /// </summary>
    public enum ConsoleTargetType
    {
        /// <summary>
        /// Default target.
        /// </summary>
        Default = Out,

        /// <summary>
        /// Standard output.
        /// </summary>
        Out = 0,

        /// <summary>
        /// Error output.
        /// </summary>
        Error
    }

    /// <summary>
    /// Console logger settings.
    /// </summary>
    public class ConsoleLoggerSettings : LoggerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.ConsoleLoggerSettings"/> class.
        /// </summary>
        public ConsoleLoggerSettings()
        {
            Target = ConsoleTargetType.Default;
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>Target.</value>
        public ConsoleTargetType Target
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Console logger.
    /// </summary>
    public class ConsoleLogger : WriterLoggerBase<ConsoleLoggerSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="settings">Logger settings.</param>
        /// <param name="writer">Text writer.</param>
        public ConsoleLogger(string name, ConsoleLoggerSettings settings, TextWriter writer)
            : base(name, settings, writer)
        {
        }
    }

    /// <summary>
    /// Console logger factory adapter.
    /// </summary>
    public class ConsoleLoggerFactoryAdapter : LoggerFactoryAdapterBase<ConsoleLoggerSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.ConsoleLoggerFactoryAdapter"/> class.
        /// </summary>
        public ConsoleLoggerFactoryAdapter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yalla.ConsoleLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <param name="settings">Logger settings.</param>
        public ConsoleLoggerFactoryAdapter(ConsoleLoggerSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// Initializes the logging system.
        /// </summary>
        /// <param name="prologue">Prologue.</param>
        public override void Initialize(string prologue)
        {
            Writer.Write(prologue);
        }

        /// <summary>
        /// Terminates the logging system.
        /// </summary>
        /// <param name="epilogue">Epilogue.</param>
        public override void Shutdown(string epilogue)
        {
            Writer.Write(epilogue);
            Writer.Close();
        }

        /// <summary>
        /// Gets a logger instance by name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>Logger.</returns>
        public override ILogger GetLogger(string name)
        {
            return new ConsoleLogger(name, Settings, Writer);
        }

        private TextWriter Writer
        {
            get
            {
                switch (Target)
                {
                    case ConsoleTargetType.Error:
                        return Console.Error;
                    default:
                        return Console.Out;
                }
            }
        }

        private ConsoleTargetType Target
        {
            get { return Settings.Target; }
        }
    }
}
