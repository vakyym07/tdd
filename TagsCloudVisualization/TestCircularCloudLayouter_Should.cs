using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class TestCircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
               layouter = new CircularCloudLayouter(new Point(500, 500));
        }

        [TestCase(-1, 2, TestName = "when coordinat x is negative")]
        [TestCase(1, -10, TestName = "when coordinat y is negative")]
        [TestCase(-1, -2, TestName = "when coordinats x and y are negative")]
        public void CircularCloudLayouterConstructor_Should_ThrowArgumentException(int x, int y)
        {
            Action action = () => new CircularCloudLayouter(new Point(x, y));
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(0, 0, TestName = "when both a width and height of zero")]
        [TestCase(-10, 5, TestName = "when width is negative")]
        [TestCase(20, -30, TestName = "when height is negative")]
        [TestCase(-20, -30, TestName = "when width and height are negative")]
        public void PutNextRectangle_Should_ThrowArgumentException(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_Should_PutRectangleThatIsNotIntersectWithAnyRectangles()
        {
            var sizes = new Size[] { new Size(10, 20), new Size(30, 5), new Size(40, 60) };
            var isIntersecting = GenereteTagsCloud(sizes)
                .Aggregate(false, (current, rectangle) => current || layouter.Rectangles.Where(r => r != rectangle)
                                                              .Any(r => r.IntersectsWith(rectangle)));
            isIntersecting.Should().BeFalse();
        }

        private IEnumerable<Rectangle> GenereteTagsCloud(IEnumerable<Size> sizes)
        {
            return sizes.Select(size => layouter.PutNextRectangle(size)).ToList();
        }
    }
}
