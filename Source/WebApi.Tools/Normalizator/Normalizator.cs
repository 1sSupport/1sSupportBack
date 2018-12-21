// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Normalizator.cs" company="">
//   
// </copyright>
// <summary>
//   The normalizator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Normalizator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using WebApi.Tools.Deserializer;
    using WebApi.Tools.Deserializer.Models;

    /// <summary>
    ///     The normalizator.
    /// </summary>
    public class Normalizator
    {
        /// <summary>
        ///     The sync.
        /// </summary>
        private readonly object sync = new object();

        /// <summary>
        ///     The article id.
        /// </summary>
        private uint articleId = 1;

        /// <summary>
        ///     The save dir.
        /// </summary>
        private DirectoryInfo saveDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="Normalizator"/> class.
        /// </summary>
        /// <param name="dumpsFolderPath">
        /// The dumps folder path.
        /// </param>
        /// <param name="saveFolderPath">
        /// The save folder path.
        /// </param>
        public Normalizator(string dumpsFolderPath, string saveFolderPath)
        {
            this.DumpsFolder = dumpsFolderPath;
            this.SaveFolder = saveFolderPath;
        }

        /// <summary>
        ///     Gets the dumps folder.
        /// </summary>
        public string DumpsFolder { get; }

        /// <summary>
        ///     Gets the save folder.
        /// </summary>
        public string SaveFolder { get; }

        /// <summary>
        ///     The normalize.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">
        /// </exception>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public Task NormalizeAsync()
        {
            var dumpDir = new DirectoryInfo(this.DumpsFolder);
            this.saveDir = new DirectoryInfo(this.SaveFolder);

            if (!dumpDir.Exists) throw new DirectoryNotFoundException(this.DumpsFolder);
            if (!this.saveDir.Exists) this.saveDir.Create();

            var dumpsFiles = dumpDir.GetFiles("*.json", SearchOption.TopDirectoryOnly);

            var tasks = new Task[Environment.ProcessorCount];

            var elementsPerTask = (dumpsFiles.Length / Environment.ProcessorCount) + 1;

            for (var k = 0; k < Environment.ProcessorCount; k++)
            {
                var start = k * elementsPerTask;
                var indexlastElementPerTask = (k + 1) * elementsPerTask;

                var finish = indexlastElementPerTask < dumpsFiles.Length ? indexlastElementPerTask : dumpsFiles.Length;

                tasks[k] = this.ConvertDumpsFilesAsync(start, finish, dumpsFiles);
            }

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// The get chapter from file.
        /// </summary>
        /// <param name="dumpFile">
        /// The dump file.
        /// </param>
        /// <returns>
        /// The <see cref="Chapter"/>.
        /// </returns>
        private static Chapter GetChapterFromFile(FileInfo dumpFile)
        {
            Chapter chapter;
            using (var stream = dumpFile.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        var serializer = new JsonSerializer();
                        chapter = serializer.Deserialize<Chapter>(jsonTextReader);
                    }
                }
            }

            return chapter;
        }

        /// <summary>
        /// The deserialize from files async.
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="last">
        /// The last.
        /// </param>
        /// <param name="files">
        /// The files.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private Task ConvertDumpsFilesAsync(int start, int last, IReadOnlyList<FileInfo> files)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        for (var i = start; i < last; i++)
                        {
                            var chapter = GetChapterFromFile(files[i]);

                            this.SaveArticleFromSaveDirectory(chapter);
                        }
                    },
                TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// The save article from save directory.
        /// </summary>
        /// <param name="chapter">
        /// The chapter.
        /// </param>
        private void SaveArticleFromSaveDirectory(Chapter chapter)
        {
            if (chapter.Contents == false || chapter.Repeated) return;

            var chapterArticleFolder = new DirectoryInfo(Path.Combine(this.DumpsFolder, chapter.Folder));

            if(!chapterArticleFolder.Exists) return;


            var serializer = new DumpArticleDeserializer(chapterArticleFolder.FullName);

            serializer.Deserialize();

            foreach (var dumpArticle in serializer.GetObjects())
            {
                foreach (var version in dumpArticle.Versions)
                {
                    uint id;
                    lock (this.sync)
                    {
                        id = this.articleId;
                        this.articleId = this.articleId + 1;
                    }

                    var newArticle = new NewArticle()
                                         {
                                             Id = id,
                                             Title = dumpArticle.Title,
                                             Link = version.Link,
                                             Response = version.Content
                                         };
                    var fileName = $"{newArticle.Id}.json";

                    var path = Path.Combine(this.saveDir.FullName, fileName);
                    var file = new FileInfo(path);
                    if (file.Exists) continue;

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
                }
            }
        }
    }
}