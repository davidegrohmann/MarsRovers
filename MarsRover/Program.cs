using System;
using System.Collections.Generic;
using System.IO;

namespace MarsRover
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                IPlateau plateau;
                var rovers = new List<IRover>();
                Console.WriteLine(args[0]);
                using (var tr = args.Length <= 0 ? Console.In : new StreamReader(args[0]))
                {
                    var parser = new Parser(tr);
                    plateau = parser.ParseMarsPlateau();
                    if (plateau == null)
                    {
                        throw new ArgumentException("Missing information about Mars plateau");
                    }

                    MarsRover rover;
                    for (uint i = 0; (rover = parser.ParseRover(i)) != null; i++)
                    {
                        rovers.Add(rover);
                    }
                }

                var h = new Houston(plateau, rovers);
                h.Control(new PrintReporter(Console.Out, Console.Error));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e.Message}\n{e.StackTrace}");
                Environment.Exit(1);
            }
        }
    }
}