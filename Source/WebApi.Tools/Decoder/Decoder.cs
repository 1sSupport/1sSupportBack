using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApi.Tools.Decoder
{
    public class Decoder: IDecoder
    {
        public InnLogin GetInnLoginFromString(string encodeString)
        {
            var INN = "";
            var Login = "";
            var decodedString = GetDecodedString(encodeString);
            var result = GetInnLogin(decodedString);
            if(result == null) throw new Exception("Invalid encoding string");
            return result;
        }

        private InnLogin GetInnLogin(string decodedString)
        {
            InnLogin result = null;
            var match = Regex.Match(decodedString, @"inn=(?<inn>.*)&login=(?<login>.*)");
            if (!match.Success) return null;
            var inn = match.Groups["inn"].Value;
            var login = match.Groups["login"].Value;
            if (inn != "" && login != "")
            {
                result = new InnLogin(inn, login);
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
    }
}
