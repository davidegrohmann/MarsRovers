namespace MarsRover
{
    public interface IRover
    {
        uint Id { get; }
        Coordinates Position { get; }
        Direction Direction { get; }
        void LandOn(IPlateau plateau);
        void MoveOn(IPlateau plateau);
    }
}