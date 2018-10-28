using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.EF.Models;
using WebApi.Tools.Finder;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly EFContext _context;

        public ArticleController(EFContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetArticle(int id)
        {
            var article = await (from a in _context.Articles where a.Id == id select new { a.Id, a.Title, a.Text }).FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound(id);
            }

            return Ok(article);
        }

        [HttpGet]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetArticle([FromQuery]string query)
        {
            var articles = await Task.Run(() =>
             {
                 var finder = new ArticleFinder(_context);
                 return finder.GetArticlesByQuery(query);
             });

            if (articles == null || !articles.Any())
            {
                return NotFound();
            }

            return Ok(from a in articles select new { a.Id, a.Title, Text = a.Text.Substring(0, 75) });
        }
    }

    public class TestPost
    {
        public string Text { get; set; }
    }
}