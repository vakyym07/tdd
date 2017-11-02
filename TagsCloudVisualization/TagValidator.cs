using System.Drawing;

namespace TagsCloudVisualization
{
    internal class TagValidator
    {
        public TagValidator(int imageWidth, int imageHeight)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
        }

        private int ImageWidth { get; }
        private int ImageHeight { get; }

        public bool RectangleIsCorrect(Rectangle rectangle)
        {
            if (rectangle.X < 0 || rectangle.X > ImageWidth || rectangle.X + rectangle.Width > ImageWidth)
                return false;
            return rectangle.Y >= 0 && rectangle.Y <= ImageHeight && rectangle.Y + rectangle.Height <= ImageHeight;
        }

        public bool SizeIsCorrect(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty)
                return false;
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                return false;
            return rectangleSize.Width <= ImageWidth && rectangleSize.Height <= ImageHeight;
        }
    }
}