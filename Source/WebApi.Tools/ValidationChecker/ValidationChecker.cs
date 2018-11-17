// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationChecker.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ValidationChecker type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.ValidationChecker
{
    #region

    using RestSharp;
    using RestSharp.Deserializers;
    using System;
    using System.Text;

    #endregion

    /// <summary>
    /// The validation checker.
    /// </summary>
    public class ValidationChecker
    {
        /// <summary>
        /// The valid code.
        /// </summary>
        private const string ValidCode = "1";

        /// <summary>
        /// The server.
        /// </summary>
        private readonly string server;

        /// <summary>
        /// The token.
        /// </summary>
        private readonly string token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationChecker"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="server">
        /// The server.
        /// </param>
        public ValidationChecker(string login, string password, string server)
        {
            this.server = server;
            var tokenToEncode = $"{login}:{password}";
            token = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenToEncode));
        }

        /// <summary>
        /// The get user validation.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public bool GetUserValidation(string login)
        {
            var isValid = false;
            var client = new RestClient(server);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "text/xml");
            request.AddHeader("authorization", $"Basic {token}");
            var requestBody =
                $"<Envelope xmlns=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n<Body>\r\n<checkItsByLogin xmlns=\"http://api.repository.onec.ru/v2\">\r\n<loginList xmlns=\"\">\r\n<loginList>{login}</loginList>\r\n</loginList>\r\n</checkItsByLogin>\r\n</Body>\r\n</Envelope>";
            request.AddParameter("text/xml", requestBody, ParameterType.RequestBody);
            var response = client.Execute(request);
            var serializer = new XmlDeserializer();
            var responseObject = serializer.Deserialize<Envelope>(response);
            try
            {
                var code = responseObject.Body.CheckItsByLoginResponse.Return.Code;
                isValid = IsValidFromCode(code);
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException("Invalid server answer", ex);
            }

            return isValid;
        }

        /// <summary>
        /// The is valid from code.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidFromCode(string code)
        {
            return code.Equals(ValidCode);
        }
    }
}