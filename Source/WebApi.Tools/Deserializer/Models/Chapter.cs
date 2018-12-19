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
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    ///     The chapter.
    /// </summary>
    public class Chapter
    {
        /// <summary>
        ///     Gets or sets the contents.
        /// </summary>
        public List<Content> Contents { get; set; }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        public string Link { get; set; }


        public int Status { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }


    public class ArticleContent
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public int Status { get; set; }

        public List<Content> Versions { get; set; }
    }

}