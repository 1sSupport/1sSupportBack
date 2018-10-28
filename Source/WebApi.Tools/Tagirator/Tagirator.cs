using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.EF.Models;

namespace WebApi.Tools
{
    public class Tagirator
    {
        private readonly ICollection<TagirationArticle> _tagirationArticles = new List<TagirationArticle>();

        private readonly EFContext _context;

        public Tagirator(EFContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        private static bool IsArticlesNullOrEmpty(IEnumerable<Article> articles)
        {
            return articles == null || !articles.Any();
        }

        private void AddArticlesFromContext()
        {
            if (IsArticlesNullOrEmpty(_context.Articles))
            {
                throw new ArgumentException("Необходимо добавить стать в БД");
            }
            /*add range*/
            var tagirationArticles = _context.Articles.Select(article => new TagirationArticle(article)).ToList();

            foreach (var tagirationArticle in tagirationArticles)
            {
                _tagirationArticles.Add(tagirationArticle);
            }
        }

        private readonly IDictionary<string, WordInfo> _globalWordsObject = new Dictionary<string, WordInfo>();

        private void SetGlobalWords()
        {
            foreach (var article in _tagirationArticles)
            {
                foreach (var word in article.CleanWords)
                {
                    if (_globalWordsObject.ContainsKey(word))
                    {
                        var value = article.GetWordFrequancy(word);
                        _globalWordsObject[word].Freq += value;
                    }
                    else
                    {
                        var wordObject = new WordInfo { Freq = article.GetWordFrequancy(word) };

                        _globalWordsObject.Add(word, wordObject);
                    }
                }
            }
        }

        private double GetRatioFreq(string key)
        {
            if (!_globalWordsObject.ContainsKey(key))
            {
                return 0;
            }

            // ReSharper disable once PossibleLossOfFraction
            double v = _globalWordsObject[key].Freq / _tagirationArticles.Count;
            return v;
        }

        private void SetGlobalWordsRate()
        {
            if (!_globalWordsObject.Any())
            {
                SetGlobalWords();
            }

            foreach (var key in _globalWordsObject.Keys)
            {
                var wordObject = _globalWordsObject[key];
                wordObject.Rate = GetRatioFreq(key);
            }
        }

        private void SetLocalRate()
        {
            foreach (var article in _tagirationArticles)
            {
                foreach (var word in article.CleanWords)
                {
                    article.SetWordRate(word, _globalWordsObject[word].Rate);
                }
            }
        }

        private static void SetTagInArticle(Article article, Tag tag, double weight)
        {
            article.ArticleTag.Add(new ArticleTag(weight, article, tag));
            article.EditDate = DateTime.Now;
        }

        public void SetTagsInArticle()
        {
            if (_context == null)
            {
                throw new NullReferenceException("Необходимо сначала добавить EFContext со статьями. Воспользуйтесь методом AddContextForTagging");
            }

            AddArticlesFromContext();

            SetGlobalWords();
            SetGlobalWordsRate();
            SetLocalRate();

            SetCurrentTag();
        }

        private void ClearArticleTagTable()
        {
            var articletags = (from at in _context.ArticleTags select at).ToList();
            _context.ArticleTags.RemoveRange(articletags);
        }

        private void SetCurrentTag()
        {
            ClearArticleTagTable();

            foreach (var tagirationArticle in _tagirationArticles)
            {
                tagirationArticle.Article.ArticleTag.Clear();

                var tagsAndRate = tagirationArticle.GetTagsAndWeight().Take(10);

                foreach (var pair in tagsAndRate)
                {
                    var dbTag = (from t in _context.Tags where t.Value == pair.Key select t).FirstOrDefault();
                    SetTagInArticle(tagirationArticle.Article, dbTag ?? new Tag(pair.Key), pair.Value);
                }
            }
        }
    }

    internal class WordInfo
    {
        public int Freq { get; set; }

        public double Rate { get; set; }
    }
}