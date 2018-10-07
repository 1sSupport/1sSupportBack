using Api.Handler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Tagirator.Parser
{
    public class Article
    {
        private List<string> _tags = new List<string>();
        private Dictionary<string, int> _words = new Dictionary<string, int>();
        private readonly Dictionary<string, double> _wordsRate = new Dictionary<string, double>();
        private bool IsParsed => _words.Any();

        public string Title
        {
            get;
            private set;
        }

        public string Text { get; private set; }
        public IReadOnlyList<string> Tags => _tags;

        public IEnumerable<string> Words
        {
            get
            {
                if (IsParsed)
                {
                    return _words.Keys;
                }
                else
                {
                    ParseText();
                    return _words.Keys;
                }
            }
        }

        public IReadOnlyDictionary<string, double> WordsRate => _wordsRate;

        public Article(string title, string text)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
            {
                Title = title;
                Text = text;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public bool ParseText()
        {
            bool result = false;
            try
            {
                var parsedText = FilteredText.GetWords(Text);
                SetWordsFrequancy(parsedText);
                result = true;
            }
            catch (Exception e)
            {
                return result;
            }

            InitialDictionary();
            return result;
        }

        private void SetWordsFrequancy(IEnumerable<string> text)
        {
            foreach (string word in text)
            {
                if (_words.ContainsKey(word))
                {
                    _words[word]++;
                }
                else
                {
                    _words.Add(word, 1);
                }
            }
        }

        private void InitialDictionary()
        {
            foreach (var word in _words.Keys)
            {
                if (!(_wordsRate.ContainsKey(word)))
                {
                    _wordsRate.Add(word, 0);
                }
            }
        }

        public bool SetCurrentRate(string key, double value)
        {
            bool result = false;
            if (!IsParsed)
            {
                ParseText();
            }

            if (!WordsRate.ContainsKey(key))
            {
                return result;
            }
            else
            {
                var valueFreq = _words[key];
                _wordsRate[key] = valueFreq - value;
                result = true;
            }

            return result;
        }

        public bool SetTags()
        {
            bool result = false;
            if (!IsParsed)
            {
                ParseText();
            }

            try
            {
                var dict = _wordsRate.Where(p => (p.Value > 0)).OrderBy(p => p.Value).ToDictionary(p => (p.Key), p => (p.Value));
                var tags = (from word in dict.Keys select word).Take(10).ToList();
                _tags = tags;
                result = true;
            }
            catch (Exception e)
            {
                return result;
            }

            return result;
        }

        public int GetRepeatFrequency(string key)
        {
            if (!IsParsed)
            {
                ParseText();
            }
            if (_words.ContainsKey(key))
            {
                return _words[key];
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}