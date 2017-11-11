using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextParser
{
    public class Parser
    {
        private readonly string text;

        public Parser(string text)
        {
            this.text = text;
        }

        public List<WordInformation> GetStatistic()
        {
            var statistic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var word in GetWords(text))
                PutToDictionary(statistic, word);
            return statistic
                .OrderByDescending(pair => pair.Value)
                .Select(pair => new WordInformation(pair.Key, pair.Value))
                .ToList();
        }

        public int GetAmountWords()
        {
            return GetWords(text).Count();
        }

        private static IEnumerable<string> GetWords(string text)
        {
            return from Match match in Regex.Matches(text, @"\w+") select match.Value;
        }

        private static void PutToDictionary(Dictionary<string, int>dictionary, string word)
        {
            if (dictionary.ContainsKey(word))
                dictionary[word] += 1;
            else dictionary[word] = 1;
        }
    }
}