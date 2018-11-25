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

    using System;
    using System.Collections.Generic;
    using System.IO;

    using WebApi.EF.Models;
    using WebApi.Tools.Deserializer.Models;

    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     The article deserializator.
    /// </summary>
    public class ArticleDeserializer : Deserializer<NewArticle>
    {
        /// <summary>
        ///     The context.
        /// </summary>
        private readonly EFContext context;

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WebApi.Tools.Deserializer.ArticleDeserializer" /> class.
        /// </summary>
        /// <param name="pathToFolder">
        ///     The path to folder with path.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public ArticleDeserializer(string pathToFolder, EFContext context)
            : base(pathToFolder)
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
        protected override void SaveObjects()
        {
            foreach (var myObject in objects)
            {
                var path = $"{myObject.Id}.json";
                    this.context.Articles.Add(new Article(myObject.Title, path));
            }
        }
    }
}