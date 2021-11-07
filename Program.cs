using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

IO.Run(new Game(3));

static class IO
{
    public static void Run(Game game)
    {
        while (true)
        {
            IO.OutputState(game.Towers);

            var input = IO.AwaitInput();

            try { HandleInput(game, input); }
            catch { continue; }

            if (game.GameSolved)
            {
                IO.OutputGameSolved(game.MoveCount);
                break;
            }
        }
    }

    private static void HandleInput(Game game, string input)
    {
        try
        {
            var move = IO.ReadMove(input);
            game.ApplyMove(move);
        }
        catch (IndexException) { IO.OutputIndexError(); throw; }
        catch (InputMoveException) { IO.OutputInputError(); throw; }
        catch (EmptyTowerException) { IO.OutputTowerError(); throw; }
        catch (InvalidMoveException) { IO.OutputMoveError(); throw; }
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
        catch { throw new InputMoveException(); }
    }

    private static void OutputState(IEnumerable<Tower> towers)
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

    private static void OutputTowerError() =>
        Console.WriteLine("You cannot move a disk from an empty tower.");

    private static void OutputMoveError() =>
        Console.WriteLine("You cannot place a disk on a smaller disk.");

    private static void OutputGameSolved(int moveCount) =>
        Console.WriteLine("You won the game in {0} moves!", moveCount);
}

class Game
{
    public int DiskCount { get; private set; }
    public ImmutableArray<Tower> Towers { get; private set; }
    public int MoveCount { get; private set; }

    public Game(int diskCount)
    {
        this.DiskCount = diskCount;
        this.MoveCount = 0;
        Towers = InitTowers(diskCount);
    }

    public bool GameSolved =>
        Towers.ElementAt(0).IsEmpty &&
        Towers.ElementAt(1).IsEmpty &&
        Towers.ElementAt(2).Height == DiskCount;

    public void ApplyMove(Move move)
    {
        Towers = Towers
            .SetItem(move.To.Value, Towers[move.To.Value].Put(Towers[move.From.Value].Top))
            .SetItem(move.From.Value, Towers[move.From.Value].Take());

        MoveCount++;
    }

    private ImmutableArray<Tower> InitTowers(int diskCount) =>
        new IEnumerable<Disk>[]
        {
            Enumerable.Range(1, diskCount)
                .Select(Disk.FromSize)
                .Reverse(),
            Enumerable.Empty<Disk>(),
            Enumerable.Empty<Disk>()
        }
            .Select(Tower.FromDisks)
            .ToImmutableArray();
}

record Tower(IEnumerable<Disk> Disks)
{
    public static Tower FromDisks(IEnumerable<Disk> disks) =>
        new Tower(disks);

    public Tower Put(Disk disk) =>
        CanAccept(disk) ?
            new Tower(Disks.Append(disk)) :
            throw new InvalidMoveException();

    public Disk Top =>
        IsEmpty ?
            throw new EmptyTowerException() :
            Disks.Last();

    public Tower Take() =>
        IsEmpty ?
            throw new EmptyTowerException() :
            new Tower(Disks.SkipLast(1));

    public bool IsEmpty =>
        !Disks.Any();

    public int Height =>
        Disks.Count();

    private bool CanAccept(Disk disk) =>
        IsEmpty ||
        disk.CanBePlacedOn(Top);
}

record Move(Index From, Index To);

record Index(int Value)
{
    public static Index FromInt(int value) =>
        value is < 0 or > 2 ?
            throw new IndexException() :
            new Index(value);
}

record Disk(int Size)
{
    public static Disk FromSize(int size) =>
        new Disk(size);

    public bool CanBePlacedOn(Disk disk) =>
        Size < disk.Size;
}

class IndexException : ArgumentOutOfRangeException
{
    public IndexException() : base("Index must be between 0 and 2.") { }
}

class InputMoveException : ArgumentException
{
    public InputMoveException() : base("Invalid user move input.") { }
}

class EmptyTowerException : InvalidOperationException
{
    public EmptyTowerException() : base("Cannot take disk from empty tower.") { }
}

class InvalidMoveException : InvalidOperationException
{
    public InvalidMoveException() : base("Cannot put a disk on a smaller disk.") { }
}