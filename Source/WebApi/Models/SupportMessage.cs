// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupportMessage.cs" company="">
//   
// </copyright>
// <summary>
//   The support message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     The support message.
    /// </summary>
    public class SupportMessage
    {
        /// <summary>
        ///     Gets or sets the contact data.
        /// </summary>
        [Required]
        public string ContactData { get; set; }

        /// <summary>
        ///     Gets or sets the session id.
        /// </summary>
        [Required]
        public int SessionId { get; set; }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        [Required]
        public string Title { get; set; }
    }
}