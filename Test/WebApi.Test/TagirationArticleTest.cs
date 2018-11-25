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
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using WebApi.EF.Models;
    using WebApi.Tools.Deserializer.Models;
    using WebApi.Tools.Tagirator;

    using Xunit;

    /// <summary>
    /// The tagiration article test.
    /// </summary>
    public class TagirationArticleTest : IDisposable
    {
        private Article article;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagirationArticleTest"/> class.
        /// </summary>
        public TagirationArticleTest()
        {
            Initial();
        }

        private DirectoryInfo Subdir;

        private void Initial()
        {

            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            Subdir = dir.CreateSubdirectory("TestArticle");
            var path = Path.Combine(this.Subdir.FullName, "test.json");

            var newArticle = new NewArticle()
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

        /// <summary>
        /// The can_ get_ repeat_ freq.
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
        /// The can get tags.
        /// </summary>
        [Fact]
        public void CanGetTags()
        {

            var test = new TagirationArticle(article).GetTagsAndWeight();

            Assert.NotNull(test);
        }

        [Fact]
        public void CanInitial()
        {

            TagirationArticle tagirationArticle = new TagirationArticle(article);

            Assert.NotNull(tagirationArticle);

            Assert.NotNull(tagirationArticle.Article);
        }

        /// <summary>
        /// The can set current rate.
        /// </summary>
        [Fact]
        public void CanSetCurrentRate()
        {

            var tagirationArticle = new TagirationArticle(article);
            tagirationArticle.SetWordRate("жоп", 1);

            var tag = tagirationArticle.GetTagsAndWeight();
            var key = tag.FirstOrDefault();
            Assert.Equal("жоп", key.Key);
        }

        
        /// <inheritdoc />
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Subdir.Delete(true);
        }

        /// <summary>
        /// The not empty words.
        /// </summary>
        [Fact]
        public void NotEmptyWords()
        {
            var tagirationArticle = new TagirationArticle(article);

            Assert.NotEmpty(tagirationArticle.CleanWords);
        }
    }
}