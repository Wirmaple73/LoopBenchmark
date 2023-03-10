using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace LoopBenchmark
{
    class Program
    {
        static int numOfLoops;

        static void Main()
        {
            Console.Title = "Loop Benchmark";

            while (true)
            {
                GetLoopCount();
                RunBenchmark();

                PromptRestart();
            }
        }

        static void GetLoopCount()
        {
            try
            {
                Console.Write("Enter the number of benchmark loops: ");
                numOfLoops = Convert.ToInt32(Console.ReadLine());

                if (numOfLoops <= 0)
                    throw new ArgumentOutOfRangeException();
            }
            catch
            {
                RestartInput();
                GetLoopCount();
            }
        }

        static void RunBenchmark()
        {
            Console.WriteLine("\nStarting the benchmark, please wait...\n\n");
            Console.WriteLine("------------------------------");

            Benchmark.Start(numOfLoops);
        }

        static void PromptRestart()
        {
            Console.WriteLine("\n\nPress \"ESC\" to exit the program, or any other key to restart the benchmark...");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default:
                    Console.Clear();
                    break;
            }
        }

        static void RestartInput()
        {
            Console.WriteLine("\nError: Invalid number entered, please enter a positive integer only.");
            Console.WriteLine("Press any key to continue...");

            Console.ReadKey(true);
            Console.Clear();
        }
    }

    static class Benchmark
    {
        static readonly Timer timer = new Timer(1000);
        static bool isRunning = true;
        static ulong loopsExecuted = 0;

        public static void Start(int maxNumOfLoops)
        {
            timer.Elapsed += timer_Elapsed;

            var loopList = new List<ulong>();


            for (int i = 0; i < maxNumOfLoops; i++)
            {
                timer.Start();

                while (isRunning)
                {
                    ++loopsExecuted;
                }

                timer.Stop();

                loopList.Add(loopsExecuted);
                Console.WriteLine("Benchmark #{0}: {1:N0} LPS", i + 1, loopsExecuted);

                ResetBenchmark();
            }

            ReportAverage(loopList.Average((x => (double)x)));
        }

        private static void ReportAverage(double average)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("\nAverage benchmark score:", average);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0:N0}", average);
            Console.ResetColor();

            Console.WriteLine(" LPS (Loops per second)");
        }

        private static void ResetBenchmark()
        {
            loopsExecuted = 0;
            isRunning = true;
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            isRunning = false;
        }
    }
}
