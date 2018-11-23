// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Decoder.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Decoder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Decoder
{
    using System;
    using System.Text.RegularExpressions;

    /// <inheritdoc />
    /// <summary>
    ///     The decoder.
    /// </summary>
    public class Decoder : IDecoder
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
        /// <exception cref="Exception">
        /// </exception>
        public InnLogin GetInnLoginFromString(string encodeString)
        {
            var INN = string.Empty;
            var Login = string.Empty;
            var decodedString = this.GetDecodedString(encodeString);
            var result = this.GetInnLogin(decodedString);
            if (result == null) throw new Exception("Invalid encoding string");

            return result;
        }

        /// <summary>
        /// The get decoded string.
        /// </summary>
        /// <param name="encodedString">
        /// The encoded string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetDecodedString(string encodedString)
        {
            var decodedCharArray = new char[encodedString.Length];
            var i = 0;
            foreach (var symbol in encodedString)
            {
                decodedCharArray[i] = Convert.ToChar(symbol - 3);
                i++;
            }

            var result = new string(decodedCharArray);
            return result;
        }

        /// <summary>
        /// The get inn login.
        /// </summary>
        /// <param name="decodedString">
        /// The decoded string.
        /// </param>
        /// <returns>
        /// The <see cref="InnLogin"/>.
        /// </returns>
        private InnLogin GetInnLogin(string decodedString)
        {
            InnLogin result = null;
            var match = Regex.Match(decodedString, @"inn=(?<inn>.*)&login=(?<login>.*)");
            if (!match.Success) return null;

            var inn = match.Groups["inn"].Value;
            var login = match.Groups["login"].Value;
            if (inn != string.Empty && login != string.Empty) result = new InnLogin(inn, login);

            return result;
        }
    }
}