using System;
using System.Collections.Generic;
using System.Linq;

record Tower(IEnumerable<Disk> Disks)
{
    public static Tower FromDisks(IEnumerable<Disk> disks) =>
        new Tower(disks);

    public Tower Put(Disk disk) =>
        CanAccept(disk) ?
            new Tower(Disks.Append(disk)) :
            throw new InvalidTowerException();

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

class EmptyTowerException : InvalidOperationException
{
    public EmptyTowerException() : base("Cannot take disk from empty tower.") { }
}

class InvalidTowerException : InvalidOperationException
{
    public InvalidTowerException() : base("Cannot put a disk on a smaller disk.") { }
}