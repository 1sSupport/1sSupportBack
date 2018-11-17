namespace WebApi.Test.UserValidationCheck
{
    using System.Collections.Generic;
    using System.IO;

    using WebApi.Tools.ValidationChecker;

    using Xunit;

    public class UserValidationCheckerTest
    {
        private readonly string _login = string.Empty;

        private readonly string _password = string.Empty;

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
        public void CheckUserValidationByLoginIsCorrect()
        {
            var userValidation = new ValidationChecker(
                _login,
                _password,
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
            var userValidation = new ValidationChecker(
                _login,
                _password,
                "https://partner-api.1c.ru/api/ws/subscription/v2");
            var isValid = userValidation.GetUserValidation("login");
            Assert.False(isValid);
        }

        [Fact]
        public void UserValidationCheckerCreateTest()
        {
            var userValidation = new ValidationChecker("login", "password", "api");
            Assert.NotNull(userValidation);
        }
    }
}