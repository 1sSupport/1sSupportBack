namespace WebApi.Test
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Text;
    using WebApi.Controllers;
    using WebApi.EF.Models;
    using WebApi.Models;
    using Xunit;

    /// <summary>
    /// The token controller test.
    /// </summary>
    public class TokenControllerTest : IDisposable
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// The parameters.
        /// </summary>
        private readonly TokenValidationParameters parameters;

        /// <summary>
        /// The user.
        /// </summary>
        private readonly User user;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenControllerTest"/> class.
        /// </summary>
        public TokenControllerTest()
        {
            parameters = new TokenValidationParameters()
                             {
                                 IssuerSigningKey =
                                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongKeyForTesting")),
                                 ValidateIssuer = true,
                                 ValidateAudience = true,
                                 ValidIssuer = $"Issuer",
                                 ValidAudience = $"Audience",
                             };
            context = new EFContext(new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options);
            user = new User("test", "test@test", "123456789000");
            context.Users.Add(user);
            context.SaveChanges();
        }

        /// <summary>
        /// The can get token.
        /// </summary>
        [Fact]
        public async void CanGetToken()
        {
            var userIfo = new UserInfo { Inn = user.INN, Login = user.Login };

            TokenController target = new TokenController(parameters, context);

            var token = await target.CreateToken(userIfo);

            Assert.NotNull(token);
            Assert.IsType<OkObjectResult>(token);
        }

        /// <summary>
        /// The can inicialization.
        /// </summary>
        [Fact]
        public void CanInicialization()
        {
            TokenController target = new TokenController(parameters, context);

            Assert.NotNull(target);
        }

        /// <inheritdoc />
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            CanInicialization();
            context.Dispose();
        }
    }
}