// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserValidationCheckerTest.cs" company="">
//   
// </copyright>
// <summary>
//   The user validation checker test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test.UserValidationCheck
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using WebApi.Tools.ValidationChecker;

    using Xunit;

    /// <summary>
    /// The user validation checker test.
    /// </summary>
    public class UserValidationCheckerTest
    {
        /// <summary>
        /// The _login.
        /// </summary>
        private readonly string _login = string.Empty;

        /// <summary>
        /// The _password.
        /// </summary>
        private readonly string _password = string.Empty;

        /// <summary>
        /// The _users list.
        /// </summary>
        private readonly List<string> _usersList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationCheckerTest"/> class.
        /// </summary>
        public UserValidationCheckerTest()
        {
            var path = @"C:\Users\Анна\source\repos\1sSupportBack\Test\WebApi.Test\UserValidationCheck\TestData.txt";
            using (var sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                var first = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (first)
                    {
                        var position = line.IndexOf(":");
                        this._login = line.Substring(0, position);
                        this._password = line.Substring(position + 1, line.Length - position - 1);
                        first = false;
                        continue;
                    }

                    this._usersList.Add(line);
                }
            }
        }

        /// <summary>
        /// The check user validation by login is correct.
        /// </summary>
        [Fact]
        public void CheckUserValidationByLoginIsCorrect()
        {
            var userValidation = new ValidationChecker(
                this._login,
                this._password,
                "https://partner-api.1c.ru/api/ws/subscription/v2");
            foreach (var user in this._usersList)
            {
                var isValid = userValidation.GetUserValidation(user);
                Assert.True(isValid);
            }
        }

        /// <summary>
        /// The check user validation by login is in correct.
        /// </summary>
        [Fact]
        public void CheckUserValidationByLoginIsInCorrect()
        {
            var userValidation = new ValidationChecker(
                this._login,
                this._password,
                "https://partner-api.1c.ru/api/ws/subscription/v2");
            var isValid = userValidation.GetUserValidation("login");
            Assert.False(isValid);
        }

        /// <summary>
        /// The user validation checker create test.
        /// </summary>
        [Fact]
        public void UserValidationCheckerCreateTest()
        {
            var userValidation = new ValidationChecker("login", "password", "api");
            Assert.NotNull(userValidation);
        }
    }
}