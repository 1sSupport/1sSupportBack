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

    using Newtonsoft.Json;
    using System.Collections.Generic;
    using WebApi.EF.Models;

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
        /// Initializes a new instance of the <see cref="T:WebApi.Tools.Deserializer.ArticleDeserializer" /> class.
        /// </summary>
        /// <param name="pathToFolderWithPath">
        /// The path to folder with path.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public ArticleDeserializer(string pathToFolderWithPath, EFContext context)
            : base(pathToFolderWithPath)
        {
            this.context = context;
        }

        /// <inheritdoc />
        /// <summary>
        /// The save objects.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        protected override void SaveObjects(ref ICollection<Chapter> objects)
        {
            foreach (var chapter in objects)
            {
                foreach (var chapterContent in chapter.contents)
                {
                    context.Articles.Add(new Article(chapterContent.title, chapterContent.response));
                }

                context.SaveChanges();
            }
        }
    }

    public class Content
    {
        public string link { get; set; }

        public string response { get; set; }

        public string title { get; set; }
    }

    public class Chapter
    {
        public List<Content> contents { get; set; }

        [JsonIgnore]
        public string FileName { get; set; }

        public string link { get; set; }

        public string title { get; set; }
    }
}