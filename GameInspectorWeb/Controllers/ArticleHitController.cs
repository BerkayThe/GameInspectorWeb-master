using GameInspectorWeb.Data;
using GameInspectorWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleHitController : ControllerBase
    {
        protected readonly ApplicationDbContext _applicationDbContext;
        protected readonly IMostViewedArticleService _mostViewedArticleService;

        public ArticleHitController([NotNull] ApplicationDbContext hostedServicesDbContext, [NotNull] IMostViewedArticleService mostViewedArticleService)
        {
            _applicationDbContext = hostedServicesDbContext;
            _mostViewedArticleService = mostViewedArticleService;

        }

        [HttpPost("create-async")]
        public async Task<IActionResult> CreateAsync(ArticleHit entity)
        {
            await _applicationDbContext.Set<ArticleHit>().AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpGet("most-viewed")]
        public IActionResult GetMostViewed()
        {
            return Ok(_mostViewedArticleService.MostViewedArticles);
        }
    }
}
