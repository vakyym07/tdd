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

        public Spiral(Point centre, int width, int height)
        {
            if (centre.X < 0 || centre.Y < 0)
                throw new ArgumentException("centre should have non negative coordinats");
            if (centre.X > width || centre.Y > height)
                throw new ArgumentException("centre should have x-coordinats less image width " +
                                            "and y-coordinats less image height");
            Centre = centre;
            Width = width;
            Height = height;
        }

        public Point Centre { get; }
        public int Width { get; }
        public int Height { get; }

        public IEnumerable<Point>GetNextPoint()
        {
            var angle = 0.0;
            while (true)
            {
                var point = new PointF(Convert.ToSingle(SpiralСoefficient * angle * Math.Sin(angle)),
                    Convert.ToSingle(SpiralСoefficient * angle * Math.Cos(angle)));
                var pointCeil = Point.Ceiling(point);
                if (pointCeil.X > Width || pointCeil.Y > Height)
                    break;
                yield return new Point(pointCeil.X + Centre.X, pointCeil.Y + Centre.Y);
                angle += StepAngle;
            }
        }
    }
}