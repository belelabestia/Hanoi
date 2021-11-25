using System;
using System.Threading.Tasks;

static class Hanoi
{
    public static void Run(Conf conf)
    {
        try
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
        catch
        {
            Console.WriteLine("Please provide a mode (play or solve) and a disk count.");
            Console.WriteLine("Example: 'hanoi solve 5', or 'hanoi play 8'");
        }
    }
}