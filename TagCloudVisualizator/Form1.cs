using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization;

namespace TagCloudVisualizator
{
    public partial class Form1 : Form
    {
        private readonly Drawer drawer;
        private readonly CircularCloudLayouter layouter;

        public Form1()
        {
            InitializeComponent();
            layouter = new CircularCloudLayouter(new Point(550, 400), 1900, 1200);
            drawer = new Drawer(1900, 1200);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var rectangles = GenerateRandomSize(200).Select(s => layouter.PutNextRectangle(s)).ToList();
            var hull = ConvexHullBuilder.GetConvexHull(GetAllRectanglesPoints(rectangles)).ToList();
            drawer.DrawRectangles(rectangles, Color.Blue);
            drawer.DrawPoligon(hull, Color.Red);
            drawer.SetBitmap(pictureBox);
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