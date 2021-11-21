using System.Collections.Generic;

static class PracticalSolver
{
    public static IEnumerable<Move> EvenCycle =>
        new[]
        {
            new Move(new Index(0), new Index(1)),
            new Move(new Index(0), new Index(2)),
            new Move(new Index(1), new Index(2))
        };

    public static IEnumerable<Move> OddCycle =>
        EvenCycle.Complementary();

    public static void Run(Game game)
    {
        var cycle = Cycle(game.DiskCount);

        while (!game.GameSolved)
        {
            foreach (var move in cycle)
            {
                try { game.ApplyMove(move); }
                catch { game.ApplyMove(move.Inverse()); }

                if (game.GameSolved)
                {
                    Solver.OutputSolved(game.MoveCount);
                    break;
                }
            }
        }
    }

    private static IEnumerable<Move> Cycle(int diskCount) =>
        diskCount % 2 == 0 ?
            EvenCycle :
            OddCycle;
}