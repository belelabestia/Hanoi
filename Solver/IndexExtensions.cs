static class IndexExtensions
{
    public static Index Symmetrical(this Index index) =>
        index.Value switch
        {
            0 => new Index(2),
            1 => new Index(1),
            2 => new Index(0),
            _ => throw new IndexException()
        };

    public static Index Complementary(this Index index) =>
        index.Value switch
        {
            0 => new Index(0),
            1 => new Index(2),
            2 => new Index(1),
            _ => throw new IndexException()
        };
}