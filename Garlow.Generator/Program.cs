using System;
using System.Threading;

namespace Garlow.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 0;
            var trend = 1;
            const int minCount = 0;
            const int maxCount = 100;

            Console.WriteLine($"Garlow data generator:");
            Console.WriteLine($"\tstart: {count}");
            Console.WriteLine($"\tmin: {minCount}");
            Console.WriteLine($"\tmax: {maxCount}");
            Console.WriteLine(new string('-', 10));

            while(true)
            {
                // 1/3 chanse of changing trend
                if (new Random().Next(0, 3) == 0)
                    trend = -trend;

                // check for boundaries
                if (count == minCount)
                    trend = 1;
                else if (count == maxCount)
                    trend = -1;

                count += trend;

                // send trend information to the server
                Console.WriteLine(count);
                Thread.Sleep(1000);
            }
        }
    }
}
