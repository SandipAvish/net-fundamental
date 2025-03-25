using System.Diagnostics;

class Program
{
    static readonly object lockObject = new object();
    static long x;
    static void Main(string[] args)
    {
    LOOP:
        Stopwatch sw = new Stopwatch();
        Console.WriteLine(@"1. Example to Understand Interlocked Add Method
2. Example to Understand Interlocked Exchange and CompareExchange Method
3. Interlocked vs Lock from a Performance Point of View
4. When to use Lock over Interlocked");

        Console.Write("Enter the option: ");
        int.TryParse(Console.ReadLine(), out int choice);

        switch (choice)
        {
            case 1:
                long sumValueWithoutInterLocked = 0;
                long sumValueWithInterLocked = 0;

                Parallel.For(0, 100000, i =>
                {
                    sumValueWithoutInterLocked += i;
                    Interlocked.Increment(ref sumValueWithInterLocked);
                });
                Console.WriteLine($"Sum Value Without Interlocked: {sumValueWithoutInterLocked}");
                Console.WriteLine($"Sum Value With Interlocked: {sumValueWithInterLocked}");
                break;
            case 2:
                Thread t1 = new Thread(new ThreadStart(SomeMethod));
                t1.Start();
                t1.Join();

                Console.WriteLine(Interlocked.Read(ref Program.x));
                break;
            case 3:
                int incrementValue = 0;
                sw.Start();
                Parallel.For(0, 10000000, num =>
                {
                    lock (lockObject)
                    {
                        incrementValue++;
                    }
                });
                sw.Stop();
                Console.WriteLine($"Result using Lock: {incrementValue}");
                Console.WriteLine($"Lock took {sw.ElapsedMilliseconds} Milliseconds");

                incrementValue = 0;
                sw.Restart();

                Parallel.For(0, 10000000, number =>
                {
                    Interlocked.Increment(ref incrementValue);
                });
                sw.Stop();
                Console.WriteLine($"Result using Interlocked: {incrementValue}");
                Console.WriteLine($"Interlocked took {sw.ElapsedMilliseconds} Milliseconds");
                break;
            case 4:
                long incrementValueLong = 0;
                long sumValue = 0;
                Parallel.For(0, 100000, num =>
                {
                    Interlocked.Increment(ref incrementValueLong);
                    Interlocked.Add(ref sumValue, incrementValueLong);
                });
                Console.WriteLine($"Increment Value With Interlocked: {incrementValueLong}");
                Console.WriteLine($"Sum Value With Interlocked: {sumValue}");

                incrementValueLong = 0;
                sumValue = 0;

                Parallel.For(0, 100000, num =>
                {
                    lock (lockObject)
                    {
                        incrementValueLong++;
                        sumValue += incrementValueLong;
                    }
                });
                Console.WriteLine($"Increment Value with lock: {incrementValueLong}");
                Console.WriteLine($"Sum value with lock: {sumValue}");
                break;
            default:
                Console.WriteLine("Invalid Choice");
                break;
        }
        Console.WriteLine("Do you want to continue? (y/n)");
        char.TryParse(Console.ReadLine(), out char res);

        if (res == 'y')
        {
            goto LOOP;
        }
    }
    static void SomeMethod()
    {
        Interlocked.Exchange(ref Program.x, 20);

        long result = Interlocked.CompareExchange(ref Program.x, 50, 20);

        Console.WriteLine(result);
    }
}