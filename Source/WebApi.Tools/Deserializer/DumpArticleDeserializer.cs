// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DumpArticleDeserializer.cs" company="">
//   
// </copyright>
// <summary>
//   The dump article deserializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using WebApi.EF.Models;

namespace WebApi.Tools.Deserializer
{
    #region

    using System;
    using System.Collections.Generic;
    

    #endregion

    /// <summary>
    ///     The dump article deserializer.
    /// </summary>
    internal class DumpArticleDeserializer : Deserializer<DumpArticle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DumpArticleDeserializer"/> class.
        /// </summary>
        /// <param name="pathToFolder">
        /// The path to folder.
        /// </param>
        public DumpArticleDeserializer(string pathToFolder)
            : base(pathToFolder)
        {
        }

        /// <summary>
        /// The get objects.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        public ICollection<DumpArticle> GetObjects()
        {
            return this.objects;
        }

        /// <summary>
        ///     The save objects.
        /// </summary>
        protected override void SaveObjects()
        {
        }
    }
}