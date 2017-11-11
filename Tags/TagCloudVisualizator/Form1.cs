using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Draw;
using TextCloud;

namespace TagCloudVisualizator
{
    public partial class Form1 : Form
    {
        private readonly Drawer drawer;
        private readonly TextCloudHandler handler;

        public Form1()
        {
            InitializeComponent();
            drawer = new Drawer(1900, 1200);
            handler = new TextCloudHandler(new FileReader("text.txt"), new Point(550, 400),
                1900, 1200, "Arial", drawer.GetStringSize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var randomGen = new Random();
            foreach (var element in handler.PutNextWord())
            {
                var color = GenerateColor(randomGen);
                drawer.DrawRectangle(element.Frame, color);
                drawer.DrawString(element.Word, element.StringFont,
                    new SolidBrush(color), element.Frame);
            }
            drawer.SetBitmap(pictureBox);
            drawer.SaveImage("text_cloud.jpg");
        }

        private Color GenerateColor(Random randomGen)
        {
            var colors = new[] {Color.Blue, Color.Red, Color.Green};
            return colors[randomGen.Next(0, colors.Length - 1)];
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

        private IEnumerable<Size> GenerateRandomSize(int countRectangle)
        {
            var rnd = new Random();
            for (var i = 0; i < countRectangle; i++)
                yield return new Size(rnd.Next(50, 70), rnd.Next(5, 30));
        }
    }
}