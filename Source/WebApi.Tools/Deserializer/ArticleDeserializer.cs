// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleDeserializer.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ArticleDeserializer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Deserializer
{
    #region

    using System.Collections.Generic;

    using WebApi.EF.Models;
    using WebApi.Tools.Deserializer.Models;

    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     The article deserializator.
    /// </summary>
    public class ArticleDeserializer : Deserializer<Chapter>
    {
        /// <summary>
        ///     The context.
        /// </summary>
        private readonly EFContext context;

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WebApi.Tools.Deserializer.ArticleDeserializer" /> class.
        /// </summary>
        /// <param name="pathToFolderWithPath">
        ///     The path to folder with path.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public ArticleDeserializer(string pathToFolderWithPath, EFContext context)
            : base(pathToFolderWithPath)
        {
            this.context = context;
        }

        /// <inheritdoc />
        /// <summary>
        ///     The save objects.
        /// </summary>
        /// <param name="objects">
        ///     The objects.
        /// </param>
        protected override void SaveObjects(ref ICollection<Chapter> objects)
        {
            foreach (var chapter in objects)
            {
                foreach (var chapterContent in chapter.Contents)
                    this.context.Articles.Add(new Article(chapterContent.Title, chapterContent.Response));

                this.context.SaveChanges();
            }
        }
    }
}