record Disk(int Size)
{
    public static Disk FromSize(int size) =>
        new Disk(size);

    public bool CanBePlacedOn(Disk disk) =>
        Size < disk.Size;
}