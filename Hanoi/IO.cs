using System;
using System.Collections.Generic;
using System.Linq;

static class IO
{
    public static void Run(Game game)
    {
        while (!game.GameSolved)
            try
            {
                OutputState(game.Towers);

                var input = AwaitInput();
                var move = ReadMove(input);

                game.ApplyMove(move);
            }
            catch (IndexException) { OutputIndexError(); }
            catch (InputException) { OutputInputError(); }
            catch (EmptyTowerException) { OutputEmptyTowerError(); }
            catch (InvalidTowerException) { OutputInvalidTowerError(); }
    }

    private static string AwaitInput()
    {
        var input = Console.ReadLine()!;
        Console.WriteLine("------");
        return input;
    }

    private static Move ReadMove(string input)
    {
        try
        {
            var indexes = input
                .Split(" ")
                .Select(int.Parse)
                .Select(Index.FromInt)
                .ToArray();

            return new Move(indexes[0], indexes[1]);
        }
        catch (IndexException) { throw; }
        catch (Exception e) { throw new InputException(e); }
    }

    public static void OutputState(IEnumerable<Tower> towers)
    {
        for (int i = 0; i < towers.Count(); i++)
        {
            var tower = towers.ElementAt(i);
            Console.WriteLine(i + " | " + string.Join(' ', tower.Disks.Select(disk => disk.Size.ToString())));
        }

        Console.WriteLine("------");
    }

    private static void OutputInputError() =>
        Console.WriteLine("Describe your next move with a '<from> <to>' notation, where <from> and <to> are the zero-based index of the towers.");

    private static void OutputIndexError() =>
        Console.WriteLine("Indexes must be between 0 and 2.");

    private static void OutputEmptyTowerError() =>
        Console.WriteLine("You cannot move a disk from an empty tower.");

    private static void OutputInvalidTowerError() =>
        Console.WriteLine("You cannot place a disk on a smaller disk.");

    private static void OutputGameSolved(int moveCount) =>
        Console.WriteLine("You won the game in {0} moves!", moveCount);
}

class InputException : ArgumentException
{
    public InputException(Exception e) : base("Invalid user move input.", e) { }
}