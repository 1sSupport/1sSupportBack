using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebApi.Tagirator.Parser
{
   public static class FilteredText
    {
        public static IEnumerable<string> GetWords(string text)
        {
            // get all significant words
            var words = Regex.Split(Clean(text), $@"[ \n\t\r$+<>№=]");

            var uniqueValues = new List<string>();
            // remove endings of words

            foreach (var word in words)
            {
                if (!Regex.Match(word.ToLower(), @"[а-яА-ЯЁёa-zA-Z]").Success)
                {
                    continue;
                }

                uniqueValues.Add(PorterStemmer.TransformingWord(word));
            }

            uniqueValues.RemoveAll((s) => s.Equals(""));

            return uniqueValues;
        }

        private static string Clean(string text)
        {
            // remove all digits and punctuation marks
            if (text == null)
            {
                return "";
            }

            var fixtext = Regex.Replace(text.ToLower(), @"[\\pP\\d]", " ");
            fixtext = Regex.Replace(fixtext, @"[.,\/#!$%\^&\*;:{}=\-_`~()]", " ");
            return fixtext;
        }
    }
}