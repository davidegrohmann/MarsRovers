using System;
using System.Collections.Generic;
using System.IO;

namespace MarsRover
{
    public class Parser
    {
        private readonly TextReader _in;

        public Parser(TextReader tr)
        {
            _in = tr ?? throw new ArgumentNullException(nameof(tr));
        }

        public MarsPlateau? ParsePlateau()
        {
            var s = _in.ReadLine();
            if (s == null)
            {
                return null;
            }

            var coordinates = s.Split(' ');
            if (coordinates.Length != 2)
            {
                throw new ArgumentException($"Malformed mars plateau coordinates: '{s}'");
            }

            return new MarsPlateau(ParseCoordinates(coordinates));
        }

        public MarsRover? ParseRover(uint id)
        {
            var l = _in.ReadLine();
            if (l == null)
            {
                return null;
            }

            var landing = l.Split(' ');
            if (landing.Length != 3)
            {
                throw new ArgumentException($"Malformed mars rover landing coordinates and direction: '{l}'");
            }

            var coordinates = ParseCoordinates(landing);
            var direction = ParseDirection(landing[2]);
            var commands = ParseCommands(_in.ReadLine());

            return new MarsRover(id, coordinates, direction, commands);
        }

        private static Coordinates ParseCoordinates(IReadOnlyList<string> coordinates)
        {
            int x, y;
            try
            {
                x = int.Parse(coordinates[0]);
                y = int.Parse(coordinates[1]);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Malformed coordinates: '{coordinates[0]} {coordinates[1]}'", e);
            }

            if (x < 0 || y < 0)
            {
                throw new ArgumentException($"Coordinates must be positive, parse [{x}, {y}]");
            }

            return new Coordinates(x, y);
        }

        private static Direction ParseDirection(string? direction)
        {
            return direction switch
            {
                "N" => Direction.N,
                "E" => Direction.E,
                "S" => Direction.S,
                "W" => Direction.W,
                _ => throw new ArgumentException($"Unknown direction: '{direction}'")
            };
        }

        private static List<Command> ParseCommands(string? l)
        {
            var commands = new List<Command>();
            if (l == null)
            {
                return commands;
            }

            foreach (var c in l)
            {
                commands.Add(ParseCommand(c));
            }

            return commands;
        }

        private static Command ParseCommand(char c)
        {
            return c switch
            {
                'M' => Command.M,
                'L' => Command.L,
                'R' => Command.R,
                _ => throw new ArgumentException($"Unknown command: '{c}'")
            };
        }
    }
}