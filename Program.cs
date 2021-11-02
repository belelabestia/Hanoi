using System;
using System.Collections.Generic;
using System.Linq;

var towers = new IEnumerable<int>[]
{
    Enumerable.Range(1, 3),
    Enumerable.Empty<int>(),
    Enumerable.Empty<int>()
};

var moveCount = 0;

while (true)
{
    foreach (var tower in towers)
    {
        Console.WriteLine("| " + string.Join(' ', tower));
    }

    var input = Console.ReadLine()!;

    int from, to;

    try
    {
        var indexes = input.Split(" ");
        from = int.Parse(indexes[0]);
        to = int.Parse(indexes[1]);
    }
    catch
    {
        Console.WriteLine("Describe your next move with a '<from> <to>' notation, where <from> and <to> are the zero-based index of the towers.");
        continue;
    }

    if ((from, to) is (< 0 or > 2, _) or (_, < 0 or > 2))
    {
        Console.WriteLine("Indexes must be between 0 and 2.");
        continue;
    }

    if (towers[from].Count() == 0)
    {
        Console.WriteLine("You cannot move a disk from an empty tower.");
        continue;
    }

    if (towers[to].Any() && towers[to].Last() > towers[from].Last())
    {
        Console.WriteLine("You cannot move a disk on a smaller disk.");
        continue;
    }

    towers[to] = towers[to].Append(towers[from].Last());
    towers[from] = towers[from].SkipLast(1);
    moveCount++;

    Console.WriteLine(input);

    if (towers[0].Count() is 0 && towers[1].Count() is 0 && towers[2].Count() is 3)
    {
        Console.WriteLine("You won the game in {0} moves!", moveCount);
        break;
    }
}
