using System;
using System.Collections.Generic;

namespace MarsRover
{
    public enum Command
    {
        M, L, R
    }

    public enum Direction
    {
        N, E, S, W
    }

    public class MarsRover : IRover
    {
        public uint Id { get; }
        public Direction Direction { get; private set; }
        public Coordinates Position { get; private set; }
        private readonly List<Command> _commands;

        public MarsRover(uint id, Coordinates landing, Direction direction, List<Command> commands)
        {
            Id = id;
            Position = landing;
            Direction = direction;
            _commands = commands;
        }

        public void LandOn(IPlateau plateau)
        {
            plateau.Land(this);
        }

        public void MoveOn(IPlateau marsPlateau)
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
                        throw new ArgumentOutOfRangeException(nameof(command), command, null);
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
                _ => throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null)
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