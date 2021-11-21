using System;

record Params(Mode Mode, int Count)
{
    public static Params FromArgs(string[] args) =>
        new Params(
            Mode: args[0].ToLower() switch
            {
                "play" => Mode.Play,
                "solve" => Mode.Solve,
                _ => throw new ArgumentException()
            },
            Count: int.Parse(args[1])
        );
}

enum Mode { Play, Solve }