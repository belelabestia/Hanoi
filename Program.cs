using System;
using System.Diagnostics;
using System.Threading.Tasks;

var count = 13;

var task1 = PrintElapsedTime("Theoretical", () => TheoreticalSolver.Run(new Game(count)));
var task2 = PrintElapsedTime("Practical", () => PracticalSolver.Run(new Game(count)));

await Task.WhenAll(task1, task2);

// IO.Run(new Game(3));

Task PrintElapsedTime(string label, Action run) =>
    Task.Run(() =>
    {
        var t = new Stopwatch();
        Console.WriteLine("Starting activity {0}.", label);
        t.Start();
        run();
        t.Stop();
        Console.WriteLine("Activity {0} finished in {1}ms.", label, t.ElapsedMilliseconds);
    });