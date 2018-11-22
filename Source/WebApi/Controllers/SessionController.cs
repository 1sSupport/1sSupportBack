// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionController.cs" company="">
//
// </copyright>
// <summary>
//   The session controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using NETCore.MailKit.Core;

namespace WebApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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

        private readonly IEmailService emailService;
        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionController" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        public SessionController(EFContext context, IEmailService emailService)
        {
            this.context = context;
            this.emailService = emailService;
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
            var user = await this.GetUserFromHttpContext().ConfigureAwait(false);

            var session =
                await (from s in this.context.Sessions where s.Id == id && s.User.Login.Equals(user.Login) select s)
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
                return BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(id);
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
                return BadRequest(new { message = "Что-то пошло не так" });
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

            var session = new Session(date, user);

            this.context.Sessions.Add(session);
            try
            {
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(new { SessionId = session.Id, User = user.Login });
        }
        [HttpPost]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateSupportMessage(SupportMessage message)
        {

            if (ModelState.IsValid)
            {
                SupportAsk supportAsk;
                var session = await (from s in context.Sessions where s.Id == message.SessionId select s).FirstOrDefaultAsync().ConfigureAwait(true);
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
                    context.SupportAsk.Add(supportAsk);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(new {message = "Что-то пошло не так"});
                }

                var user = await GetUserFromHttpContext();           
                var providerMail =
                    await (from p in context.Providers
                        from u in context.Users
                        where ((u.Id == user.Id) && p.Id == u.Provider.Id)
                        select p.SupportEmail).FirstOrDefaultAsync().ConfigureAwait(false); 
                await SendSupportMessages(
                    new List<string> {providerMail, "govjadkoilja@yandex.ru", "krumih@mail.ru"},
                    message.Title, message.Text);
                return Ok(supportAsk.Id);


            }

            return BadRequest();
        }

        private async Task SendSupportMessages(IEnumerable<string> emailsTo, string title,string text)
        {
            foreach (var email in emailsTo)
            {
                try
                {
                    await emailService.SendAsync(email, title, text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                 
                }
               
            }
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

        public class SupportMessage
        {
            public int SessionId { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string ContactData { get; set; }
        }
    }
}