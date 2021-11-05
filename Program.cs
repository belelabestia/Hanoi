using System;
using System.Collections.Generic;
using System.Linq;

Game.Run(3);

static class Game
{
    public static bool CheckGameSolved(IEnumerable<int>[] towers, int diskCount) =>
        !towers[0].Any() && !towers[1].Any() && towers[2].Count() == diskCount;

    public static void Run(int diskCount)
    {
        var moveCount = 0;
        var towers = Towers.Setup(diskCount);

        while (true)
        {
            IO.OutputState(towers);

            var input = IO.OnInput();

            int from, to;

            try
            {
                (from, to) = Move.ValidateInput(input);
            }
            catch
            {
                IO.OutputInputError();
                continue;
            }

            if (Move.ValidateIndexes(from, to))
            {
                IO.OutputIndexError();
                continue;
            }

            if (Move.ValidateFromTowerFilled(towers, from))
            {
                IO.OutputTowerError();
                continue;
            }

            if (Move.ValidateMove(towers, from, to))
            {
                IO.OutputMoveError();
                continue;
            }

            towers = Towers.ApplyMove(towers, from, to);
            moveCount++;

            if (Game.CheckGameSolved(towers, diskCount))
            {
                IO.OutputGameSolved(moveCount);
                break;
            }
        }
    }
}

static class IO
{
    public static string OnInput()
    {
        var input = Console.ReadLine()!;
        Console.WriteLine("------");
        return input;
    }

    public static void OutputInputError() =>
        Console.WriteLine("Describe your next move with a '<from> <to>' notation, where <from> and <to> are the zero-based index of the towers.");

    public static void OutputIndexError() =>
        Console.WriteLine("Indexes must be between 0 and 2.");

    public static void OutputTowerError() =>
        Console.WriteLine("You cannot move a disk from an empty tower.");

    public static void OutputMoveError() =>
        Console.WriteLine("You cannot move a disk on a smaller disk.");

    public static void OutputGameSolved(int moveCount) =>
        Console.WriteLine("You won the game in {0} moves!", moveCount);

    public static void OutputState(IEnumerable<int>[] towers)
    {
        for (int i = 0; i < towers.Count(); i++)
        {
            var tower = towers[i];
            Console.WriteLine(i + " | " + string.Join(' ', tower));
        }

        Console.WriteLine("------");
    }
}

static class Towers
{
    public static IEnumerable<int>[] Setup(int diskCount) =>
        new IEnumerable<int>[]
        {
            Enumerable.Range(1, diskCount),
            Enumerable.Empty<int>(),
            Enumerable.Empty<int>()
        };

    public static IEnumerable<int>[] ApplyMove(IEnumerable<int>[] towers, int from, int to)
    {
        towers[to] = towers[to].Append(towers[from].Last());
        towers[from] = towers[from].SkipLast(1);

        return towers;
    }
}

static class Move
{
    public static (int from, int to) ValidateInput(string input)
    {
        var indexes = input.Split(" ");
        var from = int.Parse(indexes[0]);
        var to = int.Parse(indexes[1]);

        return (from, to);
    }

    public static bool ValidateIndexes(int from, int to) =>
        (from, to) is ( < 0 or > 2, _) or (_, < 0 or > 2);

    public static bool ValidateFromTowerFilled(IEnumerable<int>[] towers, int from) =>
        !towers[from].Any();

    public static bool ValidateMove(IEnumerable<int>[] towers, int from, int to) =>
        towers[to].Any() && towers[to].Last() > towers[from].Last();
}