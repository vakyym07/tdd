using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Centre { get; }
        public List<Rectangle> Rectangles { get; }

        private const double StepSpiral = 2;
        private const double StepAngle = Math.PI / 36;
        private const double SpiralСoefficient = StepSpiral / (2 * Math.PI);

        public CircularCloudLayouter(Point centre)
        {
            if (centre.X < 0 || centre.Y < 0)
                throw new ArgumentException("centre should have non negative coordinats");
            Centre = centre;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty)
                throw new ArgumentException("width and height should be greater than zero");
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("width and height should be non negative number");
            var newRectangle = new Rectangle(SearchUpLeftCornerPoint(rectangleSize), rectangleSize);
            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        public bool IsIntersectingWithRectangles(Point possibleUpLeftCorner, Size size)
        {
            var point = new Point(possibleUpLeftCorner.X + Centre.X, possibleUpLeftCorner.Y + Centre.Y);
            var possibleRectangle = new Rectangle(point, size);
            return Rectangles.Any(r => r.IntersectsWith(possibleRectangle));
        }

        public Point SearchUpLeftCornerPoint(Size size)
        {
            var angle = 0.0;
            while (true)
            {
                var point = new PointF(Convert.ToSingle(SpiralСoefficient * angle * Math.Sin(angle)),
                                        Convert.ToSingle(SpiralСoefficient * angle * Math.Cos(angle)));
                if (!IsIntersectingWithRectangles(Point.Ceiling(point), size))
                {
                    var pointCeil = Point.Ceiling(point);
                    return new Point(pointCeil.X + Centre.X, pointCeil.Y + Centre.Y);
                }
                angle += StepAngle;
            }
        }
    }
}
