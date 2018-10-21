using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using WebApi.EF.Models;
using WebApi.Tagirator.Parser;

namespace WebApi.Infrastructer
{
    public class QueryHandler
    {
        private readonly EFContext Context;
        public QueryHandler(EFContext context)
        {
            Context = context;
        }

        public ICollection<EF.Models.Article> GetArticlesByQuery(string query)
        {
            var words = FilteredText.GetWords(query); // Список слов запроса
            var tags = words.Select(word => (from t in Context.Tags where t.Value.ToLower() == word.ToLower() select t).FirstOrDefault()).Where(tag => tag != null).ToList(); //Найденные теги для введенных слов
            var articleswithcoef = new List<WeightedArticle>(); // результирующая коллекция
            if (!tags.Any()) return null;
            {
                foreach (var tag in tags)
                {
                    var articles = (from a in tag.ArticleTag select a.Article).ToList();

                    
                    foreach (var article in articles)
                    {
                        
                        articleswithcoef.Add(GetWeightedArticle(article,tags));
                    }

                    
                }
            }


            return (from a in articleswithcoef.OrderByDescending(p => p.T) select a.Article).ToList();
        }

        private WeightedArticle GetWeightedArticle(Article article, IEnumerable<Tag> queryTags)
        {
            
            
                var tagsinarticle = article.ArticleTag.Where(innertag => queryTags.Contains(innertag.Tag)).ToList();

                double value = 0;
                foreach (var innertag in tagsinarticle)
                {
                    var temptag = (from t in article.ArticleTag where t.Tag == innertag.Tag select t).FirstOrDefault();
                    var tagIndex = article.ArticleTag.IndexOf(temptag);
                    value += (article.ArticleTag.Count - tagIndex) *innertag.Weight;
                }

                value *= tagsinarticle.Count;
                return (new WeightedArticle() { Article = article, T = value });
            
        }
    }

     class WeightedArticle 
    {
        public Article Article { get; set; }
        public double T { get; set; }

    }
}
