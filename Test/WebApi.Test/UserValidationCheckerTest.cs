using WebApi.Tools.UserValidationChecker;
using Xunit;

namespace WebApi.Test
{
    public class UserValidationCheckerTest
    {
        public UserValidationCheckerTest()
        {
            
        }
        [Fact]
        public void CheckUserValidationByLogin()
        {
            var userValidation = new UserValidationChecker("login", "pAAS",
                "https://partner-api.1c.ru/api/ws/subscription/v2");
         
            Assert.NotNull(userValidation);
        }
    }
}