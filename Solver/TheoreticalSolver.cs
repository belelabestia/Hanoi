using System.Collections.Generic;
using System.Linq;

static class TheoreticalSolver
{
    public static Move Median => new Move(new Index(0), new Index(2));

    public static void Run(Game game)
    {
        foreach (var move in NthSeries(game.DiskCount))
        {
            game.ApplyMove(move);
        }

        if (game.GameSolved)
        {
            Solver.OutputSolved(game.MoveCount);
            return;
        }

        Solver.OutputFailed();
    }

    private static IEnumerable<Move> NthSeries(int diskCount)
    {
        var series = Enumerable.Empty<Move>();

        for (var n = 0; n < diskCount; n++)
        {
            var complementary = series.Complementary();

            series = complementary
                .Append(Median)
                .Concat(complementary.Symmetrical());
        }

        return series;
    }
}
