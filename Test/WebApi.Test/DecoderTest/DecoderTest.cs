namespace WebApi.Test.DecoderTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    using WebApi.Tools.Decoder;

    using Xunit;

    public class DecoderTest
    {
        [Fact]
        public void DecoderCreationTest()
        {
            var decoder = new Decoder();
            Assert.NotNull(decoder);
        }

        [Fact]
        public void GetInnLoginFromInvalidStringTest()
        {
            var decoder = new Decoder();
            Assert.Throws<Exception>(() => decoder.GetInnLoginFromString("invalidString"));
        }

        [Fact]
        public void GetInnLoginFromStringTest()
        {
            var testData = GetTestData();
            var decoder = new Decoder();
            foreach (var tuple in testData)
            {
                var result = decoder.GetInnLoginFromString(tuple.Item3);
                Assert.Equal(tuple.Item1, result.Inn);
                Assert.Equal(tuple.Item2, result.Login);
            }
        }

        [Fact]
        public void InnLoginCreationTest()
        {
            var innLogin = new InnLogin("inn", "login");
            Assert.NotNull(innLogin);
            Assert.Equal("inn", innLogin.Inn);
            Assert.Equal("login", innLogin.Login);
        }

        private List<Tuple<string, string, string>> GetTestData()
        {
            var testData = new List<Tuple<string, string, string>>();
            var path = @"C:\Users\Анна\source\repos\1sSupportBack\Test\WebApi.Test\DecoderTest\links.json";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                var text = sr.ReadToEnd();
                var matches = Regex.Matches(
                    text,
                    "inn=(?<inn>.*?)&login=(?<login>.*?)\".*?\"linnk_encode\": \"(?<link>.*?)\"",
                    RegexOptions.Singleline);
                foreach (Match match in matches)
                {
                    var inn = match.Groups["inn"].Value;
                    var login = match.Groups["login"].Value;
                    var link = match.Groups["link"].Value;
                    var pair = new Tuple<string, string, string>(inn, login, link);
                    testData.Add(pair);
                }
            }

            return testData;
        }
    }
}