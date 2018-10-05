using System;

namespace WebApi.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Sourse { get; set; }
        public DateTime EditData { get; set; }
        public double SignificanceCoefficient { get; set; }
        public bool IsDeleted { get; set; }
        public Article ParentArticle { get; set; }
        public Configuration ConfigurationId { get; set; }
    }
}
