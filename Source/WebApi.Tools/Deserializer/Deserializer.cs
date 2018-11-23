// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Deserializer.cs" company="">
//   
// </copyright>
// <summary>
//   The deserializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Deserializer
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The deserializer.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class Deserializer<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Deserializer{T}"/> class.
        /// </summary>
        /// <param name="pathToFolderWithPath">
        /// The path to folder with path.
        /// </param>
        protected Deserializer(string pathToFolderWithPath)
        {
            this.Directory = new DirectoryInfo(pathToFolderWithPath);
        }

        /// <summary>
        ///     Gets the thread count. Default 16 threads use. Min value is 1.
        /// </summary>
        public virtual int ThreadCount => Environment.ProcessorCount;

        /// <summary>
        ///     Gets the directory.
        /// </summary>
        protected virtual DirectoryInfo Directory { get; }

        /// <summary>
        ///     The deserialize async.
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

            ICollection<T> chapters;

            for (var i = 0; i < mod; i++)
            {
                var curentfiles = files.Take(this.ThreadCount).ToList();

                files.RemoveRange(0, this.ThreadCount);
                chapters = await this.DeserializeChapterByTasksAsync(curentfiles).ConfigureAwait(false);
                this.SaveObjects(ref chapters);
            }

            chapters = await this.DeserializeChapterByTasksAsync(files).ConfigureAwait(false);
            this.SaveObjects(ref chapters);

            chapters.Clear();
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
        protected virtual ICollection<T> DeserializeChapterByTasks(ICollection<FileInfo> files)
        {
            var tasks = new Task[files.Count];

            var index = 0;

            ICollection<T> chapters = new List<T>();

            foreach (var file in files)
            {
                var fileref = file;
                tasks[index] = Task.Factory.StartNew(
                    () => { this.GetChapterFromFile(ref fileref, ref chapters); },
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
        protected virtual Task<ICollection<T>> DeserializeChapterByTasksAsync(ICollection<FileInfo> files)
        {
            return Task.Run(() => this.DeserializeChapterByTasks(files));
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
        protected virtual void GetChapterFromFile(ref FileInfo file, ref ICollection<T> chapters)
        {
            var serializer = new JsonSerializer();
            using (var stream = file.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        var chapter = serializer.Deserialize<T>(jsonTextReader);
                        chapters.Add(chapter);
                    }
                }
            }
        }

        /// <summary>
        /// The save object.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        protected abstract void SaveObjects(ref ICollection<T> objects);
    }
}