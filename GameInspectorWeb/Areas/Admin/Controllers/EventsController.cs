using GameInspectorWeb.Areas.Admin.Models;
using GameInspectorWeb.Data;
using GameInspectorWeb.Services;
using GameInspectorWeb.Utilites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Areas.Admin.Controllers
{
    public class EventsController : AdminBaseController
    {

        public EventsController(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public IActionResult Index()
        {
            return View(_db.Articles.ToList());
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(NewArticleViewModel vm, [FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (vm.CoverPhoto != null && vm.CoverPhoto.Length > 0)
                {
                    fileName = vm.CoverPhoto.GenerateFileName();
                    var savePath = Path.Combine(env.WebRootPath, "img", fileName);
                    using FileStream fs = new FileStream(savePath, FileMode.Create);
                    vm.CoverPhoto.CopyTo(fs);
                }

                if (vm.ContentPhotos != null && vm.ContentPhotos.Count > 0)
                {
                    string resim = null;
                    List<string> paths = new List<string>();
                    string path = null;
                    foreach (var photo in vm.ContentPhotos)
                    {
                        path = Guid.NewGuid() + "_" + photo.FileName;
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                        var filePath = Path.Combine(savePath, path);
                        await photo.CopyToAsync(new FileStream(filePath, FileMode.Create));
                        paths.Add(path);

                    }
                    resim = string.Join(",", paths);
                    //var data = resim.Split(',');

                    var article = new Article()
                    {
                        Title = vm.Title,
                        Content = vm.Content,
                        CharacterContent = vm.CharacterContent,
                        Time = vm.Time,
                        VideoPath = vm.VideoPath,
                        CoverPhotoPath = fileName,
                        ContentPhotosPaths = resim
                    };
                    _db.Articles.Add(article);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Article article = _db.Articles.Find(id);

            if (article == null)
            {
                return NotFound();
            }

            var vm = new EditArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CharacterContent = article.CharacterContent,
                Time = DateTime.Now,
                VideoPath = article.VideoPath,
                ExistingCoverPhotoPath = article.CoverPhotoPath,
                ExistingContentPhotoPath = article.ContentPhotosPaths
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditArticleViewModel vm, [FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (vm.CoverPhoto != null && vm.CoverPhoto.Length > 0)
                {
                    fileName = vm.CoverPhoto.GenerateFileName();
                    var savePath = Path.Combine(env.WebRootPath, "img", fileName);
                    using FileStream fs = new FileStream(savePath, FileMode.Create);
                    await vm.CoverPhoto.CopyToAsync(fs);
                }
                string resim = null;
                List<string> paths = new List<string>();
                string path = null;
                foreach (var photo in vm.ContentPhotos)
                {
                    path = Guid.NewGuid() + "_" + photo.FileName;
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                    var filePath = Path.Combine(savePath, path);
                    await photo.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    paths.Add(path);
                }
                resim = string.Join(",", paths);
                //var data = resim.Split(',');
                var article = _db.Articles.Find(vm.Id);
                article.Title = vm.Title;
                article.Content = vm.Content;
                article.CharacterContent = vm.CharacterContent;
                article.Time = DateTime.Now;
                article.VideoPath = vm.VideoPath;
                article.CoverPhotoPath = fileName;
                article.ContentPhotosPaths = resim;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var article = _db.Articles.Find(id);

            if (article == null)
            {
                return NotFound();
            }

            _db.Remove(article);
            _db.SaveChanges();
            return RedirectToAction("Index");
            
        }
    }
}

