namespace WebApi.Tools.Tagirator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using WebApi.EF.Models;

    /// <summary>
    /// The tagirator.
    /// </summary>
    public class Tagirator
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// The _global words object.
        /// </summary>
        private readonly IDictionary<string, WordInfo> globalWordsObject = new Dictionary<string, WordInfo>();

        /// <summary>
        /// The tagiration articles.
        /// </summary>
        private readonly ICollection<TagirationArticle> tagirationArticles = new List<TagirationArticle>();

        private readonly IDictionary<string, Tag> localTagsDictionary = new Dictionary<string, Tag>();
        /// <summary>
        /// Initializes a new instance of the <see cref="Tagirator"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public Tagirator(EFContext context)
        {
            this.context = context ?? throw new NullReferenceException();
        }

        /// <summary>
        /// The set tags in article.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public void SetTagsInArticle()
        {
            if (context == null)
            {
                throw new NullReferenceException(
                    "Необходимо сначала добавить EFContext со статьями. Воспользуйтесь методом AddContextForTagging");
            }

            AddArticlesFromContext();

            SetGlobalWords();
            SetGlobalWordsRate();
            SetLocalRate();

            SetCurrentTag();
        }

        /// <summary>
        /// The is articles null or empty.
        /// </summary>
        /// <param name="articles">
        /// The articles.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsArticlesNullOrEmpty(IEnumerable<Article> articles)
        {
            return articles == null || !articles.Any();
        }

        /// <summary>
        /// The set tag in article.
        /// </summary>
        /// <param name="article">
        /// The article.
        /// </param>
        /// <param name="tag">
        /// The tag.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        private static void SetTagInArticle(Article article, Tag tag, double weight)
        {
            article.ArticleTag.Add(new ArticleTag(weight, article, tag));
            article.EditDate = DateTime.Now;
        }

        /// <summary>
        /// The add articles from context.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        private void AddArticlesFromContext()
        {
            if (IsArticlesNullOrEmpty(context.Articles))
            {
                throw new ArgumentException("Необходимо добавить стать в БД");
            }

            /*add range*/
            var currentArticle = context.Articles.Select(article => new TagirationArticle(article)).ToList();

            foreach (var tagirationArticle in currentArticle)
            {
                tagirationArticles.Add(tagirationArticle);
            }
        }

        /// <summary>
        /// The clear article tag table.
        /// </summary>
        private void ClearArticleTagTable()
        {
            var articletags = (from at in context.ArticleTags select at).ToList();
            context.ArticleTags.RemoveRange(articletags);
        }

        /// <summary>
        /// The get ratio freq.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        private double GetRatioFreq(string key)
        {
            if (!globalWordsObject.ContainsKey(key))
            {
                return 0;
            }

            // ReSharper disable once PossibleLossOfFraction
            double v = globalWordsObject[key].Freq / tagirationArticles.Count;
            return v;
        }

        /// <summary>
        /// The set current tag.
        /// </summary>
        private void SetCurrentTag()
        {
            ClearArticleTagTable();

            foreach (var tagirationArticle in tagirationArticles)
            {
                tagirationArticle.Article.ArticleTag.Clear();

                var tagsAndRate = tagirationArticle.GetTagsAndWeight().Take(10);

                foreach (var pair in tagsAndRate)
                {             
                    if (!localTagsDictionary.ContainsKey(pair.Key))
                    {
                        localTagsDictionary.Add(pair.Key,new Tag(pair.Key));            
                    }
                       SetTagInArticle(tagirationArticle.Article, localTagsDictionary[pair.Key], pair.Value);
                    
                    
                }
            }
        }

        /// <summary>
        /// The set global words.
        /// </summary>
        private void SetGlobalWords()
        {
            foreach (var article in tagirationArticles)
            {
                foreach (var word in article.CleanWords)
                {
                    if (globalWordsObject.ContainsKey(word))
                    {
                        var value = article.GetWordFrequancy(word);
                        globalWordsObject[word].Freq += value;
                    }
                    else
                    {
                        var wordObject = new WordInfo { Freq = article.GetWordFrequancy(word) };

                        globalWordsObject.Add(word, wordObject);
                    }
                }
            }
        }

        /// <summary>
        /// The set global words rate.
        /// </summary>
        private void SetGlobalWordsRate()
        {
            if (!globalWordsObject.Any())
            {
                SetGlobalWords();
            }

            foreach (var key in globalWordsObject.Keys)
            {
                var wordObject = globalWordsObject[key];
                wordObject.Rate = GetRatioFreq(key);
            }
        }

        /// <summary>
        /// The set local rate.
        /// </summary>
        private void SetLocalRate()
        {
            foreach (var article in tagirationArticles)
            {
                foreach (var word in article.CleanWords)
                {
                    article.SetWordRate(word, globalWordsObject[word].Rate);
                }
            }
        }
    }
}