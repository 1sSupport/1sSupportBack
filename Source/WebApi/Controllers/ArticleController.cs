﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ArticleController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using WebApi.EF.Models;
    using WebApi.Tools.Finder;

    /// <inheritdoc />
    /// <summary>
    ///     The article controller.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetArticle(
            [FromQuery] [Required] [Range(1, int.MaxValue)]
            int id,
            [FromQuery] [Required] string query)
        {
            var article = await (from a in this.context.Articles where a.Id == id select a).FirstOrDefaultAsync()
                              .ConfigureAwait(false);
            var queryDb = await (from q in this.context.SearchingQueries where q.Text == query select q)
                              .FirstOrDefaultAsync().ConfigureAwait(false);

            if (article == null || queryDb == null)
                return this.BadRequest(new { message = $"Не найденно {id} || {query}" });

            var openedArticle = new OpenedArticle(DateTime.UtcNow, article, queryDb);
            try
            {
                this.context.OpenedArticles.Add(openedArticle);
                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            return this.Ok(new { article.Id, article.Title, Text = article.GetText() });
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
        [ProducesResponseType(404)]
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

            var session = await (from s in this.context.Sessions where s.Id == sessionId select s).FirstOrDefaultAsync()
                              .ConfigureAwait(false);
            try
            {
                this.context.SearchingQueries.Add(new SearchingQuery(query, DateTime.Now, session));

                await this.context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return this.BadRequest(new { message = "Что-то пошло не так" });
            }

            if (!articles.Any()) return this.Ok(null);

            return this.Ok((from a in articles select new { a.Id, a.Title, Text = a.Preview }).ToList());
        }
    }
}