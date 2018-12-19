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
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    ///     The mark article.
    /// </summary>
    public class MarkArticle
    {
        /// <summary>
        ///     Gets or sets the article id.
        /// </summary>
        [Required]
        public int ArticleId { get; set; }

        /// <summary>
        ///     Gets or sets the mark.
        /// </summary>
        [Required]
        public int Mark { get; set; }

        /// <summary>
        ///     Gets or sets the session id.
        /// </summary>
        [Required]
        public int SessionId { get; set; }
    }
}