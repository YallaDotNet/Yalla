using System;
using System.Text;

namespace Yalla
{
	/// <summary>
	/// Base log formatter.
	/// </summary>
    public abstract class FormatterBase : ILogFormatter
    {
		/// <summary>
		/// Formats an entry.
		/// </summary>
		/// <param name="provider">Format provider.</param>
		/// <param name="entry">The entry.</param>
        public string Format(IFormatProvider provider, LogEntry entry)
        {
            var builder = new StringBuilder(Prefix);
            return AppendEntry(builder, provider, entry)
                .Append(Suffix)
                .ToString();
        }

		/// <summary>
		/// Gets the prologue.
		/// </summary>
        public abstract string Prologue
        {
            get;
        }

		/// <summary>
		/// Gets the epilogue.
		/// </summary>
        public abstract string Epilogue
        {
            get;
        }

		/// <summary>
		/// Gets the message prefix.
		/// </summary>
        protected abstract string Prefix
        {
            get;
        }

		/// <summary>
		/// Gets the message suffix.
		/// </summary>
        protected abstract string Suffix
        {
            get;
        }

		/// <summary>
		/// Appends the entry.
		/// </summary>
		/// <param name="builder">String builder.</param>
		/// <param name="provider">Format provider.</param>
		/// <param name="entry">The entry.</param>
        /// <returns>The <see cref="StringBuilder"/> used to format the entry.</returns>
        protected abstract StringBuilder AppendEntry(StringBuilder builder, IFormatProvider provider, LogEntry entry);
    }
}
