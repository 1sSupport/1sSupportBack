namespace WebApi.Test
{
    using Microsoft.EntityFrameworkCore;

    using WebApi.Controllers;
    using WebApi.EF.Models;

    using Xunit;

    public class ArticleControllerTest
    {
        private readonly EFContext context;

        private readonly string token;

        public ArticleControllerTest()
        {
            context = new EFContext(new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options);
        }

        [Fact]
        public void CanInitialize()
        {
            var target = new ArticleController(context);
        }
    }
}