using System;

record Index(int Value)
{
    public static Index FromInt(int value) =>
        value is < 0 or > 2 ?
            throw new IndexException() :
            new Index(value);

    public static implicit operator int(Index index) =>
        index.Value;
}

class IndexException : ArgumentOutOfRangeException
{
    public IndexException() : base("Index must be between 0 and 2.") { }
}