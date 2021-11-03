using System;
using System.Collections.Generic;
using System.Linq;

var diskCount = 3;

// setup state

// state
var towers = new IEnumerable<int>[]
{
    Enumerable.Range(1, diskCount),
    Enumerable.Empty<int>(),
    Enumerable.Empty<int>()
};

var moveCount = 0;

// start game

while (true)
{
    // output state
    for (int i = 0; i < towers.Count(); i++)
    {
        var tower = towers[i];
        Console.WriteLine(i + " | " + string.Join(' ', tower));
    }
    Console.WriteLine("------");

    // on input
    var input = Console.ReadLine()!;
    Console.WriteLine("------");

    int from, to;

    // validate input as (from, to)
    try
    {
        var indexes = input.Split(" ");
        from = int.Parse(indexes[0]);
        to = int.Parse(indexes[1]);
    }
    catch
    {
        // output error
        Console.WriteLine("Describe your next move with a '<from> <to>' notation, where <from> and <to> are the zero-based index of the towers.");
        continue;
    }

    // validate (from, to) as indexes
    if ((from, to) is (< 0 or > 2, _) or (_, < 0 or > 2))
    {
        // output error
        Console.WriteLine("Indexes must be between 0 and 2.");
        continue;
    }

    // validate from-tower as filled
    if (!towers[from].Any())
    {
        // output error
        Console.WriteLine("You cannot move a disk from an empty tower.");
        continue;
    }

    // validate to-tower as filled and top disk of to-tower smaller than top disk of from-tower as move
    if (towers[to].Any() && towers[to].Last() > towers[from].Last())
    {
        // output error
        Console.WriteLine("You cannot move a disk on a smaller disk.");
        continue;
    }

    // apply move to towers
    towers[to] = towers[to].Append(towers[from].Last());
    towers[from] = towers[from].SkipLast(1);
    moveCount++;

    // check game solved from towers
    if (!towers[0].Any() && !towers[1].Any() && towers[2].Count() == diskCount)
    {
        // output game solved with state
        Console.WriteLine("You won the game in {0} moves!", moveCount);
        break;
    }
}
