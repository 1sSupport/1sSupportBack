// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeightedArticle.cs" company="">
//   
// </copyright>
// <summary>
//   The weighted article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Finder
{
    using WebApi.EF.Models;

    /// <summary>
    ///     The weighted article.
    /// </summary>
    internal class WeightedArticle
    {
        /// <summary>
        ///     Gets or sets the article.
        /// </summary>
        public Article Article { get; set; }

        /// <summary>
        ///     Gets or sets the t.
        /// </summary>
        public double T { get; set; }
    }
}