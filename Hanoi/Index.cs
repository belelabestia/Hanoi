using System;

record Index(int Value)
{
    public static Index FromInt(int value) =>
        value is < 0 or > 2 ?
            throw new IndexException() :
            new Index(value);
}

class IndexException : ArgumentOutOfRangeException
{
    public IndexException() : base("Index must be between 0 and 2.") { }
}