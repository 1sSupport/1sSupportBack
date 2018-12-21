// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter.cs" company="">
//   
// </copyright>
// <summary>
//   The chapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Deserializer.Models
{
    /// <summary>
    ///     The chapter.
    /// </summary>
    public class Chapter
    {
        /// <summary>
        ///     Gets or sets a value indicating whether contents.
        /// </summary>
        public bool Contents { get; set; }

        /// <summary>
        ///     Gets or sets the folder.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     Gets or sets the page without contents.
        /// </summary>
        public string PageWithoutContents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether repeated.
        /// </summary>
        public bool Repeated { get; set; }

        /// <summary>
        ///     Gets or sets the repeating links.
        /// </summary>
        public ChapterRepeatinglinks RepeatingLinks { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}