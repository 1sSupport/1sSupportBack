// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilteredText.cs" company="">
//
// </copyright>
// <summary>
//   Defines the FilteredText type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Parser
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The filtered text.
    /// </summary>
    public static class FilteredText
    {
        /// <summary>
        /// The get words.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="useEnglish">
        /// The use English.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<string> GetWords(string text, bool useEnglish = false)
        {
            // get all significant words
            var words = Regex.Split(Clean(text), $@"[ \n\t\r$+<>№=]");

            var containsRegex = useEnglish ? @"[а-яА-ЯЁёa-zA-Z]" : @"[а-яА-ЯЁё]";

            var uniqueValues =
                (from word in words
                 where Regex.Match(word.ToLower(), containsRegex).Success
                 select PorterStemmer.TransformingWord(word)).ToList();

            // remove endings of words
            uniqueValues.RemoveAll((s) => s.Equals(string.Empty));
            uniqueValues.RemoveAll((s) => s.Length <= 2);

            return uniqueValues;
        }

        /// <summary>
        /// The clean.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string Clean(string text)
        {
            // remove all digits and punctuation marks
            if (text == null)
            {
                return string.Empty;
            }

            var fixtext = Regex.Replace(text.ToLower(), @"[\\pP\\d]", " ");
            fixtext = Regex.Replace(fixtext, @"[.,\/#!$%\^&\*;:{}=\-_`~()]", " ");
            return fixtext;
        }
    }
}