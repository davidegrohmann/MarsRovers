using System;
using System.IO;
using MarsRover;
using NUnit.Framework;

namespace MarsRoverTest
{
    public class ParserTest
    {
        [Test]
        public void ShouldParseConfiguration()
        {
            var r = new StringReader(@"5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM");

            var parser = new Parser(r);
            var plateau = parser.ParsePlateau();
            var one = parser.ParseRover(1);
            var two = parser.ParseRover(2);
            var three = parser.ParseRover(3);
            
            Assert.NotNull(plateau);
            Assert.AreEqual(new Coordinates(5, 5), plateau.Size);
            
            Assert.NotNull(one);
            Assert.AreEqual(1, one.Id);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 2), one.Position);
            
            Assert.NotNull(two);
            Assert.AreEqual(2, two.Id);
            Assert.AreEqual(Direction.E, two.Direction);
            Assert.AreEqual(new Coordinates(3, 3), two.Position);

            Assert.Null(three);
        }
        
        [Test]
        public void ShouldParseConfigurationNoMoves()
        {
            var r = new StringReader(@"5 5
1 2 N

3 3 E");

            var parser = new Parser(r);
            var plateau = parser.ParsePlateau();
            var one = parser.ParseRover(1);
            var two = parser.ParseRover(2);
            var three = parser.ParseRover(3);
            
            Assert.NotNull(plateau);
            Assert.AreEqual(new Coordinates(5, 5), plateau.Size);
            
            Assert.NotNull(one);
            Assert.AreEqual(1, one.Id);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 2), one.Position);
            
            Assert.NotNull(two);
            Assert.AreEqual(2, two.Id);
            Assert.AreEqual(Direction.E, two.Direction);
            Assert.AreEqual(new Coordinates(3, 3), two.Position);

            Assert.Null(three);
        }
        
        [Test]
        public void ShouldParseConfigurationNoRovers()
        {
            var r = new StringReader(@"5 5");

            var parser = new Parser(r);
            var plateau = parser.ParsePlateau();
            var one = parser.ParseRover(1);
            
            Assert.NotNull(plateau);
            Assert.AreEqual(new Coordinates(5, 5), plateau.Size);
            
            Assert.Null(one);
        }

        [Test]
        public void ShouldParseConfigurationWithExtraEmptyLines()
        {
            var r = new StringReader(@"5 5

1 2 N
LMLMLMLMM

3 3 E
MMRMMRMRRM

");

            var parser = new Parser(r);
            var plateau = parser.ParsePlateau();
            var one = parser.ParseRover(1);
            var two = parser.ParseRover(2);
            var three = parser.ParseRover(3);
            
            Assert.NotNull(plateau);
            Assert.AreEqual(new Coordinates(5, 5), plateau.Size);
            
            Assert.NotNull(one);
            Assert.AreEqual(1, one.Id);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1, 2), one.Position);
            
            Assert.NotNull(two);
            Assert.AreEqual(2, two.Id);
            Assert.AreEqual(Direction.E, two.Direction);
            Assert.AreEqual(new Coordinates(3, 3), two.Position);

            Assert.Null(three);
        }
        
        [Test]
        public void ShouldReportErrorOnParsingPlateauNoNumber()
        {
            var r = new StringReader(@"5 A");

            var parser = new Parser(r);

            try
            {
                parser.ParsePlateau();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed coordinates: '5 A'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingPlateauMissingAPart()
        {
            var r = new StringReader(@"5");

            var parser = new Parser(r);

            try
            {
                parser.ParsePlateau();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed mars plateau coordinates: '5'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingPlateauMissingAPart2()
        {
            var r = new StringReader(@" 5");

            var parser = new Parser(r);

            try
            {
                parser.ParsePlateau();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed coordinates: ' 5'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReturnNullOnParsingPlateauEmpty()
        {
            var r = new StringReader(@"");

            var parser = new Parser(r);
            Assert.Null(parser.ParsePlateau());
        }
        
        [Test]
        public void ShouldReportErrorOnParsingPlateauNegativeNumber()
        {
            var r = new StringReader(@"-1 5");

            var parser = new Parser(r);

            try
            {
                parser.ParsePlateau();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Coordinates must be positive, parse [-1, 5]", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverNoNumber()
        {
            var r = new StringReader(@"5 A N");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed coordinates: '5 A'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverMissingAPart()
        {
            var r = new StringReader(@"5 5");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed mars rover landing coordinates and direction: '5 5'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverMissingAPart2()
        {
            var r = new StringReader(@" 5 N");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed coordinates: ' 5'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverMissingAPart3()
        {
            var r = new StringReader(@"5  N");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed coordinates: '5 '", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverUnknownDirection()
        {
            var r = new StringReader(@"5 5 P");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Unknown direction: 'P'", e.Message);
            }
        }

        [Test]
        public void ShouldReturnNullOnParsingRoverEmpty()
        {
            var r = new StringReader(@"");

            var parser = new Parser(r);
            Assert.Null(parser.ParseRover(0));
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverNegativeNumber()
        {
            var r = new StringReader(@"-1 5 N");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Coordinates must be positive, parse [-1, 5]", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorOnParsingRoverUnknownCommand()
        {
            var r = new StringReader(@"1 5 N
T");

            var parser = new Parser(r);

            try
            {
                parser.ParseRover(0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Unknown command: 'T'", e.Message);
            }
        }
        
        [Test]
        public void ShouldReportErrorIfEmptyLineBetweenCoordinatesAndCommands()
        {
            var r = new StringReader(@"
1 2 N

LMLMLMLMM

");

            var parser = new Parser(r);
            
            var one = parser.ParseRover(0);
            Assert.NotNull(one);
            Assert.AreEqual(Direction.N, one.Direction);
            Assert.AreEqual(new Coordinates(1,2), one.Position);
            Assert.AreEqual(0, one.Id);
            try
            {
                parser.ParseRover(1);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Malformed mars rover landing coordinates and direction: 'LMLMLMLMM'", e.Message);
            }
        }
    }
}