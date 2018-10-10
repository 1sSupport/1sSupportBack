using System;
using WebApi.Tagirator.Parser;
using Xunit;

namespace WebApi.Test
{
    public class ArticleTest : IDisposable
    {
        [Fact]
        public void Can_Initial()
        {
            Article Article = new Article(title, text);

            Assert.Equal(Article.Title, title);
            Assert.Equal(Article.Text, text);
        }

        [Fact]
        public void Can_Parse()
        {
            Article Article = new Article(title, text);
            bool test = Article.ParseText();
            Assert.True(test);
            Assert.NotEmpty(Article.Words);
            Assert.NotEmpty(Article.WordsRate);
        }

        [Fact]
        public void Not_Empty_Text()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => { Article Article = new Article(title, NullText); });

            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Can_Set_Current_Rate()
        {
            Article Article = new Article(title, text);

            bool test = Article.SetCurrentRate(k, 1);
            Assert.True(test);
        }

        [Fact]
        public void Can_Set_Tags()
        {
            Article Article = new Article(title, text);
            var test = Article.SetTags();
            Assert.True(test);
        }

        [Fact]
        public void Can_Get_Repeat_Freq()
        {
            Article Article = new Article(title, text);
            var test = Article.GetRepeatFrequency(k);
            Assert.NotEqual(1, test);
            Assert.Equal(2, test);
        }

        public void Dispose()
        {
        }

        private readonly string text;
        private readonly string title;
        private readonly string NullText;
        private readonly string k;

        public ArticleTest()
        {
            title = "dad";
            text = "Жопа я я";
            NullText = "";
            k = "я";
        }
    }
}