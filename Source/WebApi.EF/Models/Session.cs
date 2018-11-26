// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="">
//   
// </copyright>
// <summary>
//   The session.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.EF.Models
{
    using System;

    /// <summary>
    ///     The session.
    /// </summary>
    public partial class Session
    {
        /// <summary>
        ///     The end seddion.
        /// </summary>
        public void EndSession()
        {
            this.CloseTime = DateTime.UtcNow;
        }
    }
}