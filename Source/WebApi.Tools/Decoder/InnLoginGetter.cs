using System;
using System.Text.RegularExpressions;

namespace WebApi.Tools.Decoder
{
    public class InnLoginGetter
    {
        private readonly IDecoder _decoder;

        public InnLoginGetter(IDecoder decoder)
        {
            this._decoder = decoder;
        }
        public InnLogin GetInnLogin(string encodedString)
        {
            var decodedString = _decoder.GetDecodedString(encodedString);
            InnLogin result = null;
            var match = Regex.Match(decodedString, @"inn=(?<inn>.*)&login=(?<login>.*)");
            if (!match.Success) throw new Exception("Invalid encoded string");
            var inn = match.Groups["inn"].Value;
            var login = match.Groups["login"].Value;
            if (inn != "" && login != "")
            {
                result = new InnLogin(inn, login);
            }

            return result;
        }
    }
}
