// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionController.cs" company="">
//
// </copyright>
// <summary>
//   The session controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.EF.Models;
    using WebApi.Models;

    /// <inheritdoc />
    /// <summary>
    ///     The session controller.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SessionController : ControllerBase
    {
        /// <summary>
        ///     The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionController" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        public SessionController(EFContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///     The end session.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EndSession([FromBody] int id)
        {
            var user = await GetUserFromHttpContext().ConfigureAwait(false);

            var session =
                await (from s in context.Sessions where s.Id == id && s.User.Login.Equals(user.Login) select s)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

            if (session == null || session.CloseTime != null)
            {
                return BadRequest(new { message = "Сессия была не создана либо уже закрыта" });
            }

            session.EndSession();

            await context.SaveChangesAsync().ConfigureAwait(false);

            return Ok(id);
        }

        /// <summary>
        ///     The set mark.
        /// </summary>
        /// <param name="markedArticle">
        ///     The marked article.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SetMark([FromBody] MarkArticle markedArticle)
        {
            var user = await GetUserFromHttpContext().ConfigureAwait(false);

            var openArticle = await (from s in context.Sessions
                                     where s.Id == markedArticle.SessionId && s.User.Id == user.Id
                                     from sq in s.SearchingQuery
                                     from oa in sq.OpenedArticle
                                     where oa.Article.Id == markedArticle.ArticleId
                                     select oa).FirstOrDefaultAsync().ConfigureAwait(false);

            if (openArticle == null)
            {
                return NotFound(new { message = "Не было найденно открытой статьи и таким ID" });
            }

            openArticle.Mark = markedArticle.Mark;

            await context.SaveChangesAsync().ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        ///     The start session.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> StartSession()
        {
            var date = DateTime.UtcNow;

            var user = await GetUserFromHttpContext().ConfigureAwait(false);

            var session = new Session(date, user);

            context.Sessions.Add(session);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return Ok(new { SessionId = session.Id, User = user.Login });
        }

        /// <summary>
        ///     The get user from http context.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private Task<User> GetUserFromHttpContext()
        {
            var userInfo = new UserInfo { Inn = User.FindFirst("Inn").Value, Login = User.FindFirst("Login").Value };

            return (from u in context.Users where u.INN == userInfo.Inn || u.Login == userInfo.Login select u)
                .FirstOrDefaultAsync();
        }
    }
}