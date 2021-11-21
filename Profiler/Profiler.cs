using System;
using System.Diagnostics;
using System.Threading.Tasks;

static class Profiler
{
    public static Task Run(string label, Action run) =>
        Task.Run(() => PrintElapsedTime(label, run));

    private static void PrintElapsedTime(string label, Action run)
    {
        var t = new Stopwatch();
        Console.WriteLine("Starting activity {0}.", label);
        t.Start();
        run();
        t.Stop();
        Console.WriteLine("Activity {0} finished in {1}ms.", label, t.ElapsedMilliseconds);
    }
}