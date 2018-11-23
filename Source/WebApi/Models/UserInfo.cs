// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="">
//   
// </copyright>
// <summary>
//   The user info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     The user info.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        ///     Gets or sets the inn.
        /// </summary>
        [Required]
        [MinLength(12)]
        [MaxLength(12)]
        public string Inn { get; set; }

        /// <summary>
        ///     Gets or sets the login.
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Login { get; set; }
    }
}