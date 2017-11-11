using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagCloudVisualizationTests
{
    [TestFixture]
    internal class SpiralTests_Should
    {
        [TestCase(-1, 0, 100, 100, TestName = "when center x-coordinat is less than zero")]
        [TestCase(0, -1, 100, 100, TestName = "when center y-coordinat is less than zero")]
        [TestCase(-1, -1, 100, 100, TestName = "when both center coordinats are less than zero")]
        [TestCase(500, 0, 400, 100, TestName = "when center x-coordinat is greater than spiral width")]
        [TestCase(0, 500, 100, 400, TestName = "when center y-coordinat is greater than spiral height")]
        [TestCase(500, 500, 400, 400, TestName =
            "when center both center coordinats are greater than spiral widht and height")]
        public void SpiralConstructor_Should_ThrowArgumentException(int xCenter, int yCenter, int spiralWidth,
            int height)
        {
            Action action = () => new Spiral(new Point(xCenter, yCenter), spiralWidth, height);
            action.ShouldThrow<ArgumentException>();
        }
    }
}