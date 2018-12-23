// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveArticle.cs" company="">
//   
// </copyright>
// <summary>
//   The new article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.EF.Models
{
    /// <summary>
    ///     The new article.
    /// </summary>
    public class SaveArticle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SaveArticle" /> class.
        /// </summary>
        public SaveArticle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveArticle"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        public SaveArticle(string title, string text)
        {
            this.Title = title;
            this.Content = text;
        }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     Gets or sets the response.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}