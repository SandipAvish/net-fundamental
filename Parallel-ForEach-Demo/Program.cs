using System.Diagnostics;

namespace Parallel_ForEach_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            List<int> list = Enumerable.Range(1, 10).ToList();

            foreach (var i in list)
            {
                long total = DoSomeIndependentTimeConsumingTask();
                Console.WriteLine("{0} - {1}", i, total);
            }

            Console.WriteLine("Standard Foreach Loop Ended");
            sw.Stop();
            Console.WriteLine($"Time Taken by Standard Foreach Loop in Miliseconds {sw.ElapsedMilliseconds}");

            Console.WriteLine();

            Console.WriteLine("Parallel Foreach Loop Started");
            sw.Reset();
            sw.Start();
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 2
            };
            List<int> integerList = Enumerable.Range(1, 10).ToList();
            Parallel.ForEach(integerList, options, i =>
            {
                long total = DoSomeIndependentTimeConsumingTask();
                Console.WriteLine("{0} - {1}, thread = {2}", i, total, Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("Parallel Foreach Loop Ended");
            sw.Stop();

            Console.WriteLine($"Time Taken by Parallel Foreach Loop in Miliseconds {sw.ElapsedMilliseconds}");

            Console.ReadLine();
        }

        static long DoSomeIndependentTimeConsumingTask()
        {
            long total = 0;
            for (int i = 1; i < 100000000; i++)
            {
                total += i;
            }
            return total;
        }
    }
}