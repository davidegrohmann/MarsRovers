using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;

namespace MarsRover
{
    public class Houston
    {
        private readonly MarsPlateau _plateau;
        private readonly List<MarsRover> _rovers;

        public Houston(MarsPlateau plateau, List<MarsRover> rovers)
        {
            _plateau = plateau;
            _rovers = rovers;
        }

        internal void Control(TextWriter tw, TextWriter terr)
        {
            foreach (var rover in _rovers)
            {
                try
                {
                    rover.LandOn(_plateau);
                    rover.MoveOn(_plateau);
                    tw.WriteLine($"{rover.Position.X} {rover.Position.Y} {rover.Direction} [{rover.Id} Success]");
                }
                catch (FallOffPlateau e)
                {
                    terr.WriteLine(e.Message);
                    tw.WriteLine(
                        $"{rover.Position.X} {rover.Position.Y} {rover.Direction} [{rover.Id} Fall Off Plateau]");
                }
                catch (CrashedIntoRover e)
                {
                    terr.WriteLine(e.Message);
                    tw.WriteLine($"{rover.Position.X} {rover.Position.Y} {rover.Direction} [{rover.Id} Crashed]");
                }
            }
        }
    }
}