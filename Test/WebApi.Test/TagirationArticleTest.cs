// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagirationArticleTest.cs" company="">
//
// </copyright>
// <summary>
//   The tagiration article test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using System;
    using System.Linq;

    using WebApi.EF.Models;
    using WebApi.Tools.Tagirator;

    using Xunit;

    /// <summary>
    /// The tagiration article test.
    /// </summary>
    public class TagirationArticleTest : IDisposable
    {
        /// <summary>
        /// The k.
        /// </summary>
        private readonly string k;

        private readonly string nullText;

        /// <summary>
        /// The text.
        /// </summary>
        private readonly string text;

        /// <summary>
        /// The title.
        /// </summary>
        private readonly string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagirationArticleTest"/> class.
        /// </summary>
        public TagirationArticleTest()
        {
            title = "dad";
            text = "Жопа я я";
            nullText = string.Empty;
            k = "я";
        }

        /// <summary>
        /// The can_ get_ repeat_ freq.
        /// </summary>
        [Fact]
        public void CanGetRepeatFreq()
        {
            Article Article = new Article(title, text);

            var test = new TagirationArticle(Article).GetWordFrequancy("я");

            Assert.Equal(2, test);
        }

        /// <summary>
        /// The can get tags.
        /// </summary>
        [Fact]
        public void CanGetTags()
        {
            Article article = new Article(title, text);

            var test = new TagirationArticle(article).GetTagsAndWeight();

            Assert.NotNull(test);
        }

        [Fact]
        public void CanInitial()
        {
            var article = new Article(title, text);

            TagirationArticle tagirationArticle = new TagirationArticle(article);

            Assert.NotNull(tagirationArticle);

            Assert.NotNull(tagirationArticle.Article);

            Assert.Equal(tagirationArticle.Article.Title, title);
            Assert.Equal(tagirationArticle.Article.Text, text);
        }

        /// <summary>
        /// The can set current rate.
        /// </summary>
        [Fact]
        public void CanSetCurrentRate()
        {
            Article article = new Article(title, text);

            var tagirationArticle = new TagirationArticle(article);
            tagirationArticle.SetWordRate("я", 1);

            var tag = tagirationArticle.GetTagsAndWeight();
            var key = tag.FirstOrDefault();
            Assert.Equal("я", key.Key);
        }

        /// <summary>
        /// The can set current rate with more text.
        /// </summary>
        [Fact]
        public void CanSetCurrentRateWithMoreText()
        {
            Article article = new Article(title, text + "я я жопа да ло ло ло ло ло");

            var tagirationArticle = new TagirationArticle(article);

            tagirationArticle.SetWordRate("ло", 1);

            var tag = tagirationArticle.GetTagsAndWeight();
            var key = tag.FirstOrDefault();

            Assert.Equal("ло", key.Key);
        }

        /// <inheritdoc />
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// The not empty words.
        /// </summary>
        [Fact]
        public void NotEmptyWords()
        {
            var article = new Article(title, text);

            var tagirationArticle = new TagirationArticle(article);

            Assert.NotEmpty(tagirationArticle.CleanWords);
        }
    }
}