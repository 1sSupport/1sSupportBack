// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Version.cs" company="">
//   
// </copyright>
// <summary>
//   The version.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Deserializer.Models
{
    /// <summary>
    ///     The version.
    /// </summary>
    public class Version
    {
        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}