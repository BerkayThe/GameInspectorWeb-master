using GameInspectorWeb.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameInspectorWeb.Services
{
    public class MostViewedArticleHostedService : BackgroundService
    {
        protected IServiceProvider _serviceProvider;
        protected IMostViewedArticleService _mostViewedArticleService;

        public MostViewedArticleHostedService([NotNull] IServiceProvider serviceProvider, [NotNull] IMostViewedArticleService mostViewedArticleService)
        {
            _serviceProvider = serviceProvider;
            _mostViewedArticleService = mostViewedArticleService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var hostedServicesDbContext = (ApplicationDbContext)scope.ServiceProvider.GetRequiredService(typeof(ApplicationDbContext));
                    //var timeFrom = DateTimeOffset.Now.AddSeconds(-60);
                    _mostViewedArticleService.MostViewedArticles = GetMostViewedArticles(hostedServicesDbContext);
                }

                await Task.Delay(new TimeSpan(0, 0, 1));
            }
        }

        public static List<MostViewedArticleView> GetMostViewedArticles(ApplicationDbContext hostedServicesDbContext)
        {
            var mostViewedArticles = hostedServicesDbContext.Set<ArticleHit>()
                                    .Join(
                                        hostedServicesDbContext.Set<Article>(),
                                        articleHit => articleHit.ArticleId,
                                        article => article.Id,
                                        (articleHit, article) => new { ArticleHit = articleHit, Article = article }
                                    )
                                    //.Where(g => g.ArticleHit.Created >= timeFrom)
                                    .GroupBy(g => g.Article.Id)
                                    .Select(g => new MostViewedArticleView { ArticleId = g.Key, Title = g.Min(t => t.Article.Title), Hits = g.Count() })
                                    .OrderByDescending(g => g.Hits)
                                    .ToList();
            return mostViewedArticles;
        }
    }
}
