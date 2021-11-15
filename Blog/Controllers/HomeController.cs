using Blog.Filters;
using Blog.Models;
using Blog.Models.Data;
using Blog.ViewModels.Home.Overview;
using Blog.ViewModels.Home.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Articles
                .OrderByDescending(x => x.CreatedTime)
                .Take(20)
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId.ToString(),
                    AuthorName = x.Author.Username,
                    ArticlePicture = string.IsNullOrEmpty(x.ArticlePicture) ? "null.png" : x.ArticlePicture,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime
                }).ToList();
            return View(list);
        }

        [LoggedUser]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("[controller]/[action]/{username}")]
        public IActionResult Profile(string username)
        {
            List<ArticleViewModel> list =
                _context.Articles
                .Where(x => x.Author.Username.Equals(username))
                .OrderByDescending(x => x.CreatedTime)
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId.ToString(),
                    AuthorName = x.Author.Username,
                    ArticlePicture = string.IsNullOrEmpty(x.ArticlePicture) ? "null.png" : x.ArticlePicture,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime
                }).ToList();
            return View(list);
        }

        public IActionResult Overview(int id)
        {

            /*
             *  İster Select Kullan İstersen Sonradan Class Oluşturup O Class'a Değerleri Ata.
             *  Ama Önerilen Select Kullanımıdır.
             
                 var model = _context.Articles
                .Select(x => new OverviewViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    ArticlePicture = x.ArticlePicture,
                    Author = x.Author.Username,
                    CreatedTime = x.CreatedTime
                })
                .FirstOrDefault(x => x.Id.Equals(id));                
             
             */

            var model = _context.Articles
              .Select(x => new OverviewViewModel()
              {
                  Id = x.Id,
                  Title = x.Title,
                  Content = x.Content,
                  ArticlePicture = x.ArticlePicture,
                  Author = x.Author.Username,
                  CreatedTime = x.CreatedTime
              })
              .FirstOrDefault(x => x.Id.Equals(id));

            /*


            var data = _context.Articles.Include(x=>x.Author)
                .FirstOrDefault(x => x.Id.Equals(id));

            var model = new OverviewViewModel()
            {
                Id = data.Id,
                Title = data.Title,
                Content = data.Content,
                ArticlePicture = data.ArticlePicture,
                Author = data.Author.Username,
                CreatedTime = data.CreatedTime
            };
             
             */
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
