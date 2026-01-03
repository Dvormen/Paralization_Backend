using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paralization
{

    /// <summary>
    /// Manages two priority queues for file tasks and provides methods for consuming tasks.
    /// </summary>
    internal class TaskSchedulerService
    {

        /// <summary>
        /// High-priority task queue.
        /// </summary>
        public BlockingCollection<FileTask> HighPriority { get; } = new(100);

        /// <summary>
        /// Low-priority task queue.
        /// </summary>
        public BlockingCollection<FileTask> LowPriority { get; } = new(100);

        /// <summary>
        /// Takes a task from the scheduler, preferring high-priority tasks.
        /// </summary>
        /// <param name="token">Cancellation token to support task cancellation.</param>
        /// <returns>A <see cref="FileTask"/> to process.</returns>
        public FileTask TakeTask(CancellationToken token)
        {
            FileTask task;
            if (HighPriority.TryTake(out task, 50, token))
                return task;

            return LowPriority.Take(token);
        }
    }
}
