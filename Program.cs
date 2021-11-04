using System;
using System.Collections.Generic;
using System.Linq;

runGame(3);

void runGame(int diskCount)
{
    var (towers, moveCount) = setupState(diskCount);

    (IEnumerable<int>[], int) setupState(int diskCount)
    {
        var towers = new IEnumerable<int>[]
        {
            Enumerable.Range(1, diskCount),
            Enumerable.Empty<int>(),
            Enumerable.Empty<int>()
        };

        var moveCount = 0;

        return (towers, moveCount);
    }

    while (true)
    {
        outputState(towers);

        var input = onInput();

        int from, to;

        try
        {
            (from, to) = validateInput(input);
        }
        catch
        {
            outputInputError();
            continue;
        }

        if (validateIndexes(from, to))
        {
            outputIndexError();
            continue;
        }

        if (validateFromTowerFilled(towers, from))
        {
            outputTowerError();
            continue;
        }

        if (validateMove(towers, from, to))
        {
            outputMoveError();
            continue;
        }

        (towers, moveCount) = applyMove(towers, from, to, moveCount);

        if (checkGameSolved(towers, diskCount))
        {
            outputGameSolved(moveCount);
            break;
        }

        string onInput()
        {
            var input = Console.ReadLine()!;
            Console.WriteLine("------");
            return input;
        }

        (int from, int to) validateInput(string input)
        {
            var indexes = input.Split(" ");
            from = int.Parse(indexes[0]);
            to = int.Parse(indexes[1]);

            return (from, to);
        }

        void outputInputError() =>
            Console.WriteLine("Describe your next move with a '<from> <to>' notation, where <from> and <to> are the zero-based index of the towers.");

        bool validateIndexes(int from, int to) =>
            (from, to) is ( < 0 or > 2, _) or (_, < 0 or > 2);

        void outputIndexError() =>
            Console.WriteLine("Indexes must be between 0 and 2.");

        bool validateFromTowerFilled(IEnumerable<int>[] towers, int from) =>
            !towers[from].Any();

        void outputTowerError() =>
            Console.WriteLine("You cannot move a disk from an empty tower.");

        bool validateMove(IEnumerable<int>[] towers, int from, int to) =>
            towers[to].Any() && towers[to].Last() > towers[from].Last();

        void outputMoveError() =>
            Console.WriteLine("You cannot move a disk on a smaller disk.");

        (IEnumerable<int>[], int) applyMove(IEnumerable<int>[] towers, int from, int to, int moveCount)
        {
            towers[to] = towers[to].Append(towers[from].Last());
            towers[from] = towers[from].SkipLast(1);
            moveCount++;

            return (towers, moveCount);
        }

        bool checkGameSolved(IEnumerable<int>[] towers, int diskCount) =>
            !towers[0].Any() && !towers[1].Any() && towers[2].Count() == diskCount;

        void outputGameSolved(int moveCount) =>
            Console.WriteLine("You won the game in {0} moves!", moveCount);
    }

    void outputState(IEnumerable<int>[] towers)
    {
        for (int i = 0; i < towers.Count(); i++)
        {
            var tower = towers[i];
            Console.WriteLine(i + " | " + string.Join(' ', tower));
        }

        Console.WriteLine("------");
    }
}