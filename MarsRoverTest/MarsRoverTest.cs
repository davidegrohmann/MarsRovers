using System.Collections.Generic;
using MarsRover;
using NUnit.Framework;

namespace MarsRoverTest
{
    public class MarsRoverTests
    {
        [Test]
        public void ShouldRunASimulation()
        {
            IPlateau p = new MarsPlateau(new Coordinates(5, 5));
            IRover one = new MarsRover.MarsRover(1, new Coordinates(1, 2), Direction.N,
                new List<Command>
                {
                    Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.M
                });
            IRover two = new MarsRover.MarsRover(2, new Coordinates(3, 3), Direction.E,
                new List<Command>
                {
                    Command.M, Command.M, Command.R, Command.M, Command.M, Command.R, Command.M, Command.R, Command.R,
                    Command.M
                });

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover> {one, two}).Control(testReporter);

            Assert.IsEmpty(testReporter.Errors);
            var expected = new Dictionary<IRover, Status>()
            {
                {one, Status.Ok},
                {two, Status.Ok}
            };
            Assert.AreEqual(expected, testReporter.Dictionary);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 3), one.Position);
            Assert.AreEqual(Direction.E, two.Direction);
            Assert.AreEqual(new Coordinates(5, 1), two.Position);
        }

        [Test]
        public void ShouldRunASimulationWithNoRover()
        {
            IPlateau p = new MarsPlateau(new Coordinates(5, 5));

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover>()).Control(testReporter);

            Assert.IsEmpty(testReporter.Errors);
            Assert.IsEmpty(testReporter.Dictionary);
        }

        [Test]
        public void ShouldRunASimulationWithLandingOnly()
        {
            IPlateau p = new MarsPlateau(new Coordinates(5, 5));
            IRover one = new MarsRover.MarsRover(1, new Coordinates(1, 2), Direction.N,
                new List<Command>());

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover> {one}).Control(testReporter);

            Assert.IsEmpty(testReporter.Errors);
            var expected = new Dictionary<IRover, Status>()
            {
                {one, Status.Ok},
            };
            Assert.AreEqual(expected, testReporter.Dictionary);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 2), one.Position);
        }

        [Test]
        public void ShouldReportFellOffOnLanding()
        {
            IPlateau p = new MarsPlateau(new Coordinates(1, 1));
            IRover one = new MarsRover.MarsRover(1, new Coordinates(1, 2), Direction.N,
                new List<Command>
                {
                    Command.L, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M,
                    Command.M
                });

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover> {one}).Control(testReporter);

            Assert.AreEqual(new List<string>
                {
                    $"Rover {one.Id} landed at [{one.Position.X}, {one.Position.Y}] outside the plateau"
                },
                testReporter.Errors);

            Assert.AreEqual(new Dictionary<IRover, Status>
            {
                {one, Status.FellOff}
            }, testReporter.Dictionary);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 2), one.Position);
        }

        [Test]
        public void ShouldReportCrashOnLanding()
        {
            IPlateau p = new MarsPlateau(new Coordinates(5, 5));
            IRover one = new MarsRover.MarsRover(1, new Coordinates(1, 2), Direction.N,
                new List<Command>
                {
                    Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.M
                });
            IRover two = new MarsRover.MarsRover(2, new Coordinates(1, 3), Direction.E,
                new List<Command>
                {
                    Command.M, Command.M, Command.R, Command.M, Command.M, Command.R, Command.M, Command.R, Command.R,
                    Command.M
                });

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover> {one, two}).Control(testReporter);

            Assert.AreEqual(new List<string>
                {
                    $"Rover {two.Id} landed onto Rover {one.Id} at [{one.Position.X}, {one.Position.Y}]"
                },
                testReporter.Errors);

            Assert.AreEqual(new Dictionary<IRover, Status>
            {
                {one, Status.Ok},
                {two, Status.Crashed}
            }, testReporter.Dictionary);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 3), one.Position);
            Assert.AreEqual(Direction.E, two.Direction);
            Assert.AreEqual(new Coordinates(1, 3), two.Position);
        }

        [Test]
        public void ShouldReportFellOffWhileMoving()
        {
            IPlateau p = new MarsPlateau(new Coordinates(1, 1));
            IRover one = new MarsRover.MarsRover(1, new Coordinates(1, 0), Direction.N,
                new List<Command>
                {
                    Command.L, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M,
                    Command.M
                });

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover> {one}).Control(testReporter);

            Assert.AreEqual(new List<string>
                {
                    $"Rover {one.Id} fell off plateau at [{one.Position.X}, {one.Position.Y}]"
                },
                testReporter.Errors);

            Assert.AreEqual(new Dictionary<IRover, Status>
            {
                {one, Status.FellOff}
            }, testReporter.Dictionary);
            Assert.AreEqual(Direction.S, one.Direction);
            Assert.AreEqual(new Coordinates(1, -1), one.Position);
        }

        [Test]
        public void ShouldReportCrashWhileMoving()
        {
            IPlateau p = new MarsPlateau(new Coordinates(5, 5));
            IRover one = new MarsRover.MarsRover(1, new Coordinates(1, 2), Direction.N,
                new List<Command>
                {
                    Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.L, Command.M, Command.M
                });
            IRover two = new MarsRover.MarsRover(2, new Coordinates(0, 3), Direction.E,
                new List<Command>
                {
                    Command.M, Command.M, Command.R, Command.M, Command.M, Command.R, Command.M, Command.R, Command.R,
                    Command.M
                });

            var testReporter = new TestReporter();
            new Houston(p, new List<IRover> {one, two}).Control(testReporter);

            Assert.AreEqual(new List<string>
                {
                    $"Rover {two.Id} crashed into another Rover {one.Id} at [{one.Position.X}, {one.Position.Y}]"
                },
                testReporter.Errors);

            Assert.AreEqual(new Dictionary<IRover, Status>
            {
                {one, Status.Ok},
                {two, Status.Crashed}
            }, testReporter.Dictionary);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 3), one.Position);
            Assert.AreEqual(Direction.E, two.Direction);
            Assert.AreEqual(new Coordinates(1, 3), two.Position);
        }

        private class TestReporter : IReporter
        {
            public readonly IDictionary<IRover, Status> Dictionary = new Dictionary<IRover, Status>();
            public readonly IList<string> Errors = new List<string>();

            public void Report(IRover rover, Status status)
            {
                Dictionary.Add(rover, status);
            }

            public void Error(string message)
            {
                Errors.Add(message);
            }
        }
    }
}