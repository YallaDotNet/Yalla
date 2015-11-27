using System.IO;

namespace Yalla
{
    /// <summary>
    /// Adds extension methods to <see cref="CallerInfo"/>.
    /// </summary>
    public static class CallerInfoExtensions
    {
        /// <summary>
        /// Returns the file name and extension for the specified caller.
        /// </summary>
        /// <param name="callerInfo">Compiler-populated caller information.</param>
        public static string GetFileName(this CallerInfo callerInfo)
        {
            return Path.GetFileName(callerInfo.FilePath);
        }
    }
}
