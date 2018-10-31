// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagirationArticle.cs" company="">
//
// </copyright>
// <summary>
//   The tagiration article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Tagirator
{
    using System.Collections.Generic;
    using System.Linq;

    using WebApi.EF.Models;
    using WebApi.Tools.Parser;

    /// <summary>
    /// The tagiration article.
    /// </summary>
    public class TagirationArticle
    {
        /// <summary>
        /// The all words.
        /// </summary>
        private IEnumerable<string> allWords;

        /// <summary>
        /// The has set word.
        /// </summary>
        private IEnumerable<string> hasSetWord;

        /// <summary>
        /// The words info.
        /// </summary>
        private IDictionary<string, WordInfo> wordsInfo = new Dictionary<string, WordInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TagirationArticle"/> class.
        /// </summary>
        /// <param name="article">
        /// The article.
        /// </param>
        public TagirationArticle(Article article)
        {
            Article = article;
        }

        /// <summary>
        /// Gets the article.
        /// </summary>
        public Article Article { get; }

        /// <summary>
        /// The clean words.
        /// </summary>
        public IEnumerable<string> CleanWords => hasSetWord ?? (hasSetWord = new HashSet<string>(AllWords));

        /// <summary>
        /// Gets the all words.
        /// </summary>
        private IEnumerable<string> AllWords
        {
            get
            {
                if (allWords != null)
                {
                    return allWords;
                }
                else
                {
                    allWords = GetCleanAllWords();
                    return allWords;
                }
            }
        }

        /// <summary>
        /// The get tags and weight.
        /// </summary>
        /// <returns>
        /// The <see cref="IDictionary{TKey,TValue}"/>.
        /// </returns>
        public IDictionary<string, double> GetTagsAndWeight()
        {
            SortByRate();

            return (from tw in wordsInfo select new { Key = tw.Key, Value = tw.Value.Rate }).ToDictionary(
                p => p.Key,
                p => p.Value);
        }

        /// <summary>
        /// The get word frequancy.
        /// </summary>
        /// <param name="word">
        /// The word.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetWordFrequancy(string word)
        {
            if (!wordsInfo.Any())
            {
                SetFrequancy();
            }

            return !wordsInfo.ContainsKey(word) ? 0 : wordsInfo[word].Freq;
        }

        /// <summary>
        /// The set word rate.
        /// </summary>
        /// <param name="word">
        /// The word.
        /// </param>
        /// <param name="globalRate">
        /// The global rate.
        /// </param>
        public void SetWordRate(string word, double globalRate)
        {
            if (!wordsInfo.Any())
            {
                SetFrequancy();
            }

            var wordInfo = wordsInfo[word];
            wordInfo.Rate = wordInfo.Freq - globalRate;
        }

        /// <summary>
        /// The get clean all words.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        private IEnumerable<string> GetCleanAllWords()
        {
            var words = FilteredText.GetWords(Article.Text);
            return words;
        }

        /// <summary>
        /// The set frequancy.
        /// </summary>
        private void SetFrequancy()
        {
            foreach (var cleanWord in AllWords)
            {
                if (wordsInfo.ContainsKey(cleanWord))
                {
                    wordsInfo[cleanWord].Freq++;
                }
                else
                {
                    var wordInfo = new WordInfo { Freq = 1 };

                    wordsInfo.Add(cleanWord, wordInfo);
                }
            }
        }

        /// <summary>
        /// The sort by rate.
        /// </summary>
        private void SortByRate()
        {
            // var word = _wordsInfo.OrderByDescending(p => p.Value.Rate);
            wordsInfo = wordsInfo.OrderByDescending(p => p.Value.Rate).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}