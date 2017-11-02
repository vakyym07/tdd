using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Spiral spiral;

        public CircularCloudLayouter(Point centre, int width, int height)
        {
            spiral = new Spiral(centre, width, height);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return spiral.PutNextRectangle(rectangleSize);
        }
    }
}