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
        ///     The objects.
        /// </summary>
        protected ICollection<T> objects = new List<T>();

        /// <summary>
        ///     The error list.
        /// </summary>
        private readonly List<string> errorList = new List<string>();

        /// <summary>
        ///     The file error writer sync.
        /// </summary>
        private readonly object fileErrorWriterSync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Deserializer{T}"/> class.
        /// </summary>
        /// <param name="pathToFolder">
        /// The path to folder with path.
        /// </param>
        protected Deserializer(string pathToFolder)
        {
            this.Directory = new DirectoryInfo(pathToFolder);
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
        ///     Gets the file error.
        /// </summary>
        protected virtual FileInfo FileError { get; private set; }

        /// <summary>
        ///     The deserialize async.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">
        /// </exception>
        public void Deserialize()
        {
            if (!this.Directory.Exists) throw new DirectoryNotFoundException();
            this.FileError = new FileInfo(Path.Combine(this.Directory.FullName, "DeserializerError.txt"));

            var files = this.Directory.GetFiles(string.Empty).ToList();

            if (!files.Any()) throw new NullReferenceException();

            var tasks = new Task[this.ThreadCount];

            var elementsPerTask = files.Count / this.ThreadCount + 1;

            for (var k = 0; k < this.ThreadCount; k++)
            {
                var start = k * elementsPerTask;
                var indexlastElementPerTask = (k + 1) * elementsPerTask;

                var finish = indexlastElementPerTask < files.Count ? indexlastElementPerTask : files.Count;

                tasks[k] = this.DeserializeFromFilesAsync(start, finish, files);
            }

            Task.WaitAll(tasks);

            this.SaveObjects();
            this.WriteErrors();
        }

        /// <summary>
        /// The get chapter from file.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        protected virtual void GetObjectFromFile(FileInfo file)
        {
            var serializer = new JsonSerializer();
            using (var stream = file.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        try
                        {
                            var item = serializer.Deserialize<T>(jsonTextReader);

                            lock (this.objects)
                            {
                                this.objects.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (this.fileErrorWriterSync)
                            {
                                this.errorList.Add(
                                    $@"Что-то сломалось при сериализации {file.Name} -> {ex.Message}{Environment.NewLine}");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     The save objects.
        /// </summary>
        protected abstract void SaveObjects();

        /// <summary>
        ///     The write errors.
        /// </summary>
        protected virtual void WriteErrors()
        {
            lock (this.fileErrorWriterSync)
            {
                if (this.errorList.Any())
                    File.WriteAllLines(this.FileError.FullName, this.errorList);
            }
        }

        /// <summary>
        /// The deserialize from files.
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
        private Task DeserializeFromFilesAsync(int start, int last, IReadOnlyList<FileInfo> files)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        for (var i = start; i < last; i++) this.GetObjectFromFile(files[i]);
                    },
                TaskCreationOptions.LongRunning);
        }
    }
}