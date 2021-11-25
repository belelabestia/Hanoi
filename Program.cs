using System;

try { Hanoi.Run(Conf.FromArgs(args)); }
catch (ConfException e) { Console.WriteLine(e.Message); }