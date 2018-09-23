using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Handler;
using Xunit;

namespace WebApi.Test
{
   public class ArticlePoolHandlerTest : IDisposable
    {
        public void Dispose()
        {
        }

        private List<Article> Articles;


        public ArticlePoolHandlerTest()
        {
            Articles= new List<Article>()
            {
                new Article("da","да я я"),
                new Article("da","я я он")
            };

        }

        [Fact]
        public void Can_Initialize_Pool()
        {

            ArticlePoolHandler pool = new ArticlePoolHandler(Articles);


        }
        [Fact]
        public void Can_Set_Words_rate()
        {
            var pool = new ArticlePoolHandler(Articles);
            pool.SetWordsRate();
            //Assert.NotEmpty(pool.GlobalDictionary);
            Assert.NotEmpty(Articles.First().Tags);
        }
    }
}
