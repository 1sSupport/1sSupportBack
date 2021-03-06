﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SessionController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using NETCore.MailKit.Core;

    using WebApi.EF.Models;
    using WebApi.Infrastructer;
    using WebApi.Models;

    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     The session controller.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        // <summary>
        // The loger.
        /// </summary>
        // private readonly ILogger loger;

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

            // this.loger = logger;
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
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> CreateSupportMessage([Required] SupportMessage message)
        {
            if (!this.ModelState.IsValid) return this.BadRequest();
            try
            {
                var session = await (from s in this.context.Sessions where s.Id == message.SessionId select s)
                                  .FirstOrDefaultAsync().ConfigureAwait(true);

                var asktitle = await (from t in this.context.AskTitle where t.Text == message.Title select t)
                                   .FirstOrDefaultAsync().ConfigureAwait(false);

               

                var user = await this.User.GetUserFromDbInContextAsync(this.context).ConfigureAwait(false);

                var providerMail =
                    await (from p in this.context.Providers
                           from u in this.context.Users
                           where u.Id == user.Id && p.Id == u.Provider.Id
                           select p.SupportEmail).ToListAsync().ConfigureAwait(false);
                providerMail.Add("ibigcall@gmail.com");

                await this.SendSupportMessages(providerMail, message).ConfigureAwait(false);

                var supportAsk = new SupportAsk(message.Text, message.ContactData, asktitle, session);

                this.context.SupportAsk.Add(supportAsk);
                await this.context.SaveChangesAsync().ConfigureAwait(false);

                return this.Ok(supportAsk.Id);
            }
            catch (Exception e)
            {
                // this.loger.Fatal(e, $"Сломались {nameof(this.CreateSupportMessage)}");
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }
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
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> EndSession(
            [FromBody] [Required] [Range(0, int.MaxValue)]
            int id)
        {
            var user = await this.User.GetUserFromDbInContextAsync(this.context).ConfigureAwait(false);

            var session =
                await (from s in this.context.Sessions
                       where s.Id == id && s.User.Id == user.Id && s.CloseTime == null
                       select s).FirstOrDefaultAsync().ConfigureAwait(false);

            if (session == null)
                return this.BadRequest(new { message = "Сессия была не создана либо уже закрыта" });

            session.EndSession();

            try
            {
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // this.loger.Fatal(e, $"Сломались {nameof(this.EndSession)}");
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(id);
        }

        /// <summary>
        ///     The get support message title.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> GetSupportMessageTitle()
        {
            try
            {
                var title = await (from m in this.context.AskTitle select m.Text).ToListAsync().ConfigureAwait(false);
                return this.Ok(title);
            }
            catch (Exception e)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }
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
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> SetMark([FromBody] [Required] MarkArticle markedArticle)
        {
            var user = await this.User.GetUserFromDbInContextAsync(this.context).ConfigureAwait(false);

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
            catch (Exception e)
            {
                // this.loger.Fatal(e, $"Сломались {nameof(this.SetMark)}");
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
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> StartSession()
        {
            var date = DateTime.UtcNow;

            User user = null;
            try
            {
                user = await this.User.GetUserFromDbInContextAsync(this.context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var session =
                await (from s in this.context.Sessions where s.CloseTime == null && s.User.Id == user.Id select s)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

            if (session != null) return this.Ok(new { SessionId = session.Id, User = user.Login });

            session = new Session(date, user);

            this.context.Sessions.Add(session);
            try
            {
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // this.loger.Fatal(e, $"Сломались {nameof(this.StartSession)}");
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(new { SessionId = session.Id, User = user.Login });
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
        private async Task SendSupportMessages(IEnumerable<string> emailsTo, SupportMessage message)
        {
            foreach (var email in emailsTo)
                try
                {
                    await this.emailService.SendAsync(email, message.Title, $"{message.Text}{Environment.NewLine}Данные для связи:{message.ContactData}").ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw e;
                    // this.loger.Fatal(e, $"Сломались {nameof(this.SendSupportMessages)}");
                }
        }
    }
}