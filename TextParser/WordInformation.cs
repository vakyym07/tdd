using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextParser
{
    public class WordInformation
    {
        public WordInformation(string word, int frequency)
        {
            Word = word;
            Frequency = frequency;
        }

        public string Word { get; }
        public int Frequency { get; }
    }
}
