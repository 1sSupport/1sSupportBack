// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagirationArticleTest.cs" company="">
//   
// </copyright>
// <summary>
//   The tagiration article test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using WebApi.EF.Models;

namespace WebApi.Test
{
    using System;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;
    
    using WebApi.Tools.Tagirator;

    using Xunit;

    using Article = WebApi.EF.Models.Article;

    /// <summary>
    ///     The tagiration article test.
    /// </summary>
    public class TagirationArticleTest : IDisposable
    {
        /// <summary>
        /// The article.
        /// </summary>
        private Article article;

        /// <summary>
        /// The subdir.
        /// </summary>
        private DirectoryInfo Subdir;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagirationArticleTest" /> class.
        /// </summary>
        public TagirationArticleTest()
        {
            this.Initial();
        }

        /// <summary>
        ///     The can_ get_ repeat_ freq.
        /// </summary>
        [Fact]
        public void CanGetRepeatFreq()
        {
            var test = new TagirationArticle(this.article);

            var testValue1 = test.GetWordFrequancy("жоп");
            var testValue2 = test.GetWordFrequancy("я");

            Assert.Equal(2, testValue1);
            Assert.Equal(0, testValue2);
        }

        /// <summary>
        ///     The can get tags.
        /// </summary>
        [Fact]
        public void CanGetTags()
        {
            var test = new TagirationArticle(this.article).GetTagsAndWeight();

            Assert.NotNull(test);
        }

        /// <summary>
        /// The can initial.
        /// </summary>
        [Fact]
        public void CanInitial()
        {
            var tagirationArticle = new TagirationArticle(this.article);

            Assert.NotNull(tagirationArticle);

            Assert.NotNull(tagirationArticle.Article);
        }

        /// <summary>
        ///     The can set current rate.
        /// </summary>
        [Fact]
        public void CanSetCurrentRate()
        {
            var tagirationArticle = new TagirationArticle(this.article);
            tagirationArticle.SetWordRate("жоп", 1);

            var tag = tagirationArticle.GetTagsAndWeight();
            var key = tag.FirstOrDefault();
            Assert.Equal("жоп", key.Key);
        }

        /// <inheritdoc />
        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Subdir.Delete(true);
        }

        /// <summary>
        ///     The not empty words.
        /// </summary>
        [Fact]
        public void NotEmptyWords()
        {
            var tagirationArticle = new TagirationArticle(this.article);

            Assert.NotEmpty(tagirationArticle.CleanWords);
        }

        /// <summary>
        /// The initial.
        /// </summary>
        private void Initial()
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            this.Subdir = dir.CreateSubdirectory("TestArticle");
            var path = Path.Combine(this.Subdir.FullName, "test.json");

            var newArticle = new SaveArticle()
                                 {
                                     Id = 1,
                                     Link = "Test@test",
                                     Response = "тест тест тест жопа жопа я я я тест",
                                     Title = "Test"
                                 };

            var file = new FileInfo(path);
            using (var writer = new StringWriter())
            {
                writer.Write(
                    JsonConvert.SerializeObject(
                        newArticle,
                        Formatting.Indented,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                if (!file.Exists) file.Create().Close();

                using (var stream = new StreamWriter(file.FullName))
                {
                    stream.WriteLine(writer.ToString());
                }
            }

            this.article = new Article("Test", path);
        }
    }
}