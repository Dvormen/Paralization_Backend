using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paralization
{
    /// <summary>
    /// Represents the type of file task to be executed.
    /// </summary>
    public enum TaskType
    {
        Upload,
        Index,
        Backup
    }

    /// <summary>
    /// Represents a file-related task with type, path, and optional content.
    /// </summary>
    internal class FileTask
    {
        public string FilePath { get; }
        public TaskType Type { get; }
        public string? Content { get; }

        /// <summary>
        /// Represents a file-related task with type, path, and optional content.
        /// </summary>
        public FileTask(string filePath, TaskType type, string? content)
        {
            FilePath = filePath;
            Type = type;
            Content = content;
        }
    }
}
