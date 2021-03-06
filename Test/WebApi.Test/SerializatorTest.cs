﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializatorTest.cs" company="">
//   
// </copyright>
// <summary>
//   The serializator test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using WebApi.EF.Models;
    using WebApi.Tools.Deserializer;

    using Xunit;

    /// <summary>
    ///     The serializator test.
    /// </summary>
    public class SerializatorTest
    {
        /// <summary>
        ///     The can deserialize.
        /// </summary>
        [Fact]
        public async void CanDeserialize()
        {
            var path = @"D:\Загрузки\testDump";

            using (var context = new EFContext(
                new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Serialion_test2").Options))
            {
                var serializator = new ArticleDeserializer(path, context);

                var count = context.Articles.Count();

                serializator.Deserialize();

                context.SaveChanges();

                Assert.NotNull(serializator);
                Assert.NotEmpty(context.Articles);
                context.Dispose();
            }
        }
    }
}