using System.Collections.Generic;
using System.IO;
using WebApi.Tools.UserValidationChecker;
using Xunit;

namespace WebApi.Test.UserValidationCheck
{
    public class UserValidationCheckerTest
    {
        private string _login = "";
        private string _password = "";
        private List<string> _usersList = new List<string>();
        public UserValidationCheckerTest()
        {
            var path = @"C:\Users\Анна\source\repos\1sSupportBack\Test\WebApi.Test\UserValidationCheck\TestData.txt";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                var first = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (first)
                    {
                        var position = line.IndexOf(":");
                        _login = line.Substring(0, position);
                        _password = line.Substring(position + 1, line.Length - position - 1);
                        first = false;
                        continue;
                    }
                    _usersList.Add(line);
                }
            }
        }

        [Fact]
        public void UserValidationCheckerCreateTest()
        {
            var userValidation = new UserValidationChecker("login", "password",
                "api");
            Assert.NotNull(userValidation);
        }

        [Fact]
        public void CheckUserValidationByLoginIsCorrect()
        {
            var userValidation = new UserValidationChecker(_login, _password,
                "https://partner-api.1c.ru/api/ws/subscription/v2");
            foreach (var user in _usersList)
            {
                var isValid = userValidation.GetUserValidation(user);
                Assert.True(isValid);
            }
        }

        [Fact]
        public void CheckUserValidationByLoginIsInCorrect()
        {
            var userValidation = new UserValidationChecker(_login, _password,
                "https://partner-api.1c.ru/api/ws/subscription/v2");
            var isValid = userValidation.GetUserValidation("login");
            Assert.False(isValid);
        }
    }
}