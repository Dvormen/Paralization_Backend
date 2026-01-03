using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paralization
{

    /// <summary>
    /// Manages file locks to ensure thread-safe access to files.
    /// </summary>
    internal class FileLockManager
    {
        private readonly ConcurrentDictionary<string, ReaderWriterLockSlim> _locks = new();

        /// <summary>
        /// Gets a lock object for the specified file path. 
        /// If a lock does not exist, it will be created.
        /// </summary>
        /// <param name="filePath">The path of the file to lock.</param>
        /// <returns>A ReaderWriterLockSlim object for the file.</returns>
        public ReaderWriterLockSlim GetLock(string filePath)
        {
            return _locks.GetOrAdd(filePath, _ => new ReaderWriterLockSlim());
        }
    }
}
