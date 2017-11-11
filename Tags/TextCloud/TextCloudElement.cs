using System.Drawing;

namespace TextCloud
{
    public class TextCloudElement
    {
        public TextCloudElement(string word, Rectangle frame, Font font)
        {
            Word = word;
            Frame = frame;
            StringFont = font;
        }

        public string Word { get; }
        public Rectangle Frame { get; }
        public Font StringFont { get; }
    }
}