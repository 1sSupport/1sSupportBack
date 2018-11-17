namespace WebApi.Tools.Deserializer
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using WebApi.EF.Models;

    #endregion

    /// <summary>
    ///     The article deserializator.
    /// </summary>
    public class ArticleDeserializer
    {
        /// <summary>
        ///     The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArticleDeserializer" /> class.
        /// </summary>
        /// <param name="pathToFolderWithPath">
        ///     The path to folder with path.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public ArticleDeserializer(string pathToFolderWithPath, EFContext context)
        {
            this.Directory = new DirectoryInfo(pathToFolderWithPath);
            this.context = context;
        }

        /// <summary>
        ///     Gets the thread count. Default 16 threads use. Min value is 1.
        /// </summary>
        public int ThreadCount => Environment.ProcessorCount;

        /// <summary>
        ///     Gets the directory.
        /// </summary>
        private DirectoryInfo Directory { get; }

        /// <summary>
        ///     The deserialize.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        /// <exception cref="DirectoryNotFoundException">
        /// </exception>
        public async Task DeserializeAsync()
        {
            if (!this.Directory.Exists) throw new DirectoryNotFoundException();

            var files = this.Directory.GetFiles().ToList();

            var mod = files.Count / this.ThreadCount;

            ICollection<Chapter> chapters;

            for (var i = 0; i < mod; i++)
            {
                var curentfiles = files.Take(this.ThreadCount).ToList();

                files.RemoveRange(0, this.ThreadCount);
                chapters = await DeserializeChapterByTasksAsync(curentfiles).ConfigureAwait(false);
                this.AddChaptersItemInBd(ref chapters);
            }

            chapters = await DeserializeChapterByTasksAsync(files).ConfigureAwait(false);
            this.AddChaptersItemInBd(ref chapters);

            chapters.Clear();
        }

        /// <summary>
        ///     The deserialize chapter by tasks.
        /// </summary>
        /// <param name="files">
        ///     The files.
        /// </param>
        /// <returns>
        ///     The <see cref="ICollection{Chapter}" />.
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
                    () => { GetChapterFromFile(ref fileref, ref chapters); },
                    TaskCreationOptions.LongRunning);
                ++index;
            }

            Task.WaitAll(tasks);

            return chapters;
        }

        /// <summary>
        ///     The deserialize chapter by tasks.
        /// </summary>
        /// <param name="files">
        ///     The files.
        /// </param>
        /// <returns>
        ///     The <see cref="ICollection{Chapter}" />.
        /// </returns>
        private static Task<ICollection<Chapter>> DeserializeChapterByTasksAsync(ICollection<FileInfo> files)
        {
            return Task.Run(() => DeserializeChapterByTasks(files));
        }
        
        /// <summary>
        /// The get chapter from file.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="chapters">
        /// The chapters.
        /// </param>
        private static void GetChapterFromFile(ref FileInfo file, ref ICollection<Chapter> chapters)
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
        ///     The save chapters item in bd async.
        /// </summary>
        /// <param name="chapters">
        ///     The chapters.
        /// </param>
        private void AddChaptersItemInBd(ref ICollection<Chapter> chapters)
        {
            foreach (var chapter in chapters)
            {
                foreach (var chapterContent in chapter.contents)
                    this.context.Articles.Add(new Article(chapterContent.title, chapterContent.response));
                this.context.SaveChanges();
            }
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