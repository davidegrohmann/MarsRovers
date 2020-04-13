using System.IO;

namespace MarsRover
{
    public class PrintReporter : IReporter
    {
        private readonly TextWriter _tout;
        private readonly TextWriter _terr;

        public PrintReporter(TextWriter tout, TextWriter terr)
        {
            _tout = tout;
            _terr = terr;
        }

        public void Report(IRover rover, Status status)
        {
            _tout.WriteLine(status == Status.Ok
                ? $"{rover.Position.X} {rover.Position.Y} {rover.Direction}"
                : $"{rover.Position.X} {rover.Position.Y} {rover.Direction} [{status}]");
        }

        public void Error(string message)
        {
            _terr.WriteLine(message);
        }
    }
}