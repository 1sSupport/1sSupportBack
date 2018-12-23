// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DumpHelper
{
    #region

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using WebApi.EF.Models;
    using WebApi.Tools.Normalizator;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The random.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// The serializer.
        /// </summary>
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        /// <summary>
        /// The get random normal article.
        /// </summary>
        /// <param name="files">
        /// The files.
        /// </param>
        /// <returns>
        /// The <see cref="(SaveArticle, FileInfo)"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private static (SaveArticle, FileInfo) GetRandomNormalArticle(ref FileInfo[] files)
        {
            var rndNum = Random.Next(0, files.Length);
            var file = files[rndNum];
            SaveArticle article;
            using (var stream = file.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        try
                        {
                            article = Serializer.Deserialize<SaveArticle>(jsonTextReader);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"ОПА. Что-то сломалось. Если ты видишь эту ошибку значит что-то сломалось при замене сердечка на ковычки. Скорей всего нельзя сериализовать json. открой файл скопируй его на сайт https://jsonlint.com/ и проверь на коректность . {file.Name} {ex.Message} ", ex);
                        }
                    }
                }
            }

            return (article, file);
        }
        

        private static void Main(string[] args)
        {
            DirectoryInfo dumpDir;
            DirectoryInfo saveDir;

            if (args.Length >= 2)
            {
                dumpDir = new DirectoryInfo(args[0]);
                saveDir = new DirectoryInfo(args[1]);
            }
            else
            {
                Console.WriteLine("не указаны папки. 1 указывается откуда брать. 2 куда ложить.");
                Console.ReadLine();
                return;
            }

            if (!dumpDir.Exists)
            {
                Console.WriteLine("Такой папки для дампов не существует. Првоверь аргументы");
                Console.ReadLine();
                return;
            }

            if (!saveDir.Exists)
            {
                saveDir.Create();
            }
            else
            {
                foreach (var file in saveDir.GetFiles("*.json"))
                {
                    file.Delete();
                }
            }

            var normalizator = new Normalizator(dumpDir.FullName, saveDir.FullName);

            Console.WriteLine(
                "Cейчас начнется нормализация. Подожди пожалуйста. будет сообщение когда она закончится.");

            var task = normalizator.NormalizeAsync();

            while (!task.IsCompleted)
            {
                Console.WriteLine("Она все еще идет и идет.");
                Thread.Sleep(1 * 1000);
            }

            Console.WriteLine("Она закончилась.");

            Console.WriteLine("Сейчас начнется замена \"сердечка\" на ковычки для всех нормированных статей");

            var files = saveDir.GetFiles("*.json");
            Parallel.ForEach(
                files,
                f =>
                    {
                        var line = string.Empty;
                        Console.WriteLine(f.Name);
                        using (var stream = f.OpenRead())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                line = reader.ReadToEnd();
                                if (line.Contains("&#x2661;")) line = line.Replace("&#x2661;", "\"");
                            }
                        }

                        f.Delete();
                        using (var stream = new StreamWriter(f.Create()))
                        {
                            stream.WriteLine(line);
                        }
                    });

            Console.WriteLine(
                "Сейчас будут случайно выбраны 3 jsona из нормализированных файлов и выведенны на консоль.");
            for (var i = 0; i < 4; i++)
            {
                Thread.Sleep(1 * 1000);
                Console.WriteLine($"{i + 1}");
            }

            for (var i = 0; i < 3; i++)
                try
                {
                    var item = GetRandomNormalArticle(ref files);
                    PrintRandomArticle(item);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                    return;
                }

            while (true)
            {
                Console.WriteLine("Если нужно еще, введи кол-во статей котоыре нужно вывести");
                if (int.TryParse(Console.ReadLine(), out var num))
                    for (var i = 0; i < num; i++)
                        try
                        {
                            var item = GetRandomNormalArticle(ref files);
                            PrintRandomArticle(item);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadLine();
                            return;
                        }
                else Console.WriteLine("Хорошая попытка. Но нет. Введи число");
            }
        }

        /// <summary>
        /// The print random article.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        private static void PrintRandomArticle((SaveArticle, FileInfo) item)
        {
            Console.WriteLine($"Из файла {item.Item2.Name}");
            Console.WriteLine("======================================");
            Console.WriteLine($"Preview {Environment.NewLine}{item.Item1.Preview}{Environment.NewLine}");
            Console.WriteLine($"Title {Environment.NewLine}{item.Item1.Title}{Environment.NewLine}");
            Console.WriteLine($"Response {Environment.NewLine}{item.Item1.Response}{Environment.NewLine}");
            Console.WriteLine($"Откуда взяли Response {Environment.NewLine}{item.Item1.From}{Environment.NewLine}");
            Console.WriteLine("======================================");
            Console.WriteLine();
        }
    }
}