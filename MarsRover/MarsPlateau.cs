using System;
using System.Collections.Generic;

namespace MarsRover
{
    public class MarsPlateau : IPlateau
    {
        private readonly Coordinates _size;
        private readonly IDictionary<Coordinates, IRover> _heldPositions = new Dictionary<Coordinates, IRover>();

        public MarsPlateau(Coordinates c)
        {
            _size = c;
        }

        public void Land(IRover rover)
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

        public void Move(IRover rover, Coordinates to)
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
        }

        private bool OutsidePlateau(Coordinates roverPosition)
        {
            return roverPosition.X > _size.X || roverPosition.Y > _size.Y || roverPosition.X < 0 || roverPosition.Y < 0;
        }
    }
}