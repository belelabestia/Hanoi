using System;

static class Solver
{
    public static void OutputSolved(int moveCount) =>
        Console.WriteLine("Solved in {0} moves!", moveCount);

    public static void OutputFailed() =>
        Console.WriteLine("Oh no didn't make it... :(");
}