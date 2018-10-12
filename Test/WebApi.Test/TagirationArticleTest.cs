using System;
using System.Linq;
using WebApi.EF.Models;
using WebApi.Tagirator;
using Xunit;

namespace WebApi.Test
{
    public class TagirationArticleTest : IDisposable
    {
        [Fact]
        public void Can_Initial()
        {
            var article = new Article(title, text);

            TagirationArticle Article = new TagirationArticle(article);

            Assert.NotNull(Article);

            Assert.NotNull(Article.Article);

            Assert.Equal(Article.Article.Title, title);
            Assert.Equal(Article.Article.Text, text);
        }

        [Fact]
        public void Not_Empty_Words()
        {
            var article = new Article(title, text);

            var tagirationArticle = new TagirationArticle(article);

            Assert.NotEmpty(tagirationArticle.CleanWords);
        }

        [Fact]
        public void Can_Set_Current_Rate()
        {
            Article article = new Article(title, text);

            var tagirationArticle = new TagirationArticle(article);
            tagirationArticle.SetWordRate("я", 1);

            var tag = tagirationArticle.GetTagsInArticle();
            var key = tag.Keys.FirstOrDefault();

            Assert.NotNull(key);
            Assert.Equal(1, tag[key].Rate);
        }

        [Fact]
        public void Can_Set_Current_Rate_With_More_Text()
        {
            Article article = new Article(title, text + "я я жопа да ло ло ло ло ло");

            var tagirationArticle = new TagirationArticle(article);

            tagirationArticle.SetWordRate("ло", 1);

            var tag = tagirationArticle.GetTagsInArticle();
            var key = tag.Keys.FirstOrDefault();

            Assert.NotNull(key);
            Assert.Equal(4, tag[key].Rate);
        }

        [Fact]
        public void Can_Get_Tags()
        {
            Article Article = new Article(title, text);

            var test = new TagirationArticle(Article).GetTagsInArticle();

            Assert.NotNull(test);
        }

        [Fact]
        public void Can_Get_Repeat_Freq()
        {
            Article Article = new Article(title, text);

            var test = new TagirationArticle(Article).GetWordFrequancy("я");

            Assert.Equal(2, test);
        }

        public void Dispose()
        {
        }

        private readonly string text;
        private readonly string title;
        private readonly string NullText;
        private readonly string k;

        public TagirationArticleTest()
        {
            title = "dad";
            text = "Жопа я я";
            NullText = "";
            k = "я";
        }
    }
}