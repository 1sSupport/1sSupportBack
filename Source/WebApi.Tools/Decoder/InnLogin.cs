// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InnLogin.cs" company="">
//   
// </copyright>
// <summary>
//   The inn login.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Decoder
{
    /// <summary>
    ///     The inn login.
    /// </summary>
    public class InnLogin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnLogin"/> class.
        /// </summary>
        /// <param name="inn">
        /// The inn.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        public InnLogin(string inn, string login)
        {
            this.Inn = inn;
            this.Login = login;
        }

        /// <summary>
        ///     Gets the inn.
        /// </summary>
        public string Inn { get; }

        /// <summary>
        ///     Gets the login.
        /// </summary>
        public string Login { get; }
    }
}