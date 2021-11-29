using System;
using static Startup;

try { RunHanoi(Conf.FromArgs(args)); }
catch (ConfException)
{
    Console.WriteLine("Please provide a mode (play or solve) and a disk count.");
    Console.WriteLine("Example: 'hanoi solve 5', or 'hanoi play 8'");
}