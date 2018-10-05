namespace WebApi.Models
{
    public class ArticleTag
    {
        public int ArticleTagId { get; set; }
        public Tag Tags { get; set; }
        public Article Articles { get; set; }
    }
}
