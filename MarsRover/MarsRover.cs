using System;
using System.Collections.Generic;

namespace MarsRover
{
    internal enum Command
    {
        M,  L, R
    }

    internal enum Direction
    {
        N, E, S, W
    }

    public class MarsRover
    {
        internal readonly uint Id;
        internal Direction Direction { get; private set; }
        internal Coordinates Position { get; private set; }
        private readonly List<Command> _commands;

        internal MarsRover(uint id, Coordinates landing, Direction direction, List<Command> commands)
        {
            Id = id;
            Position = landing;
            Direction = direction;
            _commands = commands;
        }

        public void LandOn(MarsPlateau plateau)
        {
            plateau.Land(this);
        }

        public void MoveOn(MarsPlateau marsPlateau)
        {
            foreach (var command in _commands)
            {
                switch (command)
                {
                    case Command.R:
                        Direction = RightOf(Direction);
                        break;
                    case Command.L:
                        Direction = LeftOf(Direction);
                        break;
                    case Command.M:
                        var next = MoveTo();
                        try
                        {
                            marsPlateau.Move(this, next);
                        }
                        finally
                        {
                            Position = next;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Coordinates MoveTo()
        {
            var x = Position.X;
            var y = Position.Y;
            return Direction switch
            {
                Direction.N => new Coordinates(x, y + 1),
                Direction.E => new Coordinates(x + 1, y),
                Direction.S => new Coordinates(x, y - 1),
                Direction.W => new Coordinates(x - 1, y),
                _ => throw new ArgumentOutOfRangeException($"{Direction}")
            };
        }

        private static Direction RightOf(Direction direction)
        {
            return direction switch
            {
                Direction.N => Direction.E,
                Direction.E => Direction.S,
                Direction.S => Direction.W,
                Direction.W => Direction.N,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        private static Direction LeftOf(Direction direction)
        {
            return direction switch
            {
                Direction.N => Direction.W,
                Direction.E => Direction.N,
                Direction.S => Direction.E,
                Direction.W => Direction.S,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}