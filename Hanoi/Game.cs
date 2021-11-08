using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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