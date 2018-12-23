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
    using System.Linq;

    using WebApi.EF.Models;
    using WebApi.Tools.Parser;

    using Article = WebApi.EF.Models.Article;

    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     The article deserializator.
    /// </summary>
    public class ArticleDeserializer : Deserializer<SaveArticle>
    {
        /// <summary>
        ///     The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleDeserializer"/> class.
        /// </summary>
        /// <param name="pathToFolder">
        /// The path to folder.
        /// </param>
        /// <param name="context">
        /// The context.
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
        protected override void SaveObjects()
        {
            foreach (var myObject in this.objects)
            {
                var path = $"{myObject.Id}.json";

                var article = new Article(myObject.Title, path) { EditDate = DateTime.UtcNow };

                article.Preview = new string(article.GetText().TextWithoutHtmlTag().Take(300).ToArray());

                this.context.Articles.Add(article);
            }
        }
    }
}