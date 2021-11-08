using System;

static class Solver
{
    public static void OutputMove(Move move) =>
        Console.WriteLine("Applying move {0} -> {1}", move.From.Value, move.To.Value);

    public static void OutputSolved(int moveCount) =>
        Console.WriteLine("Solved in {0} moves!", moveCount);

    public static void OutputFailed() =>
        Console.WriteLine("Oh no didn't make it... :(");
}