using System;

record Conf(Mode Mode, int Count)
{
    public static Conf FromArgs(string[] args) =>
        new Conf(
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