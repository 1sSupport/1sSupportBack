// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializatorTest.cs" company="">
//
// </copyright>
// <summary>
//   The serializator test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using WebApi.EF.Models;
    using WebApi.Tools.Deserializer;
    using Xunit;

    /// <summary>
    ///     The serializator test.
    /// </summary>
    public class SerializatorTest
    {
        /// <summary>
        ///     The cand deserialize with custom tread value.
        /// </summary>
        [Fact]
        public async void CandDeserializeWithCustomTreadValue()
        {
            var path = @"D:\Загрузки\TestDumps";

            using (var context = new EFContext(
                new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options))
            {
                var serializator = new ArticleDeserializer(path, context);

                var count = context.Articles.Count();

                await serializator.DeserializeAsync().ConfigureAwait(false);

                Assert.NotNull(serializator);
                Assert.NotEmpty(context.Articles);
            }
        }

        /// <summary>
        ///     The can deserialize.
        /// </summary>
        [Fact]
        public async void CanDeserialize()
        {
            var path = @"D:\Загрузки\TestDumps";

            using (var context = new EFContext(
                new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options))
            {
                var serializator = new ArticleDeserializer(path, context);

                var count = context.Articles.Count();

                await serializator.DeserializeAsync().ConfigureAwait(false);

                Assert.NotNull(serializator);
                Assert.NotEmpty(context.Articles);
            }
        }
    }
}