using System;
using System.Collections.Generic;
using System.Linq;

new Game(3).Run();

class Game
{
    private int diskCount;
    private Tower[] towers;
    private int moveCount;

    public Game(int diskCount)
    {
        this.diskCount = diskCount;
        this.moveCount = 0;
        towers = InitTowers(diskCount);
    }

    public bool GameSolved =>
        towers[0].IsEmpty &&
        towers[1].IsEmpty &&
        towers[2].Height == diskCount;

    public void Run()
    {
        while (true)
        {
            IO.OutputState(towers);

            var input = IO.AwaitInput();

            try { HandleInput(input); }
            catch { continue; }

            if (GameSolved)
            {
                IO.OutputGameSolved(moveCount);
                break;
            }
        }
    }

    private void HandleInput(string input)
    {
        try
        {
            var move = IO.ReadMove(input);
            towers = ApplyMove(move);
            moveCount++;
        }
        catch (InputMoveException) { IO.OutputInputError(); throw; }
        catch (IndexException) { IO.OutputIndexError(); throw; }
        catch (EmptyTowerException) { IO.OutputTowerError(); throw; }
        catch (InvalidMoveException) { IO.OutputMoveError(); throw; }
    }

    private Tower[] ApplyMove(Move move)
    {
        towers[move.To.Value] = towers[move.To.Value].Put(towers[move.From.Value].Top);
        towers[move.From.Value] = towers[move.From.Value].Take();

        return towers;
    }

    private Tower[] InitTowers(int diskCount) =>
        new IEnumerable<Disk>[]
        {
            Enumerable.Range(1, diskCount)
                .Select(Disk.FromSize)
                .Reverse(),
            Enumerable.Empty<Disk>(),
            Enumerable.Empty<Disk>()
        }
            .Select(Tower.FromDisks)
            .ToArray();
}

static class IO
{
    public static string AwaitInput()
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

    public static void OutputState(Tower[] towers)
    {
        for (int i = 0; i < towers.Count(); i++)
        {
            var tower = towers[i];
            Console.WriteLine(i + " | " + string.Join(' ', tower.Disks.Select(disk => disk.Size.ToString())));
        }

        Console.WriteLine("------");
    }

    public static Move ReadMove(string input)
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
        catch
        {
            throw new InputMoveException();
        }
    }
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