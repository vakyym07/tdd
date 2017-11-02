using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class ConvexHullBuilder
    {
        public static IEnumerable<Point> GetConvexHull(List<Point> points)
        {
            if (points.Count == 1)
                return points;
            points.Sort(new PointComparator());
            var leastPoint = points[0];
            var greatestPoint = points[points.Count - 1];
            var upHull = new List<Point> {leastPoint};
            var downHull = new List<Point> {leastPoint};
            foreach (var currentPoint in points)
            {
                if (VectorMultiplication(leastPoint, currentPoint, greatestPoint) < 0)
                    AddToConvexHull(upHull, currentPoint, multiplicationResult => multiplicationResult >= 0);
                if (VectorMultiplication(leastPoint, currentPoint, greatestPoint) > 0)
                    AddToConvexHull(downHull, currentPoint, multiplicationResult => multiplicationResult <= 0);
            }
            upHull.Add(greatestPoint);
            downHull.Reverse();
            return upHull.Concat(downHull);
        }

        public static double GetSquareHull(List<Point> hullPoints)
        {
            var startCorner = hullPoints[0];
            double square = 0;
            for (var i = 2; i < hullPoints.Count; i++)
            {
                square += VectorMultiplication(hullPoints[i - 1], startCorner, hullPoints[i]) / 2.0;
            }
            return square;
        }

        private static void AddToConvexHull(List<Point>partOfConvexHull, Point addingPoint,
            Func<int, bool>excessCondition)
        {
            RemoveExcessPointsRelativelyPoint(addingPoint, partOfConvexHull,
                (firstPoint, secendPoint, thirdPoint) =>
                    excessCondition(VectorMultiplication(firstPoint, secendPoint, thirdPoint)));
            partOfConvexHull.Add(addingPoint);
        }

        private static void RemoveExcessPointsRelativelyPoint(Point relativelyPoint, List<Point> points,
            Func<Point, Point, Point, bool> isExcessPoint)
        {
            while (points.Count >= 2 &&
                   isExcessPoint(points[points.Count - 2], points[points.Count - 1], relativelyPoint))
                points.RemoveAt(points.Count - 1);
        }

        private static int VectorMultiplication(Point firtPoint, Point secondPoint, Point thirdPoint)
        {
            return (firtPoint.X - secondPoint.X) * (thirdPoint.Y - secondPoint.Y) -
                   (thirdPoint.X - secondPoint.X) * (firtPoint.Y - secondPoint.Y);
        }
    }

    internal class PointComparator : Comparer<Point>
    {
        public override int Compare(Point point, Point otherPoint)
        {
            if (point.X < otherPoint.X || point.X == otherPoint.X && point.Y < otherPoint.Y)
                return -1;
            if (point.X > otherPoint.X || point.X == otherPoint.X && point.Y > otherPoint.Y)
                return 1;
            return 0;
        }
    }
}