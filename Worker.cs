using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paralization
{

    /// <summary>
    /// Represents a worker that consumes tasks from the scheduler and executes them safely.
    /// </summary>
    internal class Worker
    {

        private readonly TaskSchedulerService _scheduler;
        private readonly FileLockManager _lockManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="scheduler">The task scheduler providing tasks.</param>
        /// <param name="lockManager">The file lock manager to ensure thread-safe writes.</param>
        public Worker(TaskSchedulerService scheduler, FileLockManager lockManager)
        {
            _scheduler = scheduler;
            _lockManager = lockManager;
        }

        /// <summary>
        /// Runs the worker asynchronously, consuming tasks and executing them.
        /// </summary>
        /// <param name="token">Cancellation token to allow graceful shutdown.</param>
        public async Task RunAsync(CancellationToken token)
        {
            try
            {
                foreach (var task in _scheduler.HighPriority.GetConsumingEnumerable())
                {
                    var fileLock = _lockManager.GetLock(task.FilePath);

                    try
                    {
                        if (task.Type == TaskType.Upload)
                        {
                            fileLock.EnterWriteLock();
                            File.AppendAllText(
                                task.FilePath,
                                (task.Content ?? "DEFAULT") + Environment.NewLine
                            );
                            Logger.Log($"Uploaded content to {task.FilePath}: {task.Content}");
                        }

                        await Task.Delay(200);
                    }
                    finally
                    {
                        if (fileLock.IsWriteLockHeld) fileLock.ExitWriteLock();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Worker error: " + ex.Message);
            }
        }
    }
}
