using System;

namespace WebApi.Tools.Decoder
{
    public class Decoder: IDecoder
    {
        string IDecoder.GetDecodedString(string encodeString)
        {
            var INN = "";
            var Login = "";
            var decodedString = GetDecodedString(encodeString);
            return decodedString;
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
