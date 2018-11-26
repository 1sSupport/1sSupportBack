// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SessionController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NETCore.MailKit.Core;

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
        ///     The email service.
        /// </summary>
        private readonly IEmailService emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionController"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="emailService">
        /// The email service.
        /// </param>
        public SessionController(EFContext context, IEmailService emailService)
        {
            this.context = context;
            this.emailService = emailService;
        }

        /// <summary>
        /// The create support message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateSupportMessage([Required] SupportMessage message)
        {
            if (!this.ModelState.IsValid) return this.BadRequest();

            SupportAsk supportAsk;
            var session = await (from s in this.context.Sessions where s.Id == message.SessionId select s)
                              .FirstOrDefaultAsync().ConfigureAwait(true);
            try
            {
                supportAsk = new SupportAsk(session, message.Title, message.Text, message.ContactData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                this.context.SupportAsk.Add(supportAsk);
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            var user = await this.GetUserFromHttpContext().ConfigureAwait(false);
            var providerMail =
                await (from p in this.context.Providers
                       from u in this.context.Users
                       where u.Id == user.Id && p.Id == u.Provider.Id
                       select p.SupportEmail).FirstOrDefaultAsync().ConfigureAwait(false);
            await this.SendSupportMessages(
                new List<string> { providerMail, "govjadkoilja@yandex.ru", "krumih@mail.ru" },
                message.Title,
                message.Text).ConfigureAwait(false);
            return this.Ok(supportAsk.Id);
        }

        /// <summary>
        /// The end session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EndSession(
            [FromBody] [Required] [Range(0, int.MaxValue)]int id)
        {
            var user = await this.GetUserFromHttpContext().ConfigureAwait(false);

            var session =
                await (from s in this.context.Sessions where s.Id == id && s.User.Id == user.Id select s)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

            if (session == null || session.CloseTime != null)
                return this.BadRequest(new { message = "Сессия была не создана либо уже закрыта" });

            session.EndSession();
            try
            {
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(id);
        }

        /// <summary>
        /// The set mark.
        /// </summary>
        /// <param name="markedArticle">
        /// The marked article.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SetMark([FromBody] [Required] MarkArticle markedArticle)
        {
            var user = await this.GetUserFromHttpContext().ConfigureAwait(false);

            var openArticle = await (from s in this.context.Sessions
                                     where s.Id == markedArticle.SessionId && s.User.Id == user.Id
                                     from sq in s.SearchingQuery
                                     from oa in sq.OpenedArticle
                                     where oa.Article.Id == markedArticle.ArticleId
                                     select oa).FirstOrDefaultAsync().ConfigureAwait(false);

            if (openArticle == null)
                return this.BadRequest(new { message = "Не было найденно открытой статьи и таким ID" });

            openArticle.Mark = markedArticle.Mark;
            try
            {
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok();
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

            var user = await this.GetUserFromHttpContext().ConfigureAwait(false);

            var session = await (from s in this.context.Sessions where s.CloseTime == null && s.User.Id == user.Id select s)
                              .FirstOrDefaultAsync().ConfigureAwait(false);

            if (session != null)
            {
                return this.Ok(new { SessionId = session.Id, User = user.Login });
            }

            session = new Session(date, user);

            this.context.Sessions.Add(session);
            try
            {
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(new { SessionId = session.Id, User = user.Login });
        }

        /// <summary>
        ///     The get user from http context.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private Task<User> GetUserFromHttpContext()
        {
            var userInfo = new UserInfo
                               {
                                   Inn = this.User.FindFirst("Inn").Value, Login = this.User.FindFirst("Login").Value
                               };

            return (from u in this.context.Users where u.INN == userInfo.Inn || u.Login == userInfo.Login select u)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// The send support messages.
        /// </summary>
        /// <param name="emailsTo">
        /// The emails to.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task SendSupportMessages(IEnumerable<string> emailsTo, string title, string text)
        {
            foreach (var email in emailsTo)
                try
                {
                    await this.emailService.SendAsync(email, title, text).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }
    }
}