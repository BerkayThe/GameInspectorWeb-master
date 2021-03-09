using GameInspectorWeb.Data;
using GameInspectorWeb.Models;
using GameInspectorWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameInspectorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMostViewedArticleService _mostViewedArticleService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, IMostViewedArticleService mostViewedArticleService)
        {
            _logger = logger;
            _db = applicationDbContext;
            _mostViewedArticleService = mostViewedArticleService;

        }

        public async Task<IActionResult> Index()
        {


            ViewBag.CategoryList = _db.Categories.ToList();



            //var artCatVm = _db.Articles.Join(_db.ArticleCategories, art => art.Id, ac => ac.ArticleId,
            //    (art, ac) => new { art, ac }).Join(_db.Categories, at => at.ac.CategoryId, cat => cat.Id, (at, cat) => new ArticleCategoryViewModel()
            //    {
            //        ArticleContent = at.art.Content,
            //        ArticleId = at.art.Id,
            //        ArticleTitle = at.art.Title,
            //        CoverPhotoPath = at.art.CoverPhotoPath,
            //        Time = at.art.Time,
            //        CategoryId = cat.Id,
            //        CategoryName = cat.CategoryName
            //    }).Distinct().ToList();



            TempData["MostViewed"] = await GetHits();

            ViewBag.UpcomingGames = _db.UpcomingGames.Take(10).ToList();

            ViewBag.Art = _db.Articles.OrderByDescending(x => x.Time).Take(5).ToList();

            var vm = _db.Articles.OrderByDescending(x => x.Time).ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ArticleContent(int id)
        {
            if (HttpContext.Session.GetString(id.ToString()) == null)
            {
                HttpContext.Session.SetString(id.ToString(), Guid.NewGuid().ToString());
                await SendHitRequest(id);
            }

            TempData["MostViewed"] = await GetHits();
            ViewBag.GetComments = _db.Comments.Include("Author").ToList();

            if (id != 0)
            {
                var data = _db.Articles.Find(id);
                if (data != null)
                {
                    return View(data);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult ArticleContent(Comment comment)
        {

            comment.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.ArticleId = comment.Id;
            comment.Id = 0;
            if (ModelState.IsValid)
            {
                _db.Comments.Add(comment);
                _db.SaveChanges();
            }
            return RedirectToAction("ArticleContent", new { id = comment.ArticleId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult GetData(int pageIndex = 0, int pageSize = 6, int catId = 0)
        {
            var artCatVm = new List<ArticleCategoryViewModel>();
            if (catId > 0)
            {

                artCatVm = _db.Set<Article>()
                                        .Join(
                                            _db.Set<ArticleCategory>(),
                                            art => art.Id,
                                            artCat => artCat.ArticleId,
                                            (art, artCat) => new { art = art, artCat = artCat }
                                        ).Join(
                                             _db.Set<Category>(),
                                            cat => cat.artCat.CategoryId,
                                            catId => catId.Id,
                                            (cat, catId) => new ArticleCategoryViewModel()
                                            {
                                                ArticleContent = cat.artCat.Article.Content,
                                                ArticleId = cat.artCat.Article.Id,
                                                ArticleTitle = cat.artCat.Article.Title,
                                                CoverPhotoPath = cat.artCat.Article.CoverPhotoPath,
                                                Time = cat.artCat.Article.Time,
                                                CategoryId = cat.artCat.CategoryId,
                                                CategoryName = cat.artCat.Category.CategoryName,
                                                Comments = cat.artCat.Article.Comments.Count()
                                            }
                                        )
                                        .Where(x => x.CategoryId == catId)
                                        .OrderByDescending(x => x.Time)
                                        .Skip(pageIndex * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            }
            else
            {
                artCatVm = _db.Set<Article>().Select(x => new ArticleCategoryViewModel()
                {
                    ArticleContent = x.Content,
                    ArticleId = x.Id,
                    ArticleTitle = x.Title,
                    CoverPhotoPath = x.CoverPhotoPath,
                    Time = x.Time,
                    Comments = x.Comments.Count()
                }).OrderByDescending(x => x.Time)
                 .Skip(pageIndex * pageSize)
                 .Take(pageSize)
                 .ToList();
            }


            return Json(artCatVm);
        }

        [NonAction]
        public async Task SendHitRequest(int id)
        {
            var article = _db.Articles.Find(id);
            if (article != null)
            {
                var content = new ArticleHit
                {
                    ArticleId = article.Id,
                    Created = DateTimeOffset.Now,
                    LastUpdate = DateTimeOffset.Now
                };
                var json = JsonConvert.SerializeObject(content);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "https://localhost:44382/api/ArticleHit/create-async";
                using var client = new HttpClient();

                await client.PostAsync(url, data);
            }
        }

        [NonAction]
        public async Task<List<HitViewModel>> GetHits()
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:44382/api/ArticleHit/most-viewed");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<MostViewedArticleView>>(json);
            List<HitViewModel> liste = new List<HitViewModel>();
            if (data.Any())
            {
                foreach (var item in data)
                {
                    liste.Add(_db.Articles.Where(x => x.Id == item.ArticleId).Select(t => new HitViewModel
                    {
                        ArticleId = item.ArticleId,
                        Title = t.Title,
                        PhotoPath = t.CoverPhotoPath,
                        Hits = item.Hits
                    }).FirstOrDefault());
                }
                return liste;
            }
            else
            {
                return new List<HitViewModel>();
            }
        }
    }
}
