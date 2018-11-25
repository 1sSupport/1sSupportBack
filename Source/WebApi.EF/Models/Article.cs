// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Article.cs" company="">
//   
// </copyright>
// <summary>
//   The article.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        /// <summary>
        ///     The get text.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public virtual string GetText()
        {
            // var serializer = new JsonSerializer();
            var file = new FileInfo(Path.Combine(Environment.CurrentDirectory, "articels", FileName));
            string text;
            using (var stream = file.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonReader = reader.ReadToEnd();
                    var jo = JObject.Parse(jsonReader);
                    text = jo["Response"].ToString();
                }
            }

            return text;
        }
    }
}