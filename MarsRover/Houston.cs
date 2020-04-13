using System.Collections.Generic;

namespace MarsRover
{
    public class Houston
    {
        private readonly IPlateau _plateau;
        private readonly List<IRover> _rovers;

        public Houston(IPlateau plateau, List<IRover> rovers)
        {
            _plateau = plateau;
            _rovers = rovers;
        }

        public void Control(IReporter reporter)
        {
            foreach (var rover in _rovers)
            {
                try
                {
                    rover.LandOn(_plateau);
                    rover.MoveOn(_plateau);
                    reporter.Report(rover, Status.Ok);
                }
                catch (FallOffPlateau e)
                {
                    reporter.Error(e.Message);
                    reporter.Report(rover, Status.FellOff);
                }
                catch (CrashedIntoRover e)
                {
                    reporter.Error(e.Message);
                    reporter.Report(rover, Status.Crashed);
                }
            }
        }
    }
}