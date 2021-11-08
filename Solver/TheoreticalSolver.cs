using System.Collections.Generic;
using System.Linq;

static class TheoreticalSolver
{
    public static Move Median => new Move(new Index(0), new Index(2));

    public static void Run(Game game)
    {
        foreach (var move in NthSeries(game.DiskCount))
        {
            // IO.OutputState(game.Towers);
            // Solver.OutputMove(move);
            game.ApplyMove(move);
        }

        if (game.GameSolved)
        {
            // IO.OutputState(game.Towers);
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
