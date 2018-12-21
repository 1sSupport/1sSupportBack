// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeedData.cs" company="">
//   
// </copyright>
// <summary>
//   The seed data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Infrastructer
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using WebApi.EF.Models;
    using WebApi.Tools.Deserializer;
    using WebApi.Tools.Tagirator;

    #endregion

    /// <summary>
    ///     The seed data.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// The ensure populated.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<EFContext>())
            {
                var start = DateTime.Now;
                if (!context.Users.Any())
                {
                    var provider = new Provider("testProvieder", DateTime.Now.AddYears(10), "govjadkoilja@yandex.ru");
                    context.Users.Add(new User("test", "test", "000000000000", provider));
                    context.Users.Add(new User("admin", "admin", "999999999999", provider));
                    context.SaveChanges();
                    var end = DateTime.Now;
                    File.AppendAllText(@"d:\test.txt", $"{end} -> user {end - start}{Environment.NewLine}");
                    Console.WriteLine($"{end}user {end - start}");
                }

                if (!context.AskTitle.Any())
                {
                    context.AskTitle.AddRange(
                        new List<AskTitle>
                            {
                                new AskTitle("tema1"),
                                new AskTitle("tema2"),
                                new AskTitle("tema3"),
                                new AskTitle("tema4")
                            });
                    await context.SaveChangesAsync();
                }

                if (!context.Articles.Any())
                {
                    var path = Path.Combine(Environment.CurrentDirectory, "articles");
                    if (!Directory.Exists(path)) return;

                    var serializator = new ArticleDeserializer(path, context);

                    serializator.Deserialize();

                    await context.SaveChangesAsync();

                    var end = DateTime.Now;
                    Console.WriteLine($"{end}Article {end - start}");
                    File.AppendAllText(@"d:\test.txt", $"{end} -> Article {end - start}{Environment.NewLine}");
                }

                if (!context.Tags.Any() || !context.ArticleTags.Any())
                    using (var tagirator = new Tagirator(context))
                    {
                        tagirator.SetTagsInArticle();
                        await context.SaveChangesAsync();

                        var end = DateTime.Now;
                        Console.WriteLine($"{end}Tagiration {end - start}");
                        File.AppendAllText(@"d:\test.txt", $"{end} -> Tagiration {end - start}{Environment.NewLine}");
                    }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}