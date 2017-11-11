using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization;
using TextParser;

namespace TextCloud
{
    public class TextCloudHandler
    {
        private readonly string fontName;
        private readonly Func<string, string, int, Size> getLayoutSize;
        private readonly CircularCloudLayouter layouter;
        private readonly IReader reader;

        public TextCloudHandler(IReader reader, Point cloudCenter, int imageWidth,
            int imageHeight, string fontName, Func<string, string, int, Size> getLayoutSize)
        {
            this.reader = reader;
            this.fontName = fontName;
            this.getLayoutSize = getLayoutSize;
            layouter = new CircularCloudLayouter(cloudCenter, imageWidth, imageHeight);
        }

        public IEnumerable<TextCloudElement> PutNextWord()
        {
            var parser = new Parser(reader.GetData());
            var statistic = parser.GetStatistic();
            var amountWords = parser.GetAmountWords();
            var maxPerOfTotal = (double) statistic[0].Frequency / amountWords * 100;
            foreach (var wordInformation in statistic)
            {
                var wordSize = GetWordSize(wordInformation, amountWords, maxPerOfTotal);
                yield return new TextCloudElement(
                    wordInformation.Word, layouter.PutNextRectangle(
                        getLayoutSize(wordInformation.Word, fontName, wordSize)),
                    new Font(fontName, wordSize));
            }
        }

        private int GetWordSize(WordInformation wordInformation, int amountWords, double maxPerOfTotal)
        {
            var perOfTotal = (double) wordInformation.Frequency / amountWords * 100;
            int stringSize;
            if (perOfTotal > 2 * maxPerOfTotal / 3.0)
                stringSize = (int) StringSize.Large;
            else if (perOfTotal > maxPerOfTotal / 3.0)
                stringSize = (int) StringSize.Midle;
            else stringSize = (int) StringSize.Small;
            return stringSize;
        }

        private enum StringSize
        {
            Large = 40,
            Midle = 25,
            Small = 15
        }
    }
}