using System;

record Conf(Mode Mode, int Count)
{
    public static Conf FromArgs(string[] args)
    {
        try
        {
            return new Conf(
                Mode: args[0].ToLower() switch
                {
                    "play" => Mode.Play,
                    "solve" => Mode.Solve,
                    var a => throw new ArgumentException($"Invalid argument: '{a}'.")
                },
                Count: int.Parse(args[1])
            );
        }
        catch (Exception e) { throw new ConfException(e); }
    }
}

enum Mode { Play, Solve }

class ConfException : ArgumentException
{
    public ConfException(Exception e) : base($"Invalid hanoi parameters: {e.Message}", e) { }
}