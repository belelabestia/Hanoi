using System;
using System.Collections.Generic;

static class Solver
{
    private static bool printing = false;

    public static void OutputSolved(int moveCount, IEnumerable<Move> moves)
    {
        while (printing);

        printing = true;

        Console.WriteLine("Solved in {0} moves!", moveCount);

        foreach (var move in moves)
            Console.WriteLine("{0} -> {1}", move.From.Value, move.To.Value);

        printing = false;
    }

    public static void OutputFailed() =>
        Console.WriteLine("Oh no didn't make it... :(");
}