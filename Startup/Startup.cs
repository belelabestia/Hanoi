using System;
using System.Threading.Tasks;

static class Startup
{
    public static void RunHanoi(Conf conf)
    {
        var (mode, count) = conf;

        switch (mode)
        {
            case Mode.Play:
                IO.Run(new Game(count));
                break;
            case Mode.Solve:
                var theoreticalTask = Profiler.Run("Theoretical", () => TheoreticalSolver.Run(new Game(count)));
                var practicalTask = Profiler.Run("Practical", () => PracticalSolver.Run(new Game(count)));

                Task.WhenAll(theoreticalTask, practicalTask).Wait();
                break;
            default:
                throw new ArgumentException();
        }
    }
}