using System.Collections.Generic;
using System.Linq;
using WebApi.EF.Models;
using WebApi.Tagirator.Parser;

namespace WebApi.Tagirator
{
    public class TagirationArticle
    {
        private readonly Article _article;

        public Article Article => _article;

        public TagirationArticle(Article article)
        {
            _article = article;
        }

        private IEnumerable<string> _cleanWords;

        private IEnumerable<string> AllWords
        {
            get
            {
                if (_cleanWords != null)
                {
                    return _cleanWords;
                }
                else
                {
                    _cleanWords = GetCleanWords();
                    return _cleanWords;
                }
            }
        }

        private IEnumerable<string> _hasSetWord;

        public IEnumerable<string> CleanWords => _hasSetWord ?? (_hasSetWord = new HashSet<string>(AllWords));

        private IEnumerable<string> GetCleanWords()
        {
            var words = FilteredText.GetWords(_article.Text);
            return words;
        }

        private IDictionary<string, WordInfo> _wordsInfo = new Dictionary<string, WordInfo>();

        private void SetFrequancy()
        {
            foreach (var cleanWord in AllWords)
            {
                if (_wordsInfo.ContainsKey(cleanWord))
                {
                    _wordsInfo[cleanWord].Freq++;
                }
                else
                {
                    var wordInfo = new WordInfo { Freq = 1 };

                    _wordsInfo.Add(cleanWord, wordInfo);
                }
            }
        }

        public void SetWordRate(string word, double globalRate)
        {
            if (!_wordsInfo.Any())
            {
                SetFrequancy();
            }

            var wordInfo = _wordsInfo[word];
            wordInfo.Rate = wordInfo.Freq - globalRate;
        }

        private void SortByRate()
        {
            //var word = _wordsInfo.OrderByDescending(p => p.Value.Rate);
            _wordsInfo = _wordsInfo.OrderByDescending(p => p.Value.Rate).ToDictionary(p => p.Key, p => p.Value);
        }

        public IDictionary<string, WordInfo> GetTagsInArticle()
        {
            SortByRate();

            return _wordsInfo;
        }

        public int GetWordFrequancy(string word)
        {
            if (!_wordsInfo.Any())
            {
                SetFrequancy();
            }

            return !_wordsInfo.ContainsKey(word) ? 0 : _wordsInfo[word].Freq;
        }
    }
}