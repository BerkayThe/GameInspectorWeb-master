using GameInspectorWeb.Data;
using GameInspectorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Resim(Article article)
        {
            if (article.ContentPhotos.Count > 0)
            {
                foreach (var photo in article.ContentPhotos)
                {
                    var path = Guid.NewGuid() + "_" + photo.FileName;
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                    var filePath = Path.Combine(savePath, path);
                    await photo.CopyToAsync(new FileStream(filePath, FileMode.Create));
                }
            }

            return RedirectToAction("Index");
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
    }
}
