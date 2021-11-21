using System;
using System.Threading.Tasks;

int count;

try { count = int.Parse(args[0]); }
catch
{
    Console.WriteLine("Please provide a disk count parameter.");
    return;
}

var theoreticalTask = Profiler.Run("Theoretical", () => TheoreticalSolver.Run(new Game(count)));
var practicalTask = Profiler.Run("Practical", () => PracticalSolver.Run(new Game(count)));

Task.WhenAll(theoreticalTask, practicalTask).Wait();