// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleControllerTest.cs" company="">
//   
// </copyright>
// <summary>
//   The article controller test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using Microsoft.EntityFrameworkCore;

    using WebApi.Controllers;
    using WebApi.EF.Models;

    using Xunit;

    /// <summary>
    /// The article controller test.
    /// </summary>
    public class ArticleControllerTest
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleControllerTest"/> class.
        /// </summary>
        public ArticleControllerTest()
        {
            this.context = new EFContext(
                new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options);
        }

        /// <summary>
        /// The can initialize.
        /// </summary>
        [Fact]
        public void CanInitialize()
        {
            var target = new ArticleController(this.context);
        }
    }
}