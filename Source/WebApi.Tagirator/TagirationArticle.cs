using System.Collections.Generic;
using System.Linq;
using WebApi.EF.Models;
using WebApi.Tagirator.Parser;

namespace WebApi.Tagirator
{
    public class TagirationArticle
    {
        public Article Article { get; }

        public TagirationArticle(Article article)
        {
            Article = article;
        }

        private IEnumerable<string> _allWords;

        private IEnumerable<string> AllWords
        {
            get
            {
                if (_allWords != null)
                {
                    return _allWords;
                }
                else
                {
                    _allWords = GetCleanAllWords();
                    return _allWords;
                }
            }
        }

        private IEnumerable<string> _hasSetWord;

        public IEnumerable<string> CleanWords => _hasSetWord ?? (_hasSetWord = new HashSet<string>(AllWords));

        private IEnumerable<string> GetCleanAllWords()
        {
            var words = FilteredText.GetWords(Article.Text);
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

        public ICollection<string> GetTagsInArticle()
        {
            SortByRate();

            return _wordsInfo.Keys;
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