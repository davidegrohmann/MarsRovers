using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace MarsRover
{
    public class MarsPlateau
    {
        private readonly Coordinates _size;
        private readonly IDictionary<Coordinates, MarsRover> _heldPositions = new Dictionary<Coordinates, MarsRover>();

        internal MarsPlateau(Coordinates c)
        {
            _size = c;
        }

        internal void Land(MarsRover rover)
        {
            if (OutsidePlateau(rover.Position))
            {
                throw new FallOffPlateau(
                    $"Rover {rover.Id} landed at [{rover.Position.X}, {rover.Position.Y}] outside the plateau");
            }

            if (_heldPositions.TryGetValue(rover.Position, out var other))
            {
                throw new CrashedIntoRover(
                    $"Rover {rover.Id} landed onto another Rover {other.Id} at [{rover.Position.X}, {rover.Position.Y}] ");
            }

            _heldPositions.Add(rover.Position, rover);
        }

        internal bool? Move(MarsRover rover, Coordinates to)
        {
            if (!_heldPositions.TryGetValue(rover.Position, out var self) || self != rover)
            {
                throw new ArgumentException(
                    $"Unknown rover {rover.Id} at position [{rover.Position.X}, {rover.Position.Y}]");
            }

            if (OutsidePlateau(to))
            {
                throw new FallOffPlateau(
                    $"Rover {rover.Id} fell off plateau at [{to.X}, {to.Y}]");
            }

            if (_heldPositions.TryGetValue(to, out var other))
            {
                throw new CrashedIntoRover(
                    $"Rover {rover.Id} crashed into another Rover {other.Id} at [{to.X}, {to.Y}]");
            }

            _heldPositions.Remove(rover.Position);
            _heldPositions.Add(to, rover);
            return true;
        }

        private bool OutsidePlateau(Coordinates roverPosition)
        {
            return roverPosition.X > _size.X || roverPosition.Y > _size.Y || roverPosition.X < 0 || roverPosition.Y < 0;
        }
    }

    internal class FallOffPlateau : Exception
    {
        public FallOffPlateau(string message) : base(message)
        {
        }
    }

    internal class CrashedIntoRover : Exception
    {
        public CrashedIntoRover(string message) : base(message)
        {
        }
    }
}