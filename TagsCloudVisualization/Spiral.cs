using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private const double StepSpiral = 2;
        private const double StepAngle = Math.PI / 36;
        private const double SpiralСoefficient = StepSpiral / (2 * Math.PI);
        private readonly TagValidator validator;

        public Spiral(Point centre, int width, int height)
        {
            if (centre.X < 0 || centre.Y < 0)
                throw new ArgumentException("centre should have non negative coordinats");
            if (centre.X > width || centre.Y > height)
                throw new ArgumentException("centre should have x-coordinats less image width " +
                                            "and y-coordinats less image height");
            Centre = centre;
            Rectangles = new List<Rectangle>();
            validator = new TagValidator(width, height);
        }

        public List<Rectangle> Rectangles { get; }
        public Point Centre { get; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newRectangle = new Rectangle(SearchUpLeftCornerPoint(rectangleSize), rectangleSize);
            if (!validator.RectangleIsCorrect(newRectangle))
                throw new TagValidatorException("It is impossible to arrange rectangle on spiral.\n" +
                                                "Rectangle goes beyond the boundaries of the map");
            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        public List<Point> GetAllRectanglesPoints()
        {
            var points = new List<Point>();
            foreach (var rectangle in Rectangles)
            {
                points.Add(rectangle.Location);
                points.Add(new Point(rectangle.X, rectangle.Y + rectangle.Height));
                points.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y));
                points.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            }
            return points;
        }

        private Point SearchUpLeftCornerPoint(Size rectangleSize)
        {
            if (!validator.SizeIsCorrect(rectangleSize))
                throw new ArgumentException("width or height is incorrect");

            var angle = 0.0;
            while (true)
            {
                var point = new PointF(Convert.ToSingle(SpiralСoefficient * angle * Math.Sin(angle)),
                    Convert.ToSingle(SpiralСoefficient * angle * Math.Cos(angle)));
                if (!IsIntersectingWithRectangles(Point.Ceiling(point), rectangleSize))
                {
                    var pointCeil = Point.Ceiling(point);
                    return new Point(pointCeil.X + Centre.X, pointCeil.Y + Centre.Y);
                }
                angle += StepAngle;
            }
        }

        private bool IsIntersectingWithRectangles(Point possibleUpLeftCorner, Size size)
        {
            var point = new Point(possibleUpLeftCorner.X + Centre.X, possibleUpLeftCorner.Y + Centre.Y);
            var possibleRectangle = new Rectangle(point, size);
            return Rectangles.Any(r => r.IntersectsWith(possibleRectangle));
        }
    }
}