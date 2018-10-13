using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.EF.Models;

namespace WebApi.Tagirator
{
    public static class Tagirator
    {
        private static readonly ICollection<TagirationArticle> TagirationArticles = new List<TagirationArticle>();

        private static EFContext _context;

        public static void AddContextForTagging(EFContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        private static bool IsArticlesNullOrEmpty(IEnumerable<Article> articles)
        {
            return articles == null || !articles.Any();
        }

        private static void AddArticlesFromContext()
        {
            if (IsArticlesNullOrEmpty(_context.Articles))
            {
                throw new ArgumentException("Необходимо добавить стать в БД");
            }
            /*add range*/
            var tagirationArticles = _context.Articles.Select(article => new TagirationArticle(article)).ToList();

            foreach (var tagirationArticle in tagirationArticles)
            {
                TagirationArticles.Add(tagirationArticle);
            }
        }

        private static readonly IDictionary<string, WordInfo> GlobalWordsObject = new Dictionary<string, WordInfo>();

        private static void SetGlobalWords()
        {
            foreach (var article in TagirationArticles)
            {
                foreach (var word in article.CleanWords)
                {
                    if (GlobalWordsObject.ContainsKey(word))
                    {
                        var value = article.GetWordFrequancy(word);
                        GlobalWordsObject[word].Freq += value;
                    }
                    else
                    {
                        var wordObject = new WordInfo { Freq = article.GetWordFrequancy(word) };

                        GlobalWordsObject.Add(word, wordObject);
                    }
                }
            }
        }

        private static double GetRatioFreq(string key)
        {
            if (!GlobalWordsObject.ContainsKey(key))
            {
                return 0;
            }

            // ReSharper disable once PossibleLossOfFraction
            double v = GlobalWordsObject[key].Freq / TagirationArticles.Count;
            return v;
        }

        private static void SetGlobalWordsRate()
        {
            if (!GlobalWordsObject.Any())
            {
                SetGlobalWords();
            }

            foreach (var key in GlobalWordsObject.Keys)
            {
                var wordObject = GlobalWordsObject[key];
                wordObject.Rate = GetRatioFreq(key);
            }
        }

        private static void SetLocalRate()
        {
            foreach (var article in TagirationArticles)
            {
                foreach (var word in article.CleanWords)
                {
                    article.SetWordRate(word, GlobalWordsObject[word].Rate);
                }
            }
        }

        private static void SetTagInArticle(Article article, Tag tag)
        {
            article.ArticleTag.Add(new ArticleTag(article, tag));
        }

        public static void SetTagsInArticle()
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

        private static void SetCurrentTag()
        {
            foreach (var tagirationArticle in TagirationArticles)
            {
                var tagsInArticle = tagirationArticle.GetTagsInArticle().Take(10);

                foreach (var tag in tagsInArticle)
                {
                    var dbTag = (from t in _context.Tags where t.Value == tag select t).FirstOrDefault();
                    SetTagInArticle(tagirationArticle.Article, dbTag ?? new Tag(tag));
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