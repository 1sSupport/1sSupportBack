#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApi.EF.Models;
using WebApi.Tools.Deserializer;
using WebApi.Tools.Normalizator;
using WebApi.Tools.Parser;
using WebApi.Tools.Tagirator;

#endregion

namespace WebApi.Infrastructer
{
    #region

    #endregion

    /// <summary>
    ///     The seed data.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        ///     The ensure populated.
        /// </summary>
        /// <param name="app">
        ///     The app.
        /// </param>
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var start = DateTime.Now;
            var path = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "articles"));
            DateTime end;
            

            if (!path.Exists)
            {
                File.AppendAllText(@"d:\test.txt", $"{start} Start RUN {Environment.NewLine}");
                Console.WriteLine($"{start} Start RUN {Environment.NewLine}");

                path.Create();

                var normalizator = new Normalizator(@"D:\Загрузки\dumps", path.FullName);

                await normalizator.NormalizeAsync();

                //Parallel.ForEach(path.GetFiles("*.json"), f =>
                //{
                //    var line = string.Empty;

                //    using (var stream = f.OpenRead())
                //    {
                //        using (var reader = new StreamReader(stream))
                //        {
                //            line = reader.ReadToEnd();
                //            line = line.Replace("\\\"", "\"");
                //        }
                //    }

                //    f.Delete();
                //    using (var stream = new StreamWriter(f.Create()))
                //    {
                //        stream.WriteLine(line);
                //    }
                //});

                end = DateTime.Now;
                File.AppendAllText(@"d:\test.txt", $"{end} -> ArticleNormalize {end - start}{Environment.NewLine}");
                Console.WriteLine($"{end} ArticleNormalize {end - start}");
            }

            using (var context = app.ApplicationServices.GetRequiredService<EFContext>())
            {
                if (!context.Users.Any())
                {
                    var provider = new Provider("testProvieder", DateTime.Now.AddYears(10), "govjadkoilja@yandex.ru");
                    context.Users.Add(new User("test", "test", "000000000000", provider));
                    context.Users.Add(new User("admin", "admin", "999999999999", provider));
                    context.SaveChanges();
                    end = DateTime.Now;
                    File.AppendAllText(@"d:\test.txt", $"{end} -> user {end - start}{Environment.NewLine}");
                    Console.WriteLine($"{end}user {end - start}");
                }

                if (!context.AskTitle.Any())
                {
                    context.AskTitle.AddRange(
                        new List<AskTitle>
                        {
                            new AskTitle("Тема 1"),
                            new AskTitle("Тема 2"),
                            new AskTitle("Тема 3"),
                            new AskTitle("Тема 4")
                        });
                    context.SaveChanges();
                }

                if (!context.Articles.Any())
                {
                    var serializator = new ArticleDeserializer(path.FullName, context);

                    serializator.Deserialize();

                    context.SaveChanges();

                    end = DateTime.Now;
                    Console.WriteLine($"{end}Article {end - start}");
                    File.AppendAllText(@"d:\test.txt", $"{end} -> DumpArticle {end - start}{Environment.NewLine}");
                }

                if (!context.ArticleTags.Any() || !context.Tags.Any())
                    using (var tagirator = new Tagirator(context))
                    {
                        tagirator.SetTagsInArticle();
                        await context.SaveChangesAsync();

                        end = DateTime.Now;
                        Console.WriteLine($"{end}Tagiration {end - start}");
                        File.AppendAllText(@"d:\test.txt", $"{end} -> Tagiration {end - start}{Environment.NewLine}");
                    }

                GC.Collect(GC.MaxGeneration);
                GC.WaitForPendingFinalizers();
            }
        }
    }
}

