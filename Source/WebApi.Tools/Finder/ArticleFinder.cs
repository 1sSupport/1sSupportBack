using Microsoft.EntityFrameworkCore.Internal;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.EF.Models;
using WebApi.Tools.Parser;

namespace WebApi.Tools.Finder
{
    public class ArticleFinder
    {
        private readonly EFContext _context;

        public ArticleFinder(EFContext context)
        {
            _context = context;
        }

        public ICollection<Article> GetArticlesByQuery(string query)
        {
            var words = FilteredText.GetWords(query); // Список слов запроса
            var tags = words.Select(word => (from t in _context.Tags where string.Equals(t.Value, word, StringComparison.OrdinalIgnoreCase) select t).FirstOrDefault()).Where(tag => tag != null).ToList(); //Найденные теги для введенных слов
            var articleswithcoef = new List<WeightedArticle>(); // результирующая коллекция

            if (!tags.Any())
            {
                return null;
            }

            {
                foreach (var tag in tags)
                {
                    var articles = (from a in _context.ArticleTags where (a.Tag.Value == tag.Value) select a.Article);

                    Parallel.ForEach(articles, article => { articleswithcoef.Add(GetWeightedArticle(article, tags)); });

                    foreach (var article in articles)
                    {
                        articleswithcoef.Add(GetWeightedArticle(article, tags));
                    }
                }
            }

            var currentArticle = (from a in articleswithcoef.OrderByDescending(p => p.T).DistinctBy(a => a.Article.Id) select a.Article);

            return currentArticle.ToList();
        }

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
            return (new WeightedArticle() { Article = article, T = value });
        }
    }

    internal class WeightedArticle
    {
        public Article Article { get; set; }
        public double T { get; set; }
    }
}