using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public class Drawer
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public Drawer(int widht, int height)
        {
            bitmap = new Bitmap(widht, height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawRectangles(List<Rectangle> rectangles, Color color)
        {
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(new Pen(color), rectangle);
        }

        public void DrawPoligon(List<Point> points, Color color)
        {
            points.Add(points[0]);
            for (var i = 1; i < points.Count; i++)
                graphics.DrawLine(new Pen(color), points[i - 1], points[i]);
        }

        public void DrawCurve(Point[] points, Color color)
        {
            graphics.DrawCurve(new Pen(color), points);
        }

        public void SaveImage(string path)
        {
            bitmap.Save(path);
        }

        public void SetBitmap(PictureBox pictureBox)
        {
            pictureBox.Image = bitmap;
        }
    }
}