using GameInspectorWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleSearchController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ArticleSearchController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var article = _db.Articles.Where(x => x.Title.Contains(term)).Select(x => new { x.Title,x.Id,x.CoverPhotoPath }).ToList();
                return Ok(article);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

    }
}
