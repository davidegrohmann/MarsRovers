namespace MarsRover
{
    public enum Status
    {
        Ok,
        Crashed,
        FellOff
    }

    public interface IReporter
    {
        void Report(IRover rover, Status status);
        void Error(string message);
    }
}