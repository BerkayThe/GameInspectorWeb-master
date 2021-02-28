using GameInspectorWeb.Data;
using GameInspectorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameInspectorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _db = applicationDbContext;
        }

        public IActionResult Index(int pageIndex = 0, int pageSize = 6)
        {
            ViewBag.UpcomingGames = _db.UpcomingGames.Take(10).ToList();

            ViewBag.Kategori = _db.Categories.Select(x => x.CategoryName).ToList();

            ViewBag.Art = _db.Articles.OrderByDescending(x => x.Time).Take(5).ToList();

            var vm = _db.Articles.OrderByDescending(x => x.Time).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult ArticleContent(int id)
        {
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
        public JsonResult GetData(int pageIndex = 0, int pageSize = 6)
        {
            var vm = _db.Articles.OrderByDescending(x => x.Time).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Json(vm.ToList());
        }
    }
}
