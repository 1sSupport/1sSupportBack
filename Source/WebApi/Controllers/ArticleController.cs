// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ArticleController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Controllers
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using WebApi.EF.Models;
    using WebApi.Infrastructer;
    using WebApi.Tools.Finder;

    #endregion

    /// <inheritdoc />
    /// <summary>
    ///     The article controller.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ArticleController : ControllerBase
    {
        /// <summary>
        ///     The _context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public ArticleController(EFContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// The get article.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> GetArticle(
            [FromQuery] [Required] [Range(1, int.MaxValue)]
            int id,
            [FromQuery] [Required] string query)
        {
            var article = await (from a in this.context.Articles where a.Id == id select a).FirstOrDefaultAsync()
                              .ConfigureAwait(false);

            var queryDb = await (from q in this.context.SearchingQueryes where q.Text == query select q)
                              .FirstOrDefaultAsync().ConfigureAwait(false);

            if (article == null || queryDb == null)
                return this.BadRequest(new { message = $"Не найденно {id} || {query}" });

            var user = await this.User.GetUserFromDbInContextAsync(this.context).ConfigureAwait(false);

            var userSessionQuary =
                await (from q in this.context.SessionQueries
                       where q.Session.User.Id == user.Id && q.Session.CloseTime == null
                       select q).FirstOrDefaultAsync().ConfigureAwait(false);

            var openedArticle = new OpenedArticle(DateTime.UtcNow, article, userSessionQuary);

            try
            {
                this.context.OpenedArticles.Add(openedArticle);
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            try
            {
                return this.Ok(new { article.Id, article.Title, Text = article.GetText() });
            }
            catch (Exception ex)
            {
                // this.logger.LogCritical(e, $"Сломались {nameof(this.GetArticle)}");
                return this.BadRequest(new { id, message = "Данной статьи не было найдено" });
            }
        }

        /// <summary>
        /// The get articles by query.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="sessionId">
        /// The session Id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetArticlesByQuery(
            [FromQuery] [Required] string query,
            [FromQuery] [Required] [Range(0, int.MaxValue)]
            int sessionId)
        {
            var articles = await Task.Run(
                               () =>
                                   {
                                       var finder = new ArticleFinder(this.context);
                                       return finder.GetArticlesByQuery(query);
                                   }).ConfigureAwait(false);

            if (articles == null || !articles.Any()) return this.Ok(null);

            var session = await (from s in this.context.Sessions where s.Id == sessionId select s).FirstOrDefaultAsync()
                              .ConfigureAwait(false);

            try
            {
                var searchingQuery =
                    await (from q in this.context.SearchingQueryes where q.Text == query.ToLower() select q)
                        .FirstOrDefaultAsync().ConfigureAwait(false);
                if (searchingQuery == null)
                {
                    searchingQuery = new SearchingQuery(query.ToLower());
                    await this.context.SearchingQueryes.AddAsync(searchingQuery).ConfigureAwait(false);
                }
                else
                {
                    lock (searchingQuery)
                    {
                        searchingQuery.Amount++;
                        this.context.SearchingQueryes.Update(searchingQuery);
                    }
                }

                var sessionQuary = new SessionQuery(DateTime.Now, session, searchingQuery);

                await this.context.SessionQueries.AddAsync(sessionQuary).ConfigureAwait(false);

                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // this.logger.Log(LogLevel.Critical, e, $"Сломались {nameof(this.GetArticlesByQuery)}");
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok((from a in articles select new { a.Id, a.Title, Text = a.Preview }).ToList());
        }

        /// <summary>
        /// The get marks.
        /// </summary>
        /// <param name="n">
        /// The n.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(100)]
        public async Task<IActionResult> GetMarks([FromQuery] int n = 5)
        {
            try
            {
                var marks = await (from m in this.context.SearchingQueryes where m.Amount >= n select m.Text)
                                .ToListAsync().ConfigureAwait(false);
                return this.Ok(marks.Distinct());
            }
            catch (Exception e)
            {
                // this.logger.Log(LogLevel.Critical, e, "Сломались MArks");
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }
        }
    }
}