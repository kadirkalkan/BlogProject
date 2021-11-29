using Blog.Filters;
using Blog.Managers;
using Blog.Models.Data;
using Blog.Models.Entity;
using Blog.ViewModels.Article.Create;
using Blog.ViewModels.Article.Edit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [LoggedUser]
    public class ArticleController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ArticleController(IWebHostEnvironment webHostEnvironment, DatabaseContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Create(string yonlen)
        {
            ViewBag.yonlen = yonlen;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model, string yonlen)
        {
            if (ModelState.IsValid)
            {
                var article = new Article()
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = int.Parse(HttpContext.Session.GetString("userId")),
                    ArticlePicture = model.ArticlePicture.GetUniqueNameAndSavePhotoToDisk(_webHostEnvironment)
                };
                _context.Articles.Add(article);
                _context.SaveChanges();
                TempData["message"] = "Article Created.";
                return Redirect(yonlen);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var article = _context.Articles.FirstOrDefault(x => x.Id.Equals(id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
            if (article is not null)
            {
                return View(new EditViewModel() { Id = article.Id, Title = article.Title, Content = article.Content, ArticlePictureName = article.ArticlePicture });
            }
            else
            {
                TempData["error"] = "Data Couldn't find";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = _context.Articles.FirstOrDefault(x => x.Id.Equals(model.Id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
                if (article is null)
                {
                    ViewData["error"] = "Edit Failed.";
                    return View(model);
                }

                article.Title = model.Title;
                article.Content = model.Content;

                if (model.ArticlePicture is not null)
                {
                    article.ArticlePicture = model.ArticlePicture.GetUniqueNameAndSavePhotoToDisk(_webHostEnvironment);
                    FileManager.RemoveImageFromDisk(model.ArticlePictureName, _webHostEnvironment);
                }

                _context.SaveChanges();

                TempData["message"] = "Article Editing Completed.";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var article = _context.Articles.FirstOrDefault(x => x.Id.Equals(id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
            if (article is not null)
            {
                _context.Articles.Remove(article);
                _context.SaveChanges();
                FileManager.RemoveImageFromDisk(article.ArticlePicture, _webHostEnvironment);
                TempData["message"] = "Delete completed";
            }
            else
            {
                TempData["error"] = "Data Couldn't find";
            }
            return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
        }
    }
}
