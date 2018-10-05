using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class WebApiDbContext: DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
