using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    [TestFixture]
    internal class CircularCloudLayouterTests_Should
    {
        [SetUp]
        public void SetUp()
        {
            spiralCenter = new Point(500, 500);
            layouter = new CircularCloudLayouter(spiralCenter, 1900, 1200);
            rectangles = GenerateRandomSize(200).Select(s => layouter.PutNextRectangle(s)).ToList();
            drawer = new Drawer(1900, 1200);
        }

        [TearDown]
        public void TearDown()
        {
            var testContext = new TestContext(TestExecutionContext.CurrentContext);
            if (testContext.Result.Outcome.Status != TestStatus.Failed) return;
            var savePath = Path.Combine(workDirectory, "log", $"{testContext.Test.Name}_IsFailed.jpg");
            if (!Directory.Exists(Path.Combine(workDirectory, "log")))
                Directory.CreateDirectory(Path.Combine(workDirectory, "log"));
            var hull = ConvexHullBuilder.GetConvexHull(GetAllRectanglesPoints(rectangles)).ToList();
            drawer.DrawRectangles(rectangles, Color.Blue);
            drawer.DrawPoligon(hull, Color.Red);
            drawer.SaveImage(savePath);
            TestContext.Write($"Tag cloud visualization saved to file <{savePath}>");
        }

        private const double PerSameToCircle = 60;
        private CircularCloudLayouter layouter;
        private Point spiralCenter;
        private readonly string workDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private Drawer drawer;
        private List<Rectangle> rectangles;

        private static object[] _putNextRectangleSource =
        {
            new object[] {new List<Size> {new Size(20, 30), new Size(50, 10), new Size(10, 5)}},
            new object[] {new List<Size> {new Size(40, 35), new Size(6, 7), new Size(100, 200)}},
            new object[] {new List<Size> {new Size(100, 100), new Size(2, 100), new Size(5, 5)}}
        };

        private bool IsSameFigure(List<Point> hullPoints, double circleRadius)
        {
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

        private List<Point> GetAllRectanglesPoints(List<Rectangle> rectangles)
        {
            var points = new List<Point>();
            foreach (var rectangle in rectangles)
            {
                points.Add(rectangle.Location);
                points.Add(new Point(rectangle.X, rectangle.Y + rectangle.Height));
                points.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y));
                points.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            }
            return points;
        }

        [Test]
        [TestCaseSource(nameof(_putNextRectangleSource))]
        public void PutNextRectangle_Should_PutRectangle_That_IsNotIntersectWithAnyRectangles(List<Size>lastSizes)
        {
            var isIntersecting = rectangles
                .Aggregate(false, (current, rectangle) => current || rectangles.Where(r => r != rectangle)
                                                              .Any(r => r.IntersectsWith(rectangle)));
            isIntersecting.Should().BeFalse();
        }

        [Test]
        public void TagsCloud_ShouldBe_SimilarToCircle()
        {
            var hull = ConvexHullBuilder.GetConvexHull(GetAllRectanglesPoints(rectangles)).ToList();
            IsSameFigure(hull, GetMaxDistanceToOutermostPoint(spiralCenter, hull)).Should().BeTrue();
        }
    }
}