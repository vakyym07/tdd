using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    internal class TestCircularCloudLayouter_Should
    {
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(500, 500), 1900, 1200);
            spiral = new Spiral(new Point(500, 500), 1900, 1200);
        }

        private const double PerSameToCircle = 60;
        private CircularCloudLayouter layouter;
        private Spiral spiral;

        private static object[] _putNextRectangleSource =
        {
            new object[] {new List<Size> {new Size(20, 30), new Size(50, 10), new Size(10, 5)}},
            new object[] {new List<Size> {new Size(40, 35), new Size(6, 7), new Size(100, 200)}},
            new object[] {new List<Size> {new Size(100, 100), new Size(2, 100), new Size(5, 5)}}
        };

        [TestCase(-1, 2, TestName = "when coordinat x is negative")]
        [TestCase(1, -10, TestName = "when coordinat y is negative")]
        [TestCase(-1, -2, TestName = "when coordinats x and y are negative")]
        [TestCase(2000, 1, TestName = "when coordinats x greater than width")]
        [TestCase(10, 1600, TestName = "when coordinats y greater than height")]
        public void SpiralConstructor_Should_ThrowArgumentException(int x, int y)
        {
            Action action = () => new CircularCloudLayouter(new Point(x, y), 800, 600);
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(0, 0, TestName = "when both a width and height of zero")]
        [TestCase(-10, 5, TestName = "when width is negative")]
        [TestCase(20, -30, TestName = "when height is negative")]
        [TestCase(-20, -30, TestName = "when width and height are negative")]
        [TestCase(2000, 30, TestName = "when width greater than image width")]
        [TestCase(100, 1300, TestName = "when height greater than image height")]
        public void PutNextRectangle_Should_ThrowArgumentException(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(1500, 20, TestName = "when rectangle width is too large")]
        [TestCase(20, 1100, TestName = "when rectangle width is too large")]
        public void PutNextRectangle_Should_ThrowTagValidatorException(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));
            action.ShouldThrow<TagValidatorException>();
        }

        private bool IsSameFigure(List<Point> hullPoints, double circleRadius)
        {
            var eps = 0.1;
            var perSimilar = ConvexHullBuilder.GetSquareHull(hullPoints) /
                             (Math.PI * circleRadius * circleRadius) * 100;
            return perSimilar > PerSameToCircle;
        }

        private double GetMaxDistanceToOutermostPoint(Point point, IEnumerable<Point> points)
        {
            return points.Max(p => Distanse(point, p));
        }

        private double Distanse(Point point, Point otherPoint)
        {
            return Math.Sqrt((point.X - otherPoint.X) * (point.X - otherPoint.X) -
                             (point.Y - otherPoint.Y) * (point.Y - otherPoint.Y));
        }

        private IEnumerable<Size> GenerateRandomSize(int countRectangle)
        {
            var rnd = new Random();
            for (var i = 0; i < countRectangle; i++)
                yield return new Size(rnd.Next(50, 70), rnd.Next(5, 30));
        }


        [Test]
        [TestCaseSource(nameof(_putNextRectangleSource))]
        public void PutNextRectangle_Should_PutRectangle_That_IsNotIntersectWithAnyRectangles(List<Size>lastSizes)
        {
            var rectangles = lastSizes.Select(s => layouter.PutNextRectangle(s)).ToList();
            var isIntersecting = rectangles
                .Aggregate(false, (current, rectangle) => current || rectangles.Where(r => r != rectangle)
                                                              .Any(r => r.IntersectsWith(rectangle)));
            isIntersecting.Should().BeFalse();
        }

        [Test]
        public void TagsCloud_ShouldBe_SimilarToCircle()
        {
            foreach (var size in GenerateRandomSize(200))
                spiral.PutNextRectangle(size);
            var hull = ConvexHullBuilder.GetConvexHull(spiral.GetAllRectanglesPoints()).ToList();
            IsSameFigure(hull, GetMaxDistanceToOutermostPoint(spiral.Centre, hull)).Should().BeTrue();
        }
    }
}