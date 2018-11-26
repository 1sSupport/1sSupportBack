// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupportAsk.cs" company="">
//   
// </copyright>
// <summary>
//   The support ask.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.EF.Models
{
    /// <summary>
    /// The support ask.
    /// </summary>
    public partial class SupportAsk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupportAsk"/> class.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="contactInfo">
        /// The contact info.
        /// </param>
        public SupportAsk(Session session, string title, string text, string contactInfo)
            : this(text, contactInfo, session)
        {
            this.AskTitle.Add(new AskTitle(title, this));
        }
    }
}