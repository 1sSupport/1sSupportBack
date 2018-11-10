namespace WebApi.Test
{
    using System;
    using System.Text;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    using WebApi.Controllers;
    using WebApi.EF.Models;
    using WebApi.Models;

    using Xunit;

    /// <summary>
    /// The token controller test.
    /// </summary>
    public class TokenControllerTest: IDisposable
    {
        /// <summary>
        /// The parameters.
        /// </summary>
        private readonly TokenValidationParameters parameters;

        /// <summary>
        /// The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// The user.
        /// </summary>
        private readonly User user;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenControllerTest"/> class.
        /// </summary>
        public TokenControllerTest()
        {
            this.parameters = new TokenValidationParameters()
                                  {
                                      IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongKeyForTesting")),
                                      ValidateIssuer = true,
                                      ValidateAudience = true,
                                      ValidIssuer = $"Issuer",
                                      ValidAudience = $"Audience",
            };
            context = new EFContext(new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options);
            user = new User("test", "test@test", "123456789000");
            this.context.Users.Add(this.user);
            this.context.SaveChanges();

        }

        /// <summary>
        /// The can inicialization.
        /// </summary>
        [Fact]
        public void CanInicialization()
        {
            TokenController target = new TokenController(this.parameters, this.context);

            Assert.NotNull(target);

        }

        /// <summary>
        /// The can get token.
        /// </summary>
        [Fact]
        public async void CanGetToken()
        {
            var userIfo = new UserInfo { Inn = this.user.INN, Login = this.user.Login };

            TokenController target = new TokenController(this.parameters, this.context);


            var token = await target.CreateToken(userIfo);

            Assert.NotNull(token);
            Assert.IsType<OkObjectResult>(token);
        }


        /// <inheritdoc />
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.CanInicialization();
            this.context.Dispose();
        }
    }
}
