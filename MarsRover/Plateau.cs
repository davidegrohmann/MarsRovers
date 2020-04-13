using System;

namespace MarsRover
{
    public interface IPlateau
    {
        Coordinates Size { get; }
        void Land(IRover rover);
        void Move(IRover rover, Coordinates to);
    }

    public class FallOffPlateau : Exception
    {
        public FallOffPlateau(string message) : base(message)
        {
        }
    }

    public class CrashedIntoRover : Exception
    {
        public CrashedIntoRover(string message) : base(message)
        {
        }
    }
}