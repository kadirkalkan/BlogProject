using Blog.Filters;
using Blog.Managers;
using Blog.Models.Data;
using Blog.Models.Entity;
using Blog.ViewModels.Article.Create;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly FileManager _fileManager;
        private readonly DatabaseContext _context;

        public ArticleController(IWebHostEnvironment webHostEnvironment, DatabaseContext context)
        {
            _fileManager = new FileManager(webHostEnvironment);
            _context = context;
        }


        [LoggedUser]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article()
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = int.Parse(HttpContext.Session.GetString("userId")),
                    ArticlePicture = _fileManager.GetUniqueNameAndSavePhotoToDisk(model.ArticlePicture)
                };
                _context.Articles.Add(article);
                _context.SaveChanges();
            }
            TempData["message"] = "Tebrikler Article Oluşturma Başarılı.";
            return RedirectToAction("Profile", "Home", new { username= HttpContext.Session.GetString("username") });
        }
    }
}
