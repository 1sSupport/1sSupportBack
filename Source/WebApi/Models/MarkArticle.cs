// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkArticle.cs" company="">
//
// </copyright>
// <summary>
//   The mark article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Models
{
    /// <summary>
    /// The mark article.
    /// </summary>
    public class MarkArticle
    {
        /// <summary>
        /// Gets or sets the article id.
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// Gets or sets the mark.
        /// </summary>
        public int Mark { get; set; }

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        public int SessionId { get; set; }
    }
}