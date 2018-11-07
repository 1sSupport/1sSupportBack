using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RestSharp;

namespace WebApi.Tools.UserValidationChecker
{
    public class UserValidationChecker
    {
        private readonly string _token;
        private readonly string _server;
        private const string ValidCode = "107";

        public UserValidationChecker(string login, string password, string server)
        {
            _server = server;
            var tokenToEncode = $"{login}:{password}";
            byte[] encodingTokenBytes = new byte[tokenToEncode.Length];
            for (int i = 0; i < tokenToEncode.Length; i++)
            {
                encodingTokenBytes[i] = Convert.ToByte(tokenToEncode[i]);
            }

            _token = Convert.ToBase64String(encodingTokenBytes);
        }

        public bool GetUserValidation(string login)
        {
            var isValid = false;
            var client = new RestClient(_server);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "text/xml");
            request.AddHeader("authorization", $"Basic {_token}");
            var requestBody =
                $"<Envelope xmlns=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n<Body>\r\n<checkItsByLogin xmlns=\"http://api.repository.onec.ru/v2\">\r\n<loginList xmlns=\"\">\r\n<loginList>{login}</loginList>\r\n</loginList>\r\n</checkItsByLogin>\r\n</Body>\r\n</Envelope>";
            request.AddParameter("text/xml", requestBody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var code = GetCodeFromResponse(response.Content);
            isValid = IsValidFromCode(code);
            return isValid;
        }

        private bool IsValidFromCode(string code)
        {
            if (code.Equals(ValidCode)) return true;
            return false;
        }

        private string GetCodeFromResponse(string responseContent)
        {
            var match = Regex.Match(responseContent, @"<code>(?<code>[0-9]*)</code>");
            var code = match.Groups["code"].Value;
            return code;
        }
    }
}
