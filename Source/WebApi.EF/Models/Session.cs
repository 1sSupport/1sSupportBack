namespace WebApi.EF.Models
{
    using System;

    /// <summary>
    /// The session.
    /// </summary>
    public partial class Session
    {
        /// <summary>
        /// The end seddion.
        /// </summary>
        public void EndSession()
        {
            CloseTime = DateTime.UtcNow;
        }
    }
}