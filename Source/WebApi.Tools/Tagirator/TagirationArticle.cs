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
    ///     The tagiration article.
    /// </summary>
    public class TagirationArticle
    {
        /// <summary>
        ///     The all words.
        /// </summary>
        private IEnumerable<string> allWords;

        /// <summary>
        ///     The has set word.
        /// </summary>
        private IEnumerable<string> hasSetWord;

        /// <summary>
        ///     The words info.
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
            this.Article = article;
        }

        /// <summary>
        ///     Gets the article.
        /// </summary>
        public Article Article { get; }

        /// <summary>
        ///     The clean words.
        /// </summary>
        public IEnumerable<string> CleanWords =>
            this.hasSetWord ?? (this.hasSetWord = new HashSet<string>(this.AllWords));

        /// <summary>
        ///     Gets the all words.
        /// </summary>
        private IEnumerable<string> AllWords
        {
            get
            {
                if (this.allWords != null)
                {
                    return this.allWords;
                }
                else
                {
                    this.allWords = this.GetCleanAllWords();
                    return this.allWords;
                }
            }
        }

        /// <summary>
        ///     The get tags and weight.
        /// </summary>
        /// <returns>
        ///     The <see cref="IDictionary{TKey,TValue}" />.
        /// </returns>
        public IDictionary<string, double> GetTagsAndWeight()
        {
            this.SortByRate();

            return (from tw in this.wordsInfo select new { Key = tw.Key, Value = tw.Value.Rate }).ToDictionary(
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
            if (!this.wordsInfo.Any()) this.SetFrequancy();

            return !this.wordsInfo.ContainsKey(word) ? 0 : this.wordsInfo[word].Freq;
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
            if (!this.wordsInfo.Any()) this.SetFrequancy();

            var wordInfo = this.wordsInfo[word];
            wordInfo.Rate = wordInfo.Freq - globalRate;
        }

        /// <summary>
        ///     The get clean all words.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable{T}" />.
        /// </returns>
        private IEnumerable<string> GetCleanAllWords()
        {
            var words = FilteredText.GetWords(this.Article.GetText());
            return words;
        }

        /// <summary>
        ///     The set frequancy.
        /// </summary>
        private void SetFrequancy()
        {
            foreach (var cleanWord in this.AllWords)
                if (this.wordsInfo.ContainsKey(cleanWord))
                {
                    this.wordsInfo[cleanWord].Freq++;
                }
                else
                {
                    var wordInfo = new WordInfo { Freq = 1 };

                    this.wordsInfo.Add(cleanWord, wordInfo);
                }
        }

        /// <summary>
        ///     The sort by rate.
        /// </summary>
        private void SortByRate()
        {
            // var word = _wordsInfo.OrderByDescending(p => p.Value.Rate);
            this.wordsInfo = this.wordsInfo.OrderByDescending(p => p.Value.Rate).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}