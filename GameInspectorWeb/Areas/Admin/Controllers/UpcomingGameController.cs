using GameInspectorWeb.Areas.Admin.Models;
using GameInspectorWeb.Data;
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
    public class UpcomingGameController : AdminBaseController
    {


        public UpcomingGameController(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public IActionResult Index()
        {
            return View(_db.UpcomingGames.ToList());
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(UpcomingGameViewModel vm,[FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (vm.Photo != null && vm.Photo.Length > 0)
                {
                    fileName = vm.Photo.GenerateFileName();
                    var savePath = Path.Combine(env.WebRootPath, "img", fileName);
                    using FileStream fs = new FileStream(savePath, FileMode.Create);
                    vm.Photo.CopyTo(fs);
                }

                var upcomingGame = new UpcomingGames()
                {
                    Title = vm.Title,
                    Time = vm.Time,
                    PhotoPath = fileName,
                    Platform = vm.Platform
                };

                _db.UpcomingGames.Add(upcomingGame);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            UpcomingGames upcomingGames = _db.UpcomingGames.Find(id);

            if (upcomingGames == null)
            {
                return NotFound();
            }

            var vm = new EditUpcomingGameViewModel()
            {
                Id = upcomingGames.Id,
                Title = upcomingGames.Title,
                PhotoPath = upcomingGames.PhotoPath,
                Platform = upcomingGames.Platform,
                Time = upcomingGames.Time
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(EditUpcomingGameViewModel vm,[FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (vm.Photo != null && vm.Photo.Length > 0)
                {
                    fileName = vm.Photo.GenerateFileName();
                    var savePath = Path.Combine(env.WebRootPath, "img", fileName);
                    using FileStream fs = new FileStream(savePath, FileMode.Create);
                    vm.Photo.CopyTo(fs);
                }

                var upcomingGames = _db.UpcomingGames.Find(vm.Id);

                upcomingGames.Title = vm.Title;
                upcomingGames.PhotoPath = fileName;
                upcomingGames.Platform = vm.Platform;
                upcomingGames.Time = vm.Time;

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var upcomingGames = _db.UpcomingGames.Find(id);

            if (upcomingGames == null)
            {
                return NotFound();
            }

            _db.Remove(upcomingGames);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}
