// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleDeserializer.cs" company="">
//   
// </copyright>
// <summary>
//   The article deserializator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Deserializer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using WebApi.EF.Models;

    /// <summary>
    /// The article deserializator.
    /// </summary>
    public class ArticleDeserializer
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleDeserializer"/> class.
        /// </summary>
        /// <param name="pathToFolderWithPath">
        /// The path to folder with path.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public ArticleDeserializer(string pathToFolderWithPath, EFContext context)
        {
            Directory = new DirectoryInfo(pathToFolderWithPath);
            this.context = context;
        }

        /// <summary>
        /// Gets or sets the thread count.
        /// </summary>
        public int ThreadCount { get; set; } = 16;


        /// <summary>
        /// Gets the directory.
        /// </summary>
        private DirectoryInfo Directory { get; }

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="DirectoryNotFoundException">
        /// </exception>
        public async Task DeserializeAsync()
        {
            if (!Directory.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            var files = Directory.GetFiles().ToList();

            var mod = files.Count / ThreadCount;

            for (int i = 0; i < mod; i++)
            {
                var curentfiles = files.Take(ThreadCount).ToList();

                files.RemoveRange(0, ThreadCount);

                await SaveChaptersItemInBdAsync(await DeserializeChapterByTasksAsync(curentfiles).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }

            await SaveChaptersItemInBdAsync(await DeserializeChapterByTasksAsync(files).ConfigureAwait(false))
                .ConfigureAwait(false);
        }


        /// <summary>
        /// The get chapter and add to context.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="chapters">
        /// The chapters.
        /// </param>
        private static void GetChapterAndAddToContext(ref FileInfo file, ref ICollection<Chapter> chapters)
        {
            var serializer = new JsonSerializer();
            using (var stream = file.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        var chapter = serializer.Deserialize<Chapter>(jsonTextReader);
                        chapter.FileName = file.Name;
                        chapters.Add(chapter);
                    }
                }
            }
        }

        /// <summary>
        /// The deserialize chapter by tasks.
        /// </summary>
        /// <param name="files">
        /// The files.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{Chapter}"/>.
        /// </returns>
        private static ICollection<Chapter> DeserializeChapterByTasks(ICollection<FileInfo> files)
        {
            var tasks = new Task[files.Count];

            var index = 0;

            ICollection<Chapter> chapters = new List<Chapter>();

            foreach (var file in files)
            {
                var fileref = file;
                tasks[index] = Task.Factory.StartNew(
                    () => { GetChapterAndAddToContext(ref fileref, ref chapters); },
                    TaskCreationOptions.LongRunning);
                ++index;
            }


            Task.WaitAll(tasks);

            return chapters;
        }

        /// <summary>
        /// The deserialize chapter by tasks.
        /// </summary>
        /// <param name="files">
        /// The files.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{Chapter}"/>.
        /// </returns>
        private static Task<ICollection<Chapter>> DeserializeChapterByTasksAsync(ICollection<FileInfo> files)
        {
            return Task.Run(() => DeserializeChapterByTasks(files));
        }

        /// <summary>
        /// The save chapters item in bd async.
        /// </summary>
        /// <param name="chapters">
        /// The chapters.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private Task SaveChaptersItemInBdAsync(IEnumerable<Chapter> chapters)
        {
            foreach (var chapter in chapters)
            {
                foreach (var chapterContent in chapter.contents)
                {
                    context.Articles.Add(new Article(chapterContent.title, chapterContent.response));
                }
            }

            return context.SaveChangesAsync();
        }
    }

    public class Content
    {
        public string link { get; set; }

        public string response { get; set; }

        public string title { get; set; }
    }

    public class Chapter
    {
        public List<Content> contents { get; set; }

        [JsonIgnore]
        public string FileName { get; set; }

        public string link { get; set; }

        public string title { get; set; }
    }
}