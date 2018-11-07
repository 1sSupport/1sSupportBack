// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleFinder.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ArticleFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Finder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Internal;

    using MoreLinq;

    using WebApi.EF.Models;
    using WebApi.Tools.Parser;

    /// <summary>
    ///     The article finder.
    /// </summary>
    public class ArticleFinder
    {
        /// <summary>
        ///     The _context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArticleFinder" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        public ArticleFinder(EFContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///     The get articles by query.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="ICollection{T}" />.
        /// </returns>
        public ICollection<Article> GetArticlesByQuery(string query)
        {
            var words = FilteredText.GetWords(query); // Список слов запроса
            var tags = words.Select(
                    word => (from t in this.context.Tags
                             where string.Equals(t.Value, word, StringComparison.OrdinalIgnoreCase)
                             select t).FirstOrDefault()).Where(tag => tag != null)
                .ToList(); // Найденные теги для введенных слов
            var articleswithcoef = new List<WeightedArticle>(); // результирующая коллекция

            if (!tags.Any()) return null;

            foreach (var tag in tags)
            {
                var articles = (from a in this.context.ArticleTags where a.Tag.Value == tag.Value select a.Article)
                    .ToList();

                Parallel.ForEach(articles, article => { articleswithcoef.Add(GetWeightedArticle(article, tags)); });
            }

            var currentArticle = (from a in articleswithcoef.OrderByDescending(p => p.T).DistinctBy(a => a.Article.Id)
                                  select a.Article).Take(50).ToList();

            return currentArticle;
        }

        /// <summary>
        ///     The get weighted article.
        /// </summary>
        /// <param name="article">
        ///     The article.
        /// </param>
        /// <param name="queryTags">
        ///     The query tags.
        /// </param>
        /// <returns>
        ///     The <see cref="WeightedArticle" />.
        /// </returns>
        private static WeightedArticle GetWeightedArticle(Article article, IEnumerable<Tag> queryTags)
        {
            var tagsinarticle = article.ArticleTag.Where(innertag => queryTags.Contains(innertag.Tag)).ToList();

            double value = 0;
            foreach (var innertag in tagsinarticle)
            {
                var temptag = (from t in article.ArticleTag where t.Tag == innertag.Tag select t).FirstOrDefault();
                var tagIndex = article.ArticleTag.IndexOf(temptag);
                value += (article.ArticleTag.Count - tagIndex) * innertag.Weight;
            }

            value *= tagsinarticle.Count;
            return new WeightedArticle { Article = article, T = value };
        }
    }
}