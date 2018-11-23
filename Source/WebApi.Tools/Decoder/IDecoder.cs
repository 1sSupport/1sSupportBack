// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDecoder.cs" company="">
//   
// </copyright>
// <summary>
//   The Decoder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Decoder
{
    /// <summary>
    ///     The Decoder interface.
    /// </summary>
    internal interface IDecoder
    {
        /// <summary>
        /// The get inn login from string.
        /// </summary>
        /// <param name="encodeString">
        /// The encode string.
        /// </param>
        /// <returns>
        /// The <see cref="InnLogin"/>.
        /// </returns>
        InnLogin GetInnLoginFromString(string encodeString);
    }
}