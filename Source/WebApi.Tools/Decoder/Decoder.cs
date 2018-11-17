namespace WebApi.Tools.Decoder
{
    using System;
    using System.Text.RegularExpressions;

    public class Decoder : IDecoder
    {
        public InnLogin GetInnLoginFromString(string encodeString)
        {
            var INN = string.Empty;
            var Login = string.Empty;
            var decodedString = GetDecodedString(encodeString);
            var result = GetInnLogin(decodedString);
            if (result == null)
            {
                throw new Exception("Invalid encoding string");
            }

            return result;
        }

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

        private InnLogin GetInnLogin(string decodedString)
        {
            InnLogin result = null;
            var match = Regex.Match(decodedString, @"inn=(?<inn>.*)&login=(?<login>.*)");
            if (!match.Success)
            {
                return null;
            }

            var inn = match.Groups["inn"].Value;
            var login = match.Groups["login"].Value;
            if (inn != string.Empty && login != string.Empty)
            {
                result = new InnLogin(inn, login);
            }

            return result;
        }
    }
}