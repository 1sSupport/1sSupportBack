using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace WebApi.Tools.UserValidationChecker
{
    public class UserValidationChecker
    {
        private readonly string _token;
        private readonly string _server;
        private const string ValidCode = "1";

        public UserValidationChecker(string login, string password, string server)
        {
            _server = server;
            var tokenToEncode = $"{login}:{password}";
            _token = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenToEncode));
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
            XmlDeserializer serializer = new XmlDeserializer();
            var responseObject = serializer.Deserialize<Envelope>(response);
            try
            {
                var code = responseObject.Body.CheckItsByLoginResponse.Return.Code;
                isValid = IsValidFromCode(code);
            }
            catch (NullReferenceException)
            {
                
            }
            return isValid;
        }

        private bool IsValidFromCode(string code)
        {
            if (code.Equals(ValidCode)) return true;
            return false;
        }
    }
}
