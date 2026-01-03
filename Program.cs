namespace Paralization
{
    internal class Program
    {

        /// <summary>
        /// Main program entry point. Initializes scheduler, workers, and handles user input.
        /// </summary>
        static void Main(string[] args)
        {
            var scheduler = new TaskSchedulerService();
            var lockManager = new FileLockManager();
            var cts = new CancellationTokenSource();

            var workers = new List<Task>();

            for (int i = 0; i < 2; i++)
            {
                var worker = new Worker(scheduler, lockManager);
                workers.Add(
                    Task.Run(() => worker.RunAsync(cts.Token))
                );
            }

            Console.WriteLine("Enter text (ENTER = end):");

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    break;

                scheduler.HighPriority.Add(
                    new FileTask("data.txt", TaskType.Upload, input)
                );
            }

            scheduler.HighPriority.CompleteAdding();
            Task.WaitAll(workers.ToArray());

            Console.WriteLine("Program ended.");
        }
    }
}
