using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paralization
{

    /// <summary>
    /// Adds entries to the log
    /// </summary>
    internal class Logger
    {
        private static readonly object _lock = new();

        /// <summary>
        /// Logs a message with timestamp to log.txt in a thread-safe way.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            lock (_lock)
            {
                string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
                File.AppendAllText("log.txt", logLine + Environment.NewLine);
            }
        }
    }
}
