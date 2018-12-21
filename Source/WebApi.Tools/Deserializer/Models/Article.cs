// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Article.cs" company="">
//   
// </copyright>
// <summary>
//   The article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Deserializer.Models
{
    /// <summary>
    ///     The article.
    /// </summary>
    public class Article
    {
        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     Gets or sets the repeated.
        /// </summary>
        public Repeated Repeated { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the versions.
        /// </summary>
        public Version[] Versions { get; set; }
    }
}