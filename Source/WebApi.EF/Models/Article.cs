// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Article.cs" company="">
//   
// </copyright>
// <summary>
//   The article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace WebApi.EF.Models
{
    using System;
    using System.IO;

    using Newtonsoft.Json.Linq;

    /// <summary>
    ///     The article.
    /// </summary>
    public partial class Article
    {

       private static JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        ///     The get text.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public virtual string GetText()
        {
            // var serializer = new JsonSerializer();
            var file = new FileInfo(Path.Combine(Environment.CurrentDirectory, "articles", this.FileName));
            string text;
            using (var stream = file.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                            var item = serializer.Deserialize<SaveArticle>(jsonTextReader);
                            text = item.Content;
                    }
                }
            }

            return text;
        }
    }
}